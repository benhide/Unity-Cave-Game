using UnityEngine;

// Monster attack class
public class MonsterAttack : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Monster attack settings")]
    public int attackDamage = 5;
    public bool playerInRange = false;

    [Header("Monster attack timers")]
    public float hitTimer = 0.0f;
    public float timeBetweenAttacks = 1.0f;

    [Header("Script references")]
    public MonsterController monsterController;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Script references
        monsterController = transform.parent.GetComponent<MonsterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Add the time since Update was last called to the timer.
        hitTimer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (hitTimer >= timeBetweenAttacks && playerInRange && !monsterController.monsterHealth.MonsterDead())
        {
            // Attack.
            Attack();
        }

        // Dont attack
        else
        {
            monsterController.animator.SetBool(AnimationNames.monsterAttacking, false);
        }
    }

    // Attack the player
    void Attack()
    {
        // Reset the timer.
        hitTimer = 0.0f;

        // If the player has health to lose...
        if (monsterController.playerController.CurrentHealth() > 0)
        {
            // Damage the player.
            monsterController.animator.SetBool(AnimationNames.monsterAttacking, true);
            monsterController.playerController.PlayerDamaged(attackDamage);
        }
    }

    // OnTriggerStay is called when the Collider other stays the trigger
    void OnTriggerEnter(Collider other)
    {
        // If the other collider is the player
        if (other.gameObject == monsterController.player && !monsterController.monsterHealth.MonsterDead())
        {
            playerInRange = true;
        }
    }

    // OnTriggerExit is called when the Collider other exits the trigger
    void OnTriggerExit(Collider other)
    {
        // If the other collider is the player
        if (other.gameObject == monsterController.player)
        {
            // Player in range
            playerInRange = false;

            // Set the hit timer to 0
            hitTimer = 0.0f;
            monsterController.animator.SetBool(AnimationNames.monsterAttacking, false);
        }
    }

    ///////////////////////End of Functions/////////////////////////
}