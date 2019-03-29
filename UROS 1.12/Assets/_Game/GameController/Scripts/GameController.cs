using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Game controller class
public class GameController : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Player Settings")]
    [SerializeField]
    GameObject player;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    bool playerSpawned = false;

    [Header("Player Health Settings")]
    public GameObject health;
    [SerializeField]
    bool healthSpawned = false;

    [Header("GameObjects and UI")]
    public Text questText;
    public Text gameModeProgressText;
    public Text playerHealthText;
    public Text playerScoreText;
    public Text playerPointsText;

    [Header("Script references")]
    public LevelChanger levelChanger;
    public Map map;
    public QuestManager questManager;
    public MonsterManager monsterManager;
    public Controls controls;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Awake()
    {
        // Assign the reference to the game data manager
        controls = Controls.instance;
        levelChanger = GameObject.FindGameObjectWithTag(Tags.levelChangerTag).GetComponent<LevelChanger>();

        // Assign the components
        map = GetComponent<Map>();
        questManager = GetComponent<QuestManager>();
        monsterManager = GetComponent<MonsterManager>();

        // Initialise the map
        map.InitMap();

        // Assign the player
        player = GameObject.FindGameObjectWithTag(Tags.playerTag);
        playerController = player.GetComponent<PlayerController>();

        // Set the font sizes
        int size = 0;
        switch (GameDataManager.instance.SelectedFontSize())
        {
            case FONTSIZE.SMALL: size = FontSizes.smallFont; break;
            case FONTSIZE.MEDIUM: size = FontSizes.mediumFont; break;
            case FONTSIZE.LARGE: size = FontSizes.largeFont; break;
            default: size = FontSizes.mediumFont; break;
        }

        // Set the font sizes
        playerPointsText.fontSize
        = playerScoreText.fontSize
        = playerHealthText.fontSize
        = questText.fontSize
        = gameModeProgressText.fontSize
        = size;

        Debug.Log("GC - START - COMPLETE");
    }

    // Update is called once per frame
    void /*Late*/Update()
    {
        // If the map building is complete
        if (map.MapGenerationCompleted() && !playerSpawned)
        {
            // Position the player
            playerSpawned = map.PositionPlayerInEmptyCube(player);
            playerController.SetPlayerRotation();

            // Initialse the quests
            questManager.InitQuests();

            // Record the start of the games map grid cubes
            GameDataManager.instance.RecordGridCubes(map.GridCubeArray(), true);

            // Spawn the initial monsters
            monsterManager.SpawnMonsters();

            Debug.Log("GC - INIT - COMPLETE");
        }

        // If the player needs health spawn it
        if (playerController.CurrentHealth() <= playerController.MaxHealth() / 2 && !healthSpawned)
        {
            // Spawn player health
            SpawnHealth();
            healthSpawned = true;
        }

        // If the player is dead -- needs adding to
        if (playerController.CurrentHealth() <= 0 && !playerController.PlayerDead())
        {
            // Disable the player controller
            playerController.enabled = false;

            // Record the end of the games map grid cubes
            GameDataManager.instance.RecordGridCubes(map.GridCubeArray(), false);
            GameDataManager.instance.SetQuestStatsIncomplete(questManager.QuestIndex());

            // Push the data
            GameDataManager.instance.PushData();
            Debug.Log("PLAYER DIED - GAME OVER");

            // Load the scene
            DontDestroyOnLoad(GameDataManager.instance);
            DontDestroyOnLoad(Controls.instance);

            // Load the next level
            levelChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);

            // Player is dead
            playerController.PlayerDead(true);
        }

        // Increase the game time
        GameDataManager.instance.IncreaseGameTime();
    }

    // Health has been collected
    public void HealthCollected()
    {
        healthSpawned = false;
    }

    // Spawn health for player
    public void SpawnHealth()
    {
        // Instatiate the monsters and position in the map
        GameObject healthGO = Instantiate(health, Vector3.zero, Quaternion.identity, GameObject.FindGameObjectWithTag(Tags.objectiveTag).transform) as GameObject;
        map.PositionGameObjectInEmptyCube(healthGO);
        healthGO.GetComponent<HealthPickup>().Index(GameDataManager.instance.RecordHealthPosition(healthGO.transform.position));
    }

    // Player Spawned
    public bool PlayerSpawned()
    {
        return playerSpawned;
    }

    ///////////////////////End of Functions/////////////////////////
}