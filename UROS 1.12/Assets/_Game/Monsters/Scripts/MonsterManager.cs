using UnityEngine;

// Monster type enum
public enum MONSTERTYPE { BASIC, JUICY }

public class MonsterManager : MonoBehaviour
{
    [Header("Monster and player GameObjects")]
    public GameObject basicMonster;
    public GameObject juicyMonster;
    private GameObject monsters;
    private GameObject player;
    public Sprite monsterSprite;

    [Header("Monster statistics")]
    public int totalNumberOfMonsters;
    public int totalMonstersKilled;
    public int numberOfBasicMonsters;
    public int basicMonstersKilled;
    public int numberOfJuicyMonsters;
    public int juicyMonstersKilled;

    [Header("Monster settings")]
    [Range(1, 20)]
    public int minBasicMonsters;
    [Range(1, 20)]
    public int maxBasicMonsters;
    [Range(1, 20)]
    public int minJuicyMonsters;
    [Range(1, 20)]
    public int maxJuicyMonsters;
    [Range(1, 10)]
    public int monsterHealth;
    [Range(1, 20)]
    public float minMonsterToPlayerDistance;
    public Color monsterColourBasic;
    public Color monsterColourJuicy;

    [Header("Script references")]
    public Map map;
    public AudioSource source;


    // Use this for initialization
    void Start()
    {
        // Assign the reference to the game data manager
        map = GetComponent<Map>();

        // Assign the player and other gameobjects
        player = GameObject.FindGameObjectWithTag(Tags.playerTag);
        monsters = GameObject.FindGameObjectWithTag(Tags.monstersTag);
        source = GameObject.FindGameObjectWithTag(Tags.sfxsTag).GetComponent<AudioSource>();

        // Clamp the min/max monster spawn settings
        ClampMinMaxValues();

        // Set the colours
        SetColours();
    }

    // Spawn monsters in random positions
    public void SpawnMonsters()
    {
        // Set the number of monsters
        RandomMonsterNumber();

        // Loop through all basic monsters
        for (int i = 0; i < numberOfBasicMonsters; i++)
        {
            // Instatiate the monsters and position in the map
            GameObject basicMon = Instantiate(basicMonster, Vector3.zero, Quaternion.identity, monsters.transform) as GameObject;

            // Spawn in the basic monster
            SpawnMonster(basicMon, MONSTERTYPE.BASIC, monsterHealth);
            basicMon.GetComponentInChildren<SpriteRenderer>().color = monsterColourBasic;
        }

        // Loop through all juicy monsters
        for (int i = 0; i < numberOfJuicyMonsters; i++)
        {
            // Instatiate the monsters and position in the map
            GameObject juicyMon = Instantiate(juicyMonster, Vector3.zero, Quaternion.identity, monsters.transform) as GameObject;

            // Spawn in the basic monster
            SpawnMonster(juicyMon, MONSTERTYPE.JUICY, monsterHealth);
            juicyMon.GetComponentInChildren<SpriteRenderer>().color = monsterColourJuicy;
        }

        Debug.Log("MONSTER - MONSTER INIT - COMPLETE");
    }

    // Set the colours
    public void SetColours()
    {
        // Set colours CVD
        if (GameDataManager.instance.SelectedColourScheme() == COLOURSCHEME.CVD)
        {
            // Red safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.RED)
            {
                monsterColourBasic = Colours.basicMonColourRedSafe;
                monsterColourJuicy = Colours.juicyMonColourRedSafe;
            }

            // Green safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.GREEN)
            {
                monsterColourBasic = Colours.basicMonColourGreenSafe;
                monsterColourJuicy = Colours.juicyMonColourGreenSafe;
            }

            // Blue safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.BLUE)
            {
                monsterColourBasic = Colours.basicMonColourBlueSafe;
                monsterColourJuicy = Colours.juicyMonColourBlueSafe;
            }
        }

        // Set colours Normal
        else if (GameDataManager.instance.SelectedColourScheme() == COLOURSCHEME.NORMAL)
        {
            monsterColourBasic = Colours.basicMonColour;
            monsterColourJuicy = Colours.juicyMonColour;
        }
    }

    // Spawn in a monster
    public void SpawnMonster(GameObject monster, MONSTERTYPE monsterType, int monsterHealth)
    {
        // Set initial position
        map.PositionGameObjectInEmptyCube(monster);

        // Set the monster type and health
        MonsterController monsterController = monster.GetComponent<MonsterController>();
        monsterController.typeOfMonster(monsterType);
        monsterController.GetComponent<MonsterHealth>().SetCurrentAndMaxHealth(monsterHealth);

        // While the monster is closer to the player than minimum monster to player distance reposition the monster
        while (Vector3.Distance(monster.transform.position, player.transform.position) < minMonsterToPlayerDistance)
            map.PositionGameObjectInEmptyCube(monster);

        // Record the monster in the lists
        if (monsterType == MONSTERTYPE.BASIC) monsterController.Index(GameDataManager.instance.RecordMonsterBasicPosition(monster.transform.position));
        if (monsterType == MONSTERTYPE.JUICY) monsterController.Index(GameDataManager.instance.RecordMonsterJuicyPosition(monster.transform.position));
    }

    // Spawn in a monster
    public void SpawnMonster(int monsterHealth)
    {
        // Monster gameobject
        GameObject monster;
        MONSTERTYPE type;

        // Random monster - basic monster
        if (Random.Range(0, 2) == 0)
        {
            // Instatiate the monsters and position in the map
            monster = Instantiate(basicMonster, Vector3.zero, Quaternion.identity, monsters.transform) as GameObject;
            type = MONSTERTYPE.BASIC;
            monster.GetComponentInChildren<SpriteRenderer>().color = monsterColourBasic;
        }

        // Random monster - juicy monster
        else
        {
            // Instatiate the monsters and position in the map
            monster = Instantiate(juicyMonster, Vector3.zero, Quaternion.identity, monsters.transform) as GameObject;
            type = MONSTERTYPE.JUICY;
            monster.GetComponentInChildren<SpriteRenderer>().color = monsterColourJuicy;
        }

        // Set initial position
        map.PositionGameObjectInEmptyCube(monster);

        // Set the monster type and health
        MonsterController monsterController = monster.GetComponent<MonsterController>();
        monsterController.typeOfMonster(type);
        monsterController.GetComponent<MonsterHealth>().SetCurrentAndMaxHealth(monsterHealth);

        // While the monster is closer to the player than minimum monster to player distance reposition the monster
        while (Vector3.Distance(monster.transform.position, player.transform.position) < minMonsterToPlayerDistance)
            map.PositionGameObjectInEmptyCube(monster);

        // Record the monster in the lists
        GameDataManager.instance.AddToMonsterCount(type);
        if (type == MONSTERTYPE.BASIC) monsterController.Index(GameDataManager.instance.RecordMonsterBasicPosition(monster.transform.position));
        if (type == MONSTERTYPE.JUICY) monsterController.Index(GameDataManager.instance.RecordMonsterJuicyPosition(monster.transform.position));

        Debug.Log("MONSTER - MONSTER SPAWN - COMPLETE");
    }

    // Setup the games monsters
    void RandomMonsterNumber()
    {
        // Generate random number of monsters
        numberOfBasicMonsters = Random.Range(minBasicMonsters, maxBasicMonsters);
        numberOfJuicyMonsters = Random.Range(minJuicyMonsters, maxJuicyMonsters);
        GameDataManager.instance.SetTotalMonsters(numberOfBasicMonsters, numberOfJuicyMonsters);
    }

    // Clamp the min/max cave generation settings
    void ClampMinMaxValues()
    {
        minBasicMonsters = Mathf.Clamp(minBasicMonsters, 1, maxBasicMonsters);
        maxBasicMonsters = Mathf.Clamp(maxBasicMonsters, minBasicMonsters, 20);
        minJuicyMonsters = Mathf.Clamp(minJuicyMonsters, 1, maxJuicyMonsters);
        maxJuicyMonsters = Mathf.Clamp(maxJuicyMonsters, minJuicyMonsters, 20);
    }
}
