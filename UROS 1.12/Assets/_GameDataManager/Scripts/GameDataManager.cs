#pragma warning disable 0414
using System.Collections.Generic;
using UnityEngine;

// Global game data manager enums
public enum GENDER { MALE, FEMALE }
public enum COLOURSCHEME { CVD, NORMAL }
public enum CVDCOLOURSCHEME { RED, GREEN, BLUE, NONE }
public enum FONTSIZE { SMALL, MEDIUM, LARGE }
public enum CIVILLIANTYPE { MALE, FEMALE, CAT }

// Game stat classes
[System.Serializable]
public class ChestStat
{
    public float spawnTime;
    public float openedTime;
    public bool opened;
    public Vector3 position;
}

[System.Serializable]
public class KeyStat
{
    public float spawnTime;
    public float collectedTime;
    public bool collected;
    public Vector3 position;
}

[System.Serializable]
public class PickupStat
{
    public float spawnTime;
    public float collectedTime;
    public bool collected;
    public Vector3 position;
}

[System.Serializable]
public class QuestStat
{
    public float startTime;
    public float endTime;
    public string name;
    public bool complete;
}

[System.Serializable]
public class MonsterStat
{
    public float spawnTime;
    public float killedTime;
    public Vector3 killedPos;
    public Vector3 spawnedPos;
    public MONSTERTYPE type;
    public bool killed;
    public bool killedInQuest;
}

[System.Serializable]
public class CubeStat
{
    public Vector3 position;
    public CUBETYPE type;
}

[System.Serializable]
public class CubeDestroyedStat
{
    public Vector3 position;
    public CUBETYPE type;
    public bool destroyed;
    public float destroyedTime;
}

[System.Serializable]
public class CivillianStat
{
    public Vector3 position;
    public CIVILLIANTYPE type;
    public bool saved;
    public bool killed;
    public float savedKilledTime;
}

[System.Serializable]
public class TNTStat
{
    public Vector3 position;
    public float usedTime;
}

[System.Serializable]
public class HealthStat
{
    public float collectedTime;
    public float spawnTime;
    public bool collected;
    public Vector3 position;
}


// Manager class to hold and manage all the game settings
[System.Serializable]
public class GameDataManager : MonoBehaviour
{
    [Header("Player settings / statistics")]
    [SerializeField]
    string playerName = "";
    [SerializeField]
    float gameTime = 0.0f;
    float playerPositionTime = 1.0f;
    float playerPositionTimer = 0.0f;
    [SerializeField]
    int playerHealth;
    [SerializeField]
    int playerScore;
    [SerializeField]
    GENDER genderInitial;
    [SerializeField]
    GENDER genderSelected;
    [SerializeField]
    List<Vector3> playerPositionsList;
    [SerializeField]
    List<TNTStat> playerTNTStatsList;
    [SerializeField]
    List<HealthStat> playerHealthPickupStatsList;

    [Header("Accessability settings")]
    [SerializeField]
    FONTSIZE selectedFontSize;
    [SerializeField]
    COLOURSCHEME selectedColourScheme;
    [SerializeField]
    CVDCOLOURSCHEME selectedCVDColourScheme;

    [Header("Grid cube counts")]
    [SerializeField]
    int numberOfTotalCubes;
    [SerializeField]
    int numberOfRockCubes;
    [SerializeField]
    int numberOfCrystalCubes;
    [SerializeField]
    int numberOfCubesDestroyed;
    [SerializeField]
    int numberOfCubesDestroyedRock;
    [SerializeField]
    int numberOfCubesDestroyedCrystal;

    [Header("Pickup statistics")]
    [SerializeField]
    int pickupsTotal;
    [SerializeField]
    int pickupsCollected;

    [Header("Chests statistics")]
    [SerializeField]
    int chestsTotal;
    [SerializeField]
    int chestsOpened;
    [SerializeField]
    int keysTotal;
    [SerializeField]
    int keysCollected;

    [Header("Monster statistics")]
    [SerializeField]
    int monstersTotal;
    [SerializeField]
    int monstersBasic;
    [SerializeField]
    int monstersJuicy;
    [SerializeField]
    int monstersKilledTotal;
    [SerializeField]
    int monstersKilledBasic;
    [SerializeField]
    int monstersKilledJuicy;

    [Header("Game statistics")]
    [SerializeField]
    string[] questNames;
    [SerializeField]
    int questsNumber;
    [SerializeField]
    List<QuestStat> questStatsList;
    [SerializeField]
    Vector3 questOldMinerPosition;

    [Header("Civillian statistics")]
    [SerializeField]
    CIVILLIANTYPE civillianType;
    [SerializeField]
    CivillianStat civillianStats;
    [SerializeField]
    Vector3 civillianPosition;

    [Header("Cube positional statistics")]
    [SerializeField]
    List<CubeDestroyedStat> cubePositionsDestroyed;
    [SerializeField]
    List<CubeStat> cubePositionsStart;
    [SerializeField]
    List<CubeStat> cubePositionsEnd;

    [Header("Monster positional statistics")]
    [SerializeField]
    List<MonsterStat> monsterKilledStatsBasicList;
    [SerializeField]
    List<MonsterStat> monsterKilledStatsJuicyList;

    [Header("Objective positional statistics")]
    [SerializeField]
    List<PickupStat> listPickupStats;
    [SerializeField]
    List<ChestStat> listChestStats;
    [SerializeField]
    List<KeyStat> listKeyStats;


    // Static GameDataManager instance
    public static GameDataManager instance;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Awake()
    {
        // Set static instance to this
        instance = this;

        // Initialise the cube positions lists
        cubePositionsStart = new List<CubeStat>();
        cubePositionsEnd = new List<CubeStat>();
        cubePositionsDestroyed = new List<CubeDestroyedStat>();

        // Initialise the collectable lists
        listPickupStats = new List<PickupStat>();
        listKeyStats = new List<KeyStat>();
        listChestStats = new List<ChestStat>();
        playerHealthPickupStatsList = new List<HealthStat>();

        // Initialise the TNT list
        playerTNTStatsList = new List<TNTStat>();

        // Quest stats list
        questStatsList = new List<QuestStat>();

        // Monster lists
        monsterKilledStatsBasicList = new List<MonsterStat>();
        monsterKilledStatsJuicyList = new List<MonsterStat>();

        // Select the civillian type
        int type = Random.Range(0, 3);
        if (type == 0) civillianType = CIVILLIANTYPE.MALE;
        if (type == 1) civillianType = CIVILLIANTYPE.FEMALE;
        if (type == 2) civillianType = CIVILLIANTYPE.CAT;
    }

    // Increase the game running time at each timestep - CALLED IN UPDATE ONLY ONCE!
    public void IncreaseGameTime()
    {
        gameTime += Time.deltaTime;
    }

    // Clear all the game statistics
    void ClearGameStats()
    {
        // Reset the player details
        playerName = "";
        playerHealth = 0;
        playerScore = 0;

        // Reset all the cube values
        numberOfRockCubes = 0;
        numberOfCrystalCubes = 0;
        numberOfTotalCubes = 0;
        numberOfCubesDestroyed = 0;
        numberOfCubesDestroyedRock = 0;
        numberOfCubesDestroyedCrystal = 0;

        // Reset all the monster killed values
        pickupsCollected = 0;

        // Reset all the monster killed values
        monstersKilledBasic = 0;
        monstersKilledJuicy = 0;
        monstersKilledTotal = 0;

        // Clear all the gameobject stats
        playerPositionsList.Clear();

        // Cubes
        cubePositionsDestroyed.Clear();
        cubePositionsStart.Clear();
        cubePositionsEnd.Clear();

        //// Monsters
        //monsterKillPositions.Clear();
        //monsterKillPositionsBasic.Clear();
        //monsterKillPositionsJuicy.Clear();

        // Objectives
        listPickupStats.Clear();
        listChestStats.Clear();
        listKeyStats.Clear();
        questStatsList.Clear();

        // Reset the game timer
        gameTime = 0.0f;
    }

    // Push all the game data
    public void PushData()
    {
        cubePositionsStart.Sort(Compare);
        cubePositionsEnd.Sort(Compare);
        cubePositionsDestroyed.Sort(Compare);
        GetComponent<FirebasePushData>().PushData(this);
        //ClearGameStats();
    }

    // Set the player health 
    public void SetPlayerHealth(int health)
    {
        playerHealth = health;
    }

    // Set the player score
    public void SetPlayerScore(int score)
    {
        playerScore = score;
    }

    ///////////////////////////Grid Cubes///////////////////////////

    // Record the grid cubes (start and end of game)
    public void RecordGridCubes(GameObject[,] gridCubes, bool start)
    {
        // Loop through all the cubes
        foreach (GameObject go in gridCubes)
        {
            // Add cube to the cube positions dictionary
            CubeData cubeData = go.GetComponent<CubeData>();
            Vector3 cubePos = cubeData.GetCubeCoord();

            // Start of the game
            if (start)
                cubePositionsStart.Add(new CubeStat { position = cubePos, type = cubeData.GetCubeType() });

            // End of the game
            else
                cubePositionsEnd.Add(new CubeStat { position = cubePos, type = cubeData.GetCubeType() });
        }
    }

    // Count all the grid cubes and send counts to game data manager
    public void CountGridCubeTypes()
    {
        // Find all the gridcubes
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(Tags.gridCubeTag);

        // Loop through all the grid cubes
        for (int i = 0; i < cubes.Length; i++)
        {
            // Count all the rock cubes
            if (cubes[i].GetComponent<CubeData>().GetCubeType() == CUBETYPE.ROCK)
                numberOfRockCubes++;

            // Count all the crystal cubes
            if (cubes[i].GetComponent<CubeData>().GetCubeType() == CUBETYPE.CRYSTAL)
                numberOfCrystalCubes++;
        }

        // Calculate all the total number of cubes (which can be destroyed)
        numberOfTotalCubes = numberOfRockCubes + numberOfCrystalCubes;
    }

    // Count the destroyed grid cube and send count to game data manager
    public void CountDestroyedGridCubeType(CUBETYPE cubeType, Vector3 cubePos)
    {
        // If the cube destroyed is a rock add to the rock cubes destroyed count
        if (cubeType == CUBETYPE.ROCK)
            numberOfCubesDestroyedRock++;

        // If the cube destroyed is a crystal add to the crystal cubes destroyed count
        if (cubeType == CUBETYPE.CRYSTAL)
            numberOfCubesDestroyedCrystal++;

        // Count the destroyed grid cube and record the position
        numberOfCubesDestroyed++;
        cubePositionsDestroyed.Add(new CubeDestroyedStat { position = cubePos, type = cubeType, destroyed = true });
    }

    // Remove the opened grid cube from the cube counts
    public void RemoveOpenedCubeFromCounts(CUBETYPE cubeType)
    {
        // If removing a rock cube
        if (cubeType == CUBETYPE.ROCK)
        {
            numberOfRockCubes--;
            numberOfTotalCubes--;
        }

        // If removing a rock cube
        if (cubeType == CUBETYPE.CRYSTAL)
        {
            numberOfCrystalCubes--;
            numberOfTotalCubes--;
        }
    }

    ///////////////////////////Positions////////////////////////////

    public void RecordOldMinerPosition(Vector3 oldminerPos)
    {
        questOldMinerPosition = oldminerPos;
    }

    public void RecordCivillianPosition(Vector3 civillianPos)
    {
        civillianPosition = civillianPos;
    }

    public void RecordPlayerPosition(Vector3 playerPos)
    {
        // Decrease the timer, take the player position
        playerPositionTimer -= Time.deltaTime;
        if (playerPositionTimer <= 0.0f)
        {
            playerPositionsList.Add(playerPos);
            playerPositionTimer = playerPositionTime;
        }
    }

    public int RecordPickupPosition(Vector3 pickupPos)
    {
        listPickupStats.Add(new PickupStat { spawnTime = gameTime, collectedTime = 0.0f, collected = false, position = pickupPos });
        return listPickupStats.Count - 1;
    }

    public int RecordChestPosition(Vector3 chestPos)
    {
        listChestStats.Add(new ChestStat { spawnTime = gameTime, openedTime = 0.0f, opened = false, position = chestPos });
        return listChestStats.Count - 1;
    }

    public int RecordKeyPosition(Vector3 keyPos)
    {
        listKeyStats.Add(new KeyStat { spawnTime = gameTime, collectedTime = 0.0f, collected = false, position = keyPos });
        return listKeyStats.Count - 1;
    }

    public void RecordTNTPosition(Vector3 TNTPos)
    {
        playerTNTStatsList.Add(new TNTStat() { position = TNTPos, usedTime = gameTime });
    }

    public int RecordHealthPosition(Vector3 healthPos)
    {
        playerHealthPickupStatsList.Add(new HealthStat() { position = healthPos, collected = false, collectedTime = 0.0f, spawnTime = gameTime });
        return playerHealthPickupStatsList.Count - 1;
    }

    public int RecordMonsterBasicPosition(Vector3 monsterPos)
    {
        monsterKilledStatsBasicList.Add(new MonsterStat() { killed = false, killedInQuest = false, killedPos = Vector3.zero, spawnedPos = monsterPos, killedTime = 0.0f, spawnTime = gameTime, type = MONSTERTYPE.BASIC });
        return monsterKilledStatsBasicList.Count - 1;
    }

    public int RecordMonsterJuicyPosition(Vector3 monsterPos)
    {
        monsterKilledStatsJuicyList.Add(new MonsterStat() { killed = false, killedInQuest = false, killedPos = Vector3.zero, spawnedPos = monsterPos, killedTime = 0.0f, spawnTime = gameTime, type = MONSTERTYPE.JUICY });
        return monsterKilledStatsJuicyList.Count - 1;
    }

    ////////////////////////////Settings////////////////////////////

    public void SetColourScheme(COLOURSCHEME colourScheme)
    {
        selectedColourScheme = colourScheme;
    }

    public COLOURSCHEME SelectedColourScheme()
    {
        return selectedColourScheme;
    }

    public void SetCVDColourScheme(CVDCOLOURSCHEME colourScheme)
    {
        selectedCVDColourScheme = colourScheme;
    }

    public CVDCOLOURSCHEME SelectedCVDColourScheme()
    {
        return selectedCVDColourScheme;
    }

    public void SetFontSize(FONTSIZE fontSize)
    {
        selectedFontSize = fontSize;
    }

    public FONTSIZE SelectedFontSize()
    {
        return selectedFontSize;
    }

    public void SetGender(GENDER gender)
    {
        genderSelected = gender;
    }

    public GENDER SelectedGender()
    {
        return genderSelected;
    }

    public void SetInitialGender(GENDER gender)
    {
        genderInitial = gender;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public string PlayerName()
    {
        return playerName;
    }

    public int PlayerScore()
    {
        return playerScore;
    }

    public int PlayerHealth()
    {
        return playerHealth;
    }

    public CIVILLIANTYPE CivillianType()
    {
        return civillianType;
    }

    /////////////////////////////Quests/////////////////////////////

    public void NumberOfQuests(int numb)
    {
        questsNumber = numb;
        questNames = new string[numb];
    }

    public void SetQuestNames(int index, string name)
    {
        questNames[index] = name;
    }

    public void SetQuestStats(string questName)
    {
        questStatsList.Add(new QuestStat { startTime = gameTime, complete = false, endTime = gameTime, name = questName });
    }

    public void SetQuestStatsCompleted(int index)
    {
        questStatsList[index].complete = true;
        questStatsList[index].endTime = gameTime;
    }

    public void SetQuestStatsIncomplete(int index)
    {
        questStatsList[index].complete = false;
        questStatsList[index].endTime = gameTime;
    }

    public void SetCivillianStats(Vector3 civillianPos, bool civillianKilled, bool civillianSaved)
    {
        civillianStats = new CivillianStat() { type = civillianType, position = civillianPos, killed = civillianKilled, saved = civillianSaved, savedKilledTime = gameTime };
    }


    ////////////////////////////Pickups/////////////////////////////

    public void SetTotalPickups(int numbOfPickups)
    {
        pickupsTotal = numbOfPickups;
    }

    public void PickupCollected(int index)
    {
        pickupsCollected++;
        listPickupStats[index].collected = true;
        listPickupStats[index].collectedTime = gameTime;
    }

    public void HealthCollected(int index)
    {
        playerHealthPickupStatsList[index].collected = true;
        playerHealthPickupStatsList[index].collectedTime = gameTime;
    }

    /////////////////////////Chests and Keys////////////////////////

    public void SetTotalChestsAndKeys(int numbOfChests)
    {
        chestsTotal = numbOfChests;
        keysTotal = numbOfChests;
    }

    public void ChestOpened(int index)
    {
        chestsOpened++;
        listChestStats[index].opened = true;
        listChestStats[index].openedTime = gameTime;
    }

    public void KeyCollected(int index)
    {
        keysCollected++;
        listKeyStats[index].collected = true;
        listKeyStats[index].collectedTime = gameTime;
    }

    /////////////////////////////Monsters///////////////////////////

    public void SetTotalMonsters(int numbOfBasicMonsters, int numbOfJuicyMonsters)
    {
        monstersTotal = numbOfBasicMonsters + numbOfJuicyMonsters;
        monstersJuicy = numbOfJuicyMonsters;
        monstersBasic = numbOfBasicMonsters;
    }

    public void MonsterKilled(MONSTERTYPE type, int index, Vector3 killedPos, bool killedInQuest)
    {
        if (type == MONSTERTYPE.BASIC) monstersKilledBasic++;
        if (type == MONSTERTYPE.JUICY) monstersKilledJuicy++;

        MonsterKilledStats(type, index, killedPos, killedInQuest);
        monstersKilledTotal++;
    }

    public void MonsterKilledStats(MONSTERTYPE type, int index, Vector3 killedPos, bool killedInQuest)
    {
        if (type == MONSTERTYPE.BASIC)
        {
            monsterKilledStatsBasicList[index].killed = true;
            monsterKilledStatsBasicList[index].killedTime = gameTime;
            monsterKilledStatsBasicList[index].killedPos = killedPos;
            monsterKilledStatsBasicList[index].killedInQuest = killedInQuest;
        }
        if (type == MONSTERTYPE.JUICY)
        {
            monsterKilledStatsJuicyList[index].killed = true;
            monsterKilledStatsJuicyList[index].killedTime = gameTime;
            monsterKilledStatsJuicyList[index].killedPos = killedPos;
            monsterKilledStatsJuicyList[index].killedInQuest = killedInQuest;
        }
    }

    public void AddToMonsterCount(MONSTERTYPE type)
    {
        if (type == MONSTERTYPE.BASIC)
        {
            monstersBasic++;
            monstersTotal++;
        }
        if (type == MONSTERTYPE.JUICY)
        {
            monstersJuicy++;
            monstersTotal++;
        }
    }

    /////////////////////////////Compare////////////////////////////

    private int Compare(CubeStat positionA, CubeStat positionB)
    {
        Vector3 va = positionA.position;
        Vector3 vb = positionB.position;
        if (Mathf.Abs(va.x - vb.x) < 0.001f)
        {
            if (Mathf.Abs(va.y - vb.y) < 0.001f) return 0;
            return va.y < vb.y ? -1 : 1;
        }
        return va.x < vb.x ? -1 : 1;
    }

    private int Compare(CubeDestroyedStat positionA, CubeDestroyedStat positionB)
    {
        Vector3 va = positionA.position;
        Vector3 vb = positionB.position;
        if (Mathf.Abs(va.x - vb.x) < 0.001f)
        {
            if (Mathf.Abs(va.y - vb.y) < 0.001f) return 0;
            return va.y < vb.y ? -1 : 1;
        }
        return va.x < vb.x ? -1 : 1;
    }

    ///////////////////////End of Functions/////////////////////////
}