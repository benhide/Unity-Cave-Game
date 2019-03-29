using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// IGameMode interface
public interface IQuest
{
    // Initialises quest
    void Init();

    // Generate quest map items
    void GenerateQuestItems();

    // Quest completed 
    bool QuestComplete();

    // Returns the quest name
    string QuestName();

    // Returns the quest description
    string QuestText();

    // Returns the quest completed text
    string QuestTextCompleted();
}

// Quest manager class
public class QuestManager : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Quest settings")]
    [SerializeField]
    int numberOfQuests;
    [SerializeField]
    int questIndex = -1;

    [Header("Game mode selected (randomly generated)")]
    private IQuest[] iQuests = new IQuest[]
    {
        new PickupQuest(),
        new KillMonsters(),
        new ChestQuest(),
        new ExitQuest()
    };
    public IQuest[] quests;

    [Header("Pickup settings")]
    [Range(2, 20)]
    public int minPickupPlayerDist;
    [Range(1, 20)]
    public int minPickups;
    [Range(1, 20)]
    public int maxPickups;
    [SerializeField]
    int totalPickups;
    [SerializeField]
    int pickupsCollected;

    [Header("Chest settings")]
    [Range(10, 20)]
    public int maxKeyToChestDist;
    [Range(2, 10)]
    public int minKeyToPlayerDist;
    [Range(1, 6)]
    public int minChests;
    [Range(1, 6)]
    public int maxChests;
    [SerializeField]
    int totalChestsKeys;
    [SerializeField]
    int chestsOpened;
    [SerializeField]
    int keysCollected;

    [Header("Civillian settings")]
    [Range(2, 20)]
    public int minPlayerToCivillianDist;

    [Header("Monsters settings")]
    [Range(1, 20)]
    public int minMonsters;
    [Range(1, 20)]
    public int maxMonsters;
    [SerializeField]
    int totalMonsters;
    [SerializeField]
    int monstersKilled;

    [Header("Quest game objects")]
    public GameObject pickup;
    public GameObject key;
    public GameObject chest;
    public GameObject oldMiner;
    public GameObject civillian;
    public GameObject exitLight;
    public GameObject player;

    [Header("Quest sprites and images")]
    public Sprite pickupSprite;
    public Sprite chestSprite;
    public Sprite keySprite;
    public Sprite monsterSprite;
    public Sprite exitSprite;
    public Image exitImage;
    public Sprite[] artefacts = new Sprite[6];

    [Header("Quest text")]
    public Image questBackground;
    public Text questText;
    public GameObject questDisplay;

    [Header("Quest SFXs and audio sources")]
    public AudioSource soundFx;
    public AudioClip collectableSFX;

    [Header("Grid cube particle systems")]
    public ParticleSystem collectableParticles;

    [Header("Script references")]
    public Map map;
    public HUD hud;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the references
        hud = GameObject.FindGameObjectWithTag(Tags.uiTag).GetComponent<HUD>();
        map = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<Map>();

        // Find the player
        player = GameObject.FindGameObjectWithTag(Tags.playerTag);

        // Set the colours
        SetColours();
    }

    // Initialise quests
    public void InitQuests()
    {
        // Set the number of quests and initialise the quests array
        numberOfQuests = iQuests.Length;
        quests = new IQuest[numberOfQuests];

        // List of quests
        List<IQuest> selectedQuests = new List<IQuest>();

        // Select quests
        while (selectedQuests.Count < numberOfQuests - 1)
        {
            // Random quest
            IQuest iQuest = iQuests[Random.Range(0, quests.Length - 1)];

            // Add selected quest
            if (!selectedQuests.Contains(iQuest))
                selectedQuests.Add(iQuest);
        }

        // Copy to array
        selectedQuests.CopyTo(quests);

        // Initialise the initial quest and quest itmes
        InitQuestController();

        // Set the last gamemode as exit
        quests[numberOfQuests - 1] = iQuests[iQuests.Length - 1];

        // Set the number of game quests
        GameDataManager.instance.NumberOfQuests(numberOfQuests);

        // Assign the game quests names
        for (int i = 0; i < quests.Length; i++)
            GameDataManager.instance.SetQuestNames(i, quests[i].QuestName());

        Debug.Log("QUEST - INIT - COMPLETE");

        // Initialise the civillian
        InitCivillian();
    }

    // Initialise the quest controller
    void InitQuestController()
    {
        // Instatiate the old miner and position in the map
        GameObject om = Instantiate(oldMiner, oldMiner.transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag(Tags.objectiveTag).transform) as GameObject;

        // Set initial position
        if (map.PositionOldMiner(om, player.transform.position))
            om.GetComponent<OldMiner>().SetPlayerRotation();

        // Record old miners position
        GameDataManager.instance.RecordOldMinerPosition(om.transform.position);

        Debug.Log("QUEST - OLD MINER INIT - COMPLETE");
    }

    // Initialse the civillian
    void InitCivillian()
    {
        // Instatiate the civillian and position in the map
        GameObject cv = Instantiate(civillian, civillian.transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag(Tags.objectiveTag).transform) as GameObject;

        // Position the civillian
        map.PositionGameObjectInEmptyCube(cv);

        // Civillian spawned
        bool civillianSpawned = false;

        // While the civillian is not spawned
        while (!civillianSpawned)
        {
            // Distance from player to civillian
            float dist = Vector3.Distance(cv.transform.position, player.transform.position);

            // If civillian is too close
            if (dist < minPlayerToCivillianDist)
            {
                // Position the civillian
                map.PositionGameObjectInEmptyCube(cv);
            }

            // Civillian spawned 
            else civillianSpawned = true;
        }

        // Record civillian position
        GameDataManager.instance.RecordCivillianPosition(cv.transform.position);

        Debug.Log("QUEST - CIVILLIAN INIT - COMPLETE");
    }

    // Return if the quest has been completed
    public bool CurrentQuestCompleted()
    {
        // Check if current quest is complete
        if (quests != null && quests[questIndex].QuestComplete())
            return true;
        else
            return false;
    }

    // Get the current quest text
    public string CurrentQuestText()
    {
        return quests[questIndex].QuestText();
    }

    // Check if the chests quest is completed
    public void ChestQuestCompleted()
    {
        // If the quests exsists and the name is the chest quest name and the quest is complete - remove the artefacts
        if (quests != null && quests[questIndex].QuestName() == StaticStrings.chestQuestName && quests[questIndex].QuestComplete())
            for (int i = 0; i < artefacts.Length; i++)
                hud.RemoveArtefact(i);
    }

    // Get the previous quest text
    public string PerviousQuestCompleteText()
    {
        return quests[questIndex - 1].QuestTextCompleted();
    }

    // Display the quest text
    public void InitQuest(int pauseTime, string textOne = "", string textTwo = "")
    {
        // Container for text
        string text = "";
        string secondText = "";

        // Start next quest
        if (questIndex < quests.Length)
        {
            questIndex++;

            Debug.Log("QUEST - QUEST INIT - STARTED");

            quests[questIndex].Init();

            Debug.Log("QUEST - QUEST INIT - COMPLETE");

            quests[questIndex].GenerateQuestItems();

            Debug.Log("QUEST - QUEST GEN - COMPLETE");
        }

        // Set the display as active
        questDisplay.SetActive(true);

        // Start quest
        if (questIndex == 0)
        {
            GameDataManager.instance.SetQuestStats(quests[questIndex].QuestName());
            text = textOne;
            secondText = textTwo + " " + CurrentQuestText();
            StartCoroutine(QuestDisplayPause(pauseTime, true, text, secondText));
        }

        // Else if monsters quest
        else if (quests[questIndex].QuestName() == StaticStrings.monsterQuestName)
        {
            GameDataManager.instance.SetQuestStatsCompleted(questIndex - 1);
            GameDataManager.instance.SetQuestStats(quests[questIndex].QuestName());
            text = PerviousQuestCompleteText() + " " + CurrentQuestText();
            StartCoroutine(QuestDisplayPause(pauseTime, false, text));
        }

        // Other quests
        else
        {
            GameDataManager.instance.SetQuestStatsCompleted(questIndex - 1);
            GameDataManager.instance.SetQuestStats(quests[questIndex].QuestName());
            text = PerviousQuestCompleteText() + " " + CurrentQuestText();
            StartCoroutine(QuestDisplayPause(pauseTime, false, text));
        }
    }

    // Display the quest text
    public void QuestText(string text, int pauseTime)
    {
        questDisplay.SetActive(true);
        StartCoroutine(QuestDisplayPause(pauseTime, false, text));
    }

    // Display the quest text
    public void CivillianQuestText(string text)
    {
        questDisplay.SetActive(true);
        StartCoroutine(CivillianQuestDisplayPause(text));
    }

    // Set the colours
    void SetColours()
    {
        // Set the text and background colours
        if (GameDataManager.instance.SelectedColourScheme() == COLOURSCHEME.NORMAL)
        {
            questText.color = Colours.UITextColour;
            questBackground.color = Colours.UIBackgroundColour;
            pickup.GetComponent<SpriteRenderer>().color = Colours.pickUpColour;
        }

        // Set the text and background colours
        if (GameDataManager.instance.SelectedColourScheme() == COLOURSCHEME.CVD)
        {
            // Red safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.RED)
            {
                questText.color = Colours.UITextColourRedSafe;
                questBackground.color = Colours.UIBackgroundColourRedSafe;
                pickup.GetComponent<SpriteRenderer>().color = Colours.pickUpColourRedSafe;
            }

            // Green safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.GREEN)
            {
                questText.color = Colours.UITextColourGreenSafe;
                questBackground.color = Colours.UIBackgroundColourGreenSafe;
                pickup.GetComponent<SpriteRenderer>().color = Colours.pickUpColourGreenSafe;
            }

            // Blue safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.BLUE)
            {
                questText.color = Colours.UITextColourBlueSafe;
                questBackground.color = Colours.UIBackgroundColourBlueSafe;
                pickup.GetComponent<SpriteRenderer>().color = Colours.pickUpColourBlueSafe;
            }
        }
    }

    // Is it the monsters quest
    public bool MonsterQuest()
    {
        return quests[questIndex].QuestName() == StaticStrings.monsterQuestName;
    }

    // Get the quest index
    public int QuestIndex()
    {
        return questIndex;
    }

    // Display the quest details
    IEnumerator QuestDisplayPause(float pauseTime, bool secondText, string textOne = "", string textTwo = "")
    {
        // Set end time and text
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        questText.text = textOne;

        // Set the time scale
        Time.timeScale = 0.0f;

        // Pause while the timer counts down or player breaks loop
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            if (Controls.instance.diggingAttacking) break;
            yield return 0.0f;
        }

        // If there are two texts to display
        if (secondText)
        {
            float secondPauseEndTime = Time.realtimeSinceStartup + pauseTime;
            questText.text = textTwo;
            Controls.instance.diggingAttacking = false;

            // Pause while the timer counts down or player breaks loop
            while (Time.realtimeSinceStartup < secondPauseEndTime)
            {
                if (Controls.instance.diggingAttacking) break;
                yield return 0.0f;
            }
        }

        // Set time scale, hide display
        Time.timeScale = 1.0f;
        questDisplay.SetActive(false);
    }

    // Display the quest details
    IEnumerator CivillianQuestDisplayPause(string textOne = "")
    {
        // Set text
        questText.text = textOne;

        // Set the time scale
        Time.timeScale = 0.0f;

        // Pause unitil player breaks loop
        while (true)
        {
            if (Controls.instance.diggingAttacking) break;
            if (Controls.instance.otherAction) break;
            yield return 0.0f;
        }

        // Set time scale, hide display
        Time.timeScale = 1.0f;
        questDisplay.SetActive(false);
    }

    ////////////////////////////Pickups/////////////////////////////

    public void SetTotalPickups()
    {
        totalPickups = Random.Range(minPickups, maxPickups);
        GameDataManager.instance.SetTotalPickups(totalPickups);
    }

    public int TotalPickups()
    {
        return totalPickups;
    }

    public void PickupCollected(int index)
    {
        pickupsCollected++;
        GameDataManager.instance.PickupCollected(index);
    }

    public int PickupsCollected()
    {
        return pickupsCollected;
    }

    public bool AllPickupsCollected()
    {
        return pickupsCollected >= totalPickups;
    }

    public float MinPickupPlayerDist()
    {
        return minPickupPlayerDist;
    }

    /////////////////////////Chests and Keys////////////////////////

    public void SetTotalChestsAndKeys()
    {
        totalChestsKeys = Random.Range(minChests, maxChests);
        GameDataManager.instance.SetTotalChestsAndKeys(totalChestsKeys);
    }

    public int TotalChests()
    {
        return totalChestsKeys;
    }

    public int ChestsOpened()
    {
        return chestsOpened;
    }

    public void ChestOpened(int index)
    {
        chestsOpened++;
        GameDataManager.instance.ChestOpened(index);
    }

    public bool AllChestsOpened()
    {
        return chestsOpened >= totalChestsKeys;
    }

    public void KeyCollected(int index)
    {
        keysCollected++;
        GameDataManager.instance.KeyCollected(index);
    }

    /////////////////////////////Monsters///////////////////////////

    public void SetTotalMonsters()
    {
        totalMonsters = Random.Range(minMonsters, maxMonsters);
    }

    public int TotalMonsters()
    {
        return totalMonsters;
    }

    public void MonsterKilled()
    {
        monstersKilled++;
    }

    public int MonstersKilled()
    {
        return monstersKilled;
    }

    public bool AllMonstersKilled()
    {
        return monstersKilled >= totalMonsters;
    }

    ////////////////////SFX and Particle Systems////////////////////

    public void PlaySFX()
    {
        soundFx.PlayOneShot(collectableSFX);
    }

    public void PlaySFX(AudioClip clip)
    {
        soundFx.PlayOneShot(clip);
    }
}