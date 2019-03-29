#pragma warning disable 0414
using UnityEngine;
using UnityEngine.UI;

// Player controller class
public class PlayerController : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    // Player actions
    bool diggingAttacking = false;
    bool actionsEnabled = false;
    bool action = false;

    [Header("Player Damaged Settings")]
    bool playerDamaged = false;
    bool playerDead = false;
    public Image damageImage;
    public float flashSpeed = 0.5f;
    public Color flashColour = new Color(1.0f, 0.0f, 0.0f, 0.5f);

    [Header("Player Controller Settings")]
    public float movementSpeed;
    public float rotationSpeedMultiplier;
    float lockedYPosition = 0.0f;
    Vector3 directionVector = Vector3.zero;
    Vector3 playerVelocity = Vector3.zero;

    // Player poition and rotation
    Vector3 currentPosition = Vector3.zero;
    Vector3 lastPosition = Vector3.zero;
    Vector3 currentRotation = Vector3.zero;
    Vector3 lastRotation = Vector3.zero;

    [Header("Player health and points")]
    public Text healthText;
    public Text scoreText;
    public int currentHealth;
    public int maxHealth;
    int pointScore = 0;

    [Header("Player TNT")]
    public GameObject TNT;
    public Sprite TNTSprite;
    HUDItem[] TNTs = new HUDItem[3];
    int TNTCount = 3;

    [Header("Player SFXs and audio sources")]
    public AudioSource footsteps;
    public AudioSource soundFx;
    public AudioClip slash;
    public AudioClip damaged;

    [Header("Script references")]
    public GameController gameController;
    public Controls controls;
    public Map map;
    public HUD hud;

    [Header("Component references")]
    public RuntimeAnimatorController maleAnimator;
    public RuntimeAnimatorController femaleAnimator;
    private Animator animator;
    private CharacterController characterController;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the references
        hud = GameObject.FindGameObjectWithTag(Tags.uiTag).GetComponent<HUD>();
        map = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<Map>();
        gameController = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<GameController>();
        controls = Controls.instance;

        // Assign the reference to the animator, charactercontroller and healthbar
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();

        // Initialise the player
        InitialisePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the current and last positions and rotations of the player
        SetPositionsAndRotations();

        // Record player position (for every secs passed)
        if (gameController.PlayerSpawned())
            GameDataManager.instance.RecordPlayerPosition(transform.position);

        // Set the players health bar
        SetHealthText();
        SetScoreText();

        // If the player has just been damaged
        if (playerDamaged)
            damageImage.color = flashColour;
        else
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

        // Reset the damaged flag
        playerDamaged = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Move the player and player actions
        MovePlayer();
        AnimatePlayerMovement();

        // Quests start weapons active
        if (actionsEnabled)
        {
            Actions();
            AnimatePlayerDigging();
        }
    }

    // Initialise the player 
    void InitialisePlayer()
    {
        // Set the animator controller of the animator - either male or female controller
        if (GameDataManager.instance.SelectedGender() == GENDER.MALE)
            animator.runtimeAnimatorController = maleAnimator;

        else if (GameDataManager.instance.SelectedGender() == GENDER.FEMALE)
            animator.runtimeAnimatorController = femaleAnimator;

        // Set the last and current position
        currentPosition = transform.position;
        lastPosition = currentPosition;

        // Set the last and current rotation
        currentRotation = transform.rotation.eulerAngles;
        lastRotation = currentRotation;

        // Set the players health
        currentHealth = maxHealth;
    }

    // Set the current and last positions and rotations of the player
    void SetPositionsAndRotations()
    {
        // Set the last and current position
        lastPosition = currentPosition;
        currentPosition = transform.position;

        // Set the last and current rotation
        lastRotation = currentRotation;
        currentRotation = transform.rotation.eulerAngles;
    }

    // Rotate the game object to face away from closest wall
    public void SetPlayerRotation()
    {
        // Get the gameobjects position
        Vector3 position = transform.position;

        // Rotate gameobject to face out from walls
        if (position.x <= map.GridWidth() / 2 && position.z <= map.GridHeight() / 2)
            transform.Rotate(Vector3.up, 90.0f, Space.Self);

        if (position.x <= map.GridWidth() / 2 && position.z > map.GridHeight() / 2)
            transform.Rotate(Vector3.up, 180.0f, Space.Self);

        if (position.x > map.GridWidth() / 2 && position.z > map.GridHeight() / 2)
            transform.Rotate(Vector3.up, -90.0f, Space.Self);
    }

    // Move the player
    void MovePlayer()
    {
        // The movement direction from the input
        directionVector = new Vector3(controls.horizontal, 0.0f, controls.vertical).normalized;

        // If the dirVector is not equal to zero rotate the player to face the dirVector
        if (directionVector != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * rotationSpeedMultiplier);

        // Set the direction vector and move Character Controller
        playerVelocity = directionVector * movementSpeed;
        characterController.Move(playerVelocity * Time.deltaTime);

        // Lock the transform position y to 0
        if (transform.position.y != lockedYPosition)
            transform.position = new Vector3(transform.position.x, lockedYPosition, transform.position.z);
    }

    // Player actions
    void Actions()
    {
        // Set if the player is digging
        diggingAttacking = controls.diggingAttacking && !animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationNames.playerDiggingAttacking);

        // Drop a tnt
        if (controls.dropTNT && TNTCount > 0)
        {
            // Drop tnt and reduce tnt count
            Instantiate(TNT, transform.position, Quaternion.identity);
            TNTCount--;
            hud.RemoveTNT(TNTs[TNTCount]);
            GameDataManager.instance.RecordTNTPosition(transform.position);
        }

        // Set the action bool
        action = controls.otherAction;
    }

    // Add TNT for player
    public void AddTNT()
    {
        // If less than max TNT add new TNT
        while (TNTCount < 3)
        {
            TNTs[TNTCount] = new HUDItem() { ID = TNTCount, HUDSprite = TNTSprite };
            hud.AddTNT(TNTs[TNTCount]);
            TNTCount++;
        }
    }

    // Animate the player
    void AnimatePlayerMovement()
    {
        // If the player is moving (position or rotation changed)
        if ((controls.horizontal != 0 || controls.vertical != 0) && currentPosition != lastPosition || currentRotation != lastRotation)
        {
            // Play the moving animation
            animator.SetBool(AnimationNames.playerMoving, true);

            // If the footstep sfx is not playing
            if (!footsteps.isPlaying)
            {
                // Play the footstep sfx and loop
                footsteps.loop = true;
                footsteps.Play();
            }
        }

        // Else player is not moving
        else
        {
            // Stop animation and looping
            animator.SetBool(AnimationNames.playerMoving, false);
            footsteps.loop = false;
        }
    }

    // Animate the player digging
    void AnimatePlayerDigging()
    {
        // If the player is digging/atttacking
        if (diggingAttacking)
        {
            // Play digging animation and sfx
            animator.SetBool(AnimationNames.playerDiggingAttacking, true);
            soundFx.PlayOneShot(slash);
        }

        // Set digging as false in animator
        else
            animator.SetBool(AnimationNames.playerDiggingAttacking, false);
    }

    // Get the player health
    public float CurrentHealth()
    {
        return currentHealth;
    }

    // Get the player health
    public float MaxHealth()
    {
        return maxHealth;
    }

    // Set the player health text
    void SetHealthText()
    {
        healthText.text = currentHealth.ToString();
        GameDataManager.instance.SetPlayerHealth(currentHealth);
    }

    // Take damage from attacks
    public void PlayerDamaged(int damage)
    {
        // Reduce health
        currentHealth -= damage;
        if (currentHealth <= 0) currentHealth = 0;
        playerDamaged = true;

        // Play sound fx
        soundFx.PlayOneShot(damaged);

        // Set health text
        SetHealthText();
    }

    // Access to the digging attacking bool
    public bool IsDiggingAttacking()
    {
        return diggingAttacking;
    }

    // Access to the action bool
    public bool IsAction()
    {
        return action;
    }

    // Enable player actions
    public void ActionsEnabled()
    {
        actionsEnabled = true;

        // Initialse the tnt
        for (int i = 0; i < TNTCount; i++)
        {
            TNTs[i] = new HUDItem() { ID = i, HUDSprite = TNTSprite };
            hud.AddTNT(TNTs[i]);
        }
    }

    // Add points to player score
    public void AddToScore(int points)
    {
        pointScore += points;
    }

    // Add points to player health
    public void AddToHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    // Set the player score text
    void SetScoreText()
    {
        scoreText.text = pointScore.ToString();
        GameDataManager.instance.SetPlayerScore(pointScore);
    }

    // Is the player dead
    public bool PlayerDead()
    {
        return playerDead;
    }

    // Player is dead
    public void PlayerDead(bool dead)
    {
        playerDead = dead;
    }

    ///////////////////////End of Functions/////////////////////////
}