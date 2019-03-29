using UnityEngine;

// Monster health class
public class MonsterHealth : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Monster health and type settings")]
    public int currentHealth;
    public int maxHealth;
    public int pointsScore;
    public float targetScale;
    public float shrinkSpeed;
    public float deathTimer;
    public bool monsterDead = false;
    public GameObject bodyShadow;
    public AudioClip monsterDamaged;

    [Header("Script references")]
    public MonsterController monsterController;
    public QuestManager questManager;
    public MonsterManager monsterManager;

    // Event callbacks
    public delegate void OnMonsterKilledEvent(GameObject monster);
    public static event OnMonsterKilledEvent onMonsterKilled;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Script references
        questManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<QuestManager>();
        monsterManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<MonsterManager>();
        monsterController = GetComponent<MonsterController>();
    }

    // Update is called once per frame
    public void HealthUpdate()
    {
        // If the monster has no health
        if (currentHealth == 0)
        {
            // If the monster has no health (not given dead status yet)
            if (!monsterDead)
            {
                // Play the death particles 
                monsterController.playerController.AddToScore(pointsScore);

                // If the monster is a basic monster record the monster kill position
                if (monsterController.monsterType == MONSTERTYPE.BASIC)
                {
                    // Killed in quest flag
                    bool killedInQuest;

                    // Is the monster killed during quest
                    if (questManager.MonsterQuest()) killedInQuest = true;
                    else killedInQuest = false;

                    // Record themonster killed
                    GameDataManager.instance.MonsterKilled(MONSTERTYPE.BASIC, monsterController.Index(), transform.position, killedInQuest);
                }

                // If the monsters is a juicy monster record the monster kill position
                if (monsterController.monsterType == MONSTERTYPE.JUICY)
                {
                    // Killed in quest flag
                    bool killedInQuest;

                    // Is the monster killed during quest
                    if (questManager.MonsterQuest()) killedInQuest = true;
                    else killedInQuest = false;

                    // Record themonster killed
                    GameDataManager.instance.MonsterKilled(MONSTERTYPE.JUICY, monsterController.Index(), transform.position, killedInQuest);

                    // Play the juicy particle death
                    ParticleSystemController.InstaniateParticleSystem(monsterController.destroyedSystem, transform.position, Quaternion.identity);
                }
            }

            // The monster is dead
            monsterDead = true;

            // If the monster is a basic monster shrink it on death
            if (monsterController.monsterType == MONSTERTYPE.BASIC)
            {
                // Shrink the monster
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime * shrinkSpeed);
                Destroy(bodyShadow);
                monsterController.characterController.enabled = false;
                deathTimer -= Time.deltaTime;
            }

            // If the monster is a juicy monster disable sprite
            else if (monsterController.monsterType == MONSTERTYPE.JUICY)
            {
                GetComponentInChildren<SpriteRenderer>().enabled = false;
                Destroy(bodyShadow);
                monsterController.characterController.enabled = false;
                deathTimer -= Time.deltaTime;
            }

            // When the monster has finished dying
            if (deathTimer <= 0.0f)
            {
                // Monster quest
                if (questManager.MonsterQuest() && questManager.MonstersKilled() < questManager.TotalMonsters())
                {
                    if (onMonsterKilled != null)
                        onMonsterKilled.Invoke(gameObject);
                }

                // Destroy the gameobject
                Destroy(gameObject);

                // Spawn new monster
                monsterManager.SpawnMonster(monsterManager.monsterHealth);
            }
        }
    }

    // Get the monsters death state
    public bool MonsterDead()
    {
        return monsterDead;
    }

    // Get the monsters health
    public int CurrentHealth()
    {
        return currentHealth;
    }

    // Set the monsters health
    public void SetCurrentAndMaxHealth(int health)
    {
        currentHealth = maxHealth = health;
    }

    // Damage the monster
    public void DamageMonster(int damage)
    {
        // Remove damage from health
        currentHealth -= damage;

        // Play the damaged particles and SFX
        monsterController.damagedSystem.Play();
        monsterManager.source.PlayOneShot(monsterDamaged);

        // Set the player in sight and set the chase timer
        monsterController.monsterSight.PlayerInSight(true);
    }

    ///////////////////////End of Functions/////////////////////////
}
