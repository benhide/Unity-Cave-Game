using UnityEngine;

// Monster controller class
public class MonsterController : MonoBehaviour
{
    [Header("Monster type")]
    public MONSTERTYPE monsterType;

    [Header("Component references")]
    public CharacterController characterController;
    public ParticleSystem damagedSystem;
    public ParticleSystem destroyedSystem;
    public Animator animator;

    [Header("Script references")]
    public MonsterSight monsterSight;
    public MonsterHealth monsterHealth;
    public MonsterMovement monsterMovement;
    public PlayerController playerController;
    public GameController gameController;

    [Header("GameObject references")]
    public GameObject player;
    int index;

    // Use this for initialization
    void Start()
    {
        // Find the player and playercontroller
        player = GameObject.FindGameObjectWithTag(Tags.playerTag);
        playerController = player.GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();

        // Assign the reference to the controls and game data manager
        gameController = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<GameController>();

        // Assign the script component references
        monsterSight = GetComponent<MonsterSight>();
        monsterHealth = GetComponent<MonsterHealth>();
        monsterMovement = GetComponent<MonsterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        monsterSight.SightUpdate();
        monsterMovement.MovementUpdate();
        monsterHealth.HealthUpdate();
    }

    public MONSTERTYPE typeOfMonster()
    {
        return monsterType;
    }

    public void typeOfMonster(MONSTERTYPE type)
    {
        monsterType = type;
    }

    public void Index(int index)
    {
        this.index = index;
    }

    public int Index()
    {
        return index;
    }
}