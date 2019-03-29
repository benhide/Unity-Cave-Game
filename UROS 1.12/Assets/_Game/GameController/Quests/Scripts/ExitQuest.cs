#pragma warning disable 0414
using UnityEngine;
using UnityEngine.SceneManagement;

// Exit quest 
public class ExitQuest : IQuest
{
    ///////////////////////////Variables////////////////////////////

    // Exit quest items
    public GameObject exitLight;
    bool questComplete = false;
    bool questInitialised = false;
    string questName = StaticStrings.exitQuestName;
    string questText = StaticStrings.exitQuestText;
    string questTextCompleted = StaticStrings.exitQuestTextCompleted;

    // References
    public QuestManager questManager;
    public Map map;
    public HUD hud;
    public MiniMapIcon miniMapIcon;
    public LevelChanger levelChanger;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Initialises quest
    public void Init()
    {
        // References
        questManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<QuestManager>();
        map = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<Map>();
        hud = GameObject.FindGameObjectWithTag(Tags.uiTag).GetComponent<HUD>();

        // Event/callbacks
        CubeData.onPlayerExit += ExitedGame;
        Debug.Log("Exit quest initialised!");

        // Assign the level changer
        levelChanger = GameObject.FindGameObjectWithTag(Tags.levelChangerTag).GetComponent<LevelChanger>();
        exitLight = questManager.exitLight;

        // Set the quest text 
        hud.SetQuestProgressText("Escape!");
        hud.SetTextAlignment();
        hud.SetQuestProgressItem(questManager.exitSprite);
    }

    // Player exited game
    public void ExitedGame(GameObject player)
    {
        // Record the end of the games map grid cubes
        GameDataManager.instance.RecordGridCubes(map.GridCubeArray(), false);

        // Push the data
        GameDataManager.instance.PushData();
        Debug.Log("Exit quest completed GAME WON");

        // Load the scene
        Object.DontDestroyOnLoad(GameDataManager.instance);
        Object.DontDestroyOnLoad(Controls.instance);

        // Load the next level
        levelChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Generates quest items
    public void GenerateQuestItems()
    {
        // Assign the exit cube position
        int xPos = 0;
        int zPos = 0;
        int xPosTwo = 0;
        int zPosTwo = 0;

        // Choose the exit side
        int exitSide = Random.Range(0, 4);

        // Switch on exit side and assign random exit cube
        switch (exitSide)
        {
            // Left side
            case 0:
                xPos = 1;
                xPosTwo = 0;
                zPos = Random.Range(2, map.GridHeight() - 3);
                zPosTwo = zPos;
                break;

            // Top side
            case 1:
                xPos = Random.Range(2, map.GridWidth() - 3);
                xPosTwo = xPos;
                zPos = map.GridHeight() - 2;
                zPosTwo = map.GridHeight() - 1;
                break;

            // Right side
            case 2:
                xPos = map.GridWidth() - 2;
                xPosTwo = map.GridWidth() - 1;
                zPos = Random.Range(2, map.GridHeight() - 3);
                zPosTwo = zPos;
                break;

            // Bottom side
            case 3:
                xPos = Random.Range(2, map.GridWidth() - 3);
                xPosTwo = xPos;
                zPos = 1;
                zPosTwo = 0;
                break;
        }

        // Set the exit cube as a tigger and disable the mesh renderer
        GameObject exit = map.GridCubeArray()[xPos, zPos];
        exit.GetComponent<BoxCollider>().isTrigger = true;
        exit.GetComponent<MeshRenderer>().enabled = false;
        exit.GetComponent<CubeData>().SetCubeType(CUBETYPE.EXIT);
        GameObject light = Object.Instantiate(exitLight, exit.transform.position, Quaternion.identity, exit.transform);
        light.SetActive(true);

        exit = map.GridCubeArray()[xPosTwo, zPosTwo];
        exit.GetComponent<BoxCollider>().isTrigger = true;
        exit.GetComponent<MeshRenderer>().enabled = false;
        exit.GetComponent<CubeData>().SetCubeType(CUBETYPE.EXIT);
        light = Object.Instantiate(exitLight, exit.transform.position, Quaternion.identity, exit.transform);
        light.SetActive(true);

        // Add exit to the minimap
        miniMapIcon = exit.AddComponent<MiniMapIcon>();
        miniMapIcon.miniMapImage = questManager.exitImage;
    }

    //////////////////////////Set and Gets//////////////////////////

    // Quest completed
    public bool QuestComplete()
    {
        return questComplete;
    }

    public string QuestName()
    {
        return questName;
    }

    public string QuestText()
    {
        return questText;
    }

    public string QuestTextCompleted()
    {
        return questTextCompleted;
    }

    ///////////////////////End of Functions/////////////////////////
}
