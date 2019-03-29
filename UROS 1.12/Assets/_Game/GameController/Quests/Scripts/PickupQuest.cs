#pragma warning disable 0414
using UnityEngine;

// Collect pick game mode class
public class PickupQuest : IQuest
{
    ///////////////////////////Variables////////////////////////////

    // Collect treasure game mode items
    public GameObject pickupGO;
    bool questComplete = false;
    bool questInitialised = false;
    string questName = StaticStrings.pickupQuestName;
    string questText = StaticStrings.pickupQuestText;
    string questTextCompleted = StaticStrings.pickupQuestTextCompleted;
    public Color pickupColor;

    // References
    public QuestManager questManager;
    public Map map;
    public HUD hud;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////


    // Initialises quest
    public void Init()
    {
        // References
        questManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<QuestManager>();
        map = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<Map>();
        hud = GameObject.FindGameObjectWithTag(Tags.uiTag).GetComponent<HUD>();

        // Quest initialised
        questInitialised = true;

        // Callbacks/events
        Pickup.onPickupCollected += PickupCollected;
        Debug.Log("Collect Pickups quest initialised!");

        // Count the total game pickups
        questManager.SetTotalPickups();

        // Load the pickup prefab
        pickupGO = questManager.pickup;

        // Set the game mode text
        hud.SetQuestProgressText(questManager.PickupsCollected() + "/" + questManager.TotalPickups());
        hud.SetQuestProgressItem(questManager.pickupSprite);

        // Set the colours
        SetColours();
    }

    // Pick up has been collected
    public void PickupCollected(GameObject pickup)
    {
        Debug.Log("Pickup collected!");

        // Count the pickup collected
        questManager.PickupCollected(pickup.GetComponent<Pickup>().Index());

        // Set the screen ui 
        hud.SetQuestProgressText(questManager.PickupsCollected() + "/" + questManager.TotalPickups());

        // If the pickups have all been collected
        if (questManager.AllPickupsCollected())
        {
            questComplete = true;
            Debug.Log("Collect Pickups quest complete");
        }
    }

    // Generates game mode items
    public void GenerateQuestItems()
    {
        // Loop through quest items
        for (int i = 0; i < questManager.TotalPickups(); i++)
        {
            // Instatiate the pickups and position in the map
            GameObject pickup = Object.Instantiate(pickupGO, Vector3.zero, Quaternion.identity, GameObject.FindGameObjectWithTag(Tags.objectiveTag).transform) as GameObject;
            pickup.GetComponent<SpriteRenderer>().color = pickupColor;

            // Set initial position
            map.PositionGameObjectInEmptyCube(pickup);

            // Distance from key to chest
            float dist = Vector3.Distance(pickup.transform.position, GameObject.FindGameObjectWithTag(Tags.playerTag).transform.position);

            // While the pickup is closer to the player than minimum pickup to player
            while (dist < questManager.MinPickupPlayerDist())
            {
                // Reposition pickup
                map.PositionGameObjectInEmptyCube(pickup);
                dist = Vector3.Distance(pickup.transform.position, GameObject.FindGameObjectWithTag(Tags.playerTag).transform.position);
            }
            pickup.GetComponent<Pickup>().Index(GameDataManager.instance.RecordPickupPosition(pickup.transform.position));
        }
    }

    // Set the colours
    public void SetColours()
    {
        // Set colours CVD
        if (GameDataManager.instance.SelectedColourScheme() == COLOURSCHEME.CVD)
        {
            // Red safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.RED)
                pickupColor = Colours.pickUpColourRedSafe;

            // Green safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.GREEN)
                pickupColor = Colours.pickUpColourGreenSafe;

            // Blue safe
            if (GameDataManager.instance.SelectedCVDColourScheme() == CVDCOLOURSCHEME.BLUE)
                pickupColor = Colours.pickUpColourBlueSafe;
        }

        // Set colours Normal
        else if (GameDataManager.instance.SelectedColourScheme() == COLOURSCHEME.NORMAL)
            pickupColor = Colours.pickUpColour;
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
        return questText;
    }

    public string QuestTextCompleted()
    {
        return questTextCompleted;
    }

    ///////////////////////End of Functions/////////////////////////
}