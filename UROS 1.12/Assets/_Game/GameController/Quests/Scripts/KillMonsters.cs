using UnityEngine;

// Kill monsters game mode
public class KillMonsters : IQuest
{
    // Has the game mode been won
    public bool questComplete = false;
    public bool questInitialised = false;
    public string questName = StaticStrings.monsterQuestName;
    public string questTextOne = StaticStrings.monsterQuestTextOne;
    public string questTextTwo = StaticStrings.monsterQuestTextTwo;
    public string questTextCompleted = StaticStrings.monsterQuestTextComplete;

    // References
    public QuestManager questManager;
    public HUD hud;

    // Initialises gamemode
    public void Init()
    {
        // References
        questManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<QuestManager>();
        hud = GameObject.FindGameObjectWithTag(Tags.uiTag).GetComponent<HUD>();

        // Callbacks/events
        MonsterHealth.onMonsterKilled += MonsterKilled;
        Debug.Log("Kill Monsters game mode init!");

        // Set the total monsters to kill
        questManager.SetTotalMonsters();

        // Set the quest text
        hud.SetQuestProgressText(questManager.MonstersKilled() + "/" + questManager.TotalMonsters());
        hud.SetQuestProgressItem(questManager.monsterSprite);
    }

    // Monster killed in game mode
    public void MonsterKilled(GameObject monster)
    {
        Debug.Log("Monster killed!");

        // Count the pickup collected
        questManager.MonsterKilled();

        // Set the screen ui and destroy the key
        hud.SetQuestProgressText(questManager.MonstersKilled() + "/" + questManager.TotalMonsters());

        // If the chests have all been opened
        if (questManager.AllMonstersKilled())
        {
            questComplete = true;
            Debug.Log("Monsters killed quest complete");
        }
    }

    // Generates quest items
    public void GenerateQuestItems()
    {
    }

    //////////////////////////Set and Gets//////////////////////////

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
        return questTextOne + " " + questManager.TotalMonsters() + " " + questTextTwo;
    }

    public string QuestTextCompleted()
    {
        return questTextCompleted;
    }

    ///////////////////////End of Functions/////////////////////////
}
