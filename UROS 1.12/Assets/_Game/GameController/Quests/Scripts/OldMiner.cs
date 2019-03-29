using UnityEngine;

// Old miner class
public class OldMiner : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    // Old miner settings
    GameObject player;
    bool startQuest = false;
    string startTextOne = StaticStrings.startQuestText;
    string startTextTwo = StaticStrings.startQuestTextContinued;

    // Script references
    public QuestManager questManager;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the player

        player = GameObject.FindGameObjectWithTag(Tags.playerTag);
        questManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Look at the player
        transform.LookAt(player.transform.position);
    }

    // Player faces old miner
    public void SetPlayerRotation()
    {
        // Rotate the player
        GameObject.FindGameObjectWithTag(Tags.playerTag).transform.LookAt(transform.position);
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // If the other collider is the player
        if (other.gameObject.tag == Tags.playerTag)
        {
            // If start of the game - first quest
            if (!startQuest)
            {
                Debug.Log("OLD MINER - QUEST INIT - STARTED");

                questManager.InitQuest(10, startTextOne, startTextTwo);
                player.GetComponent<PlayerController>().ActionsEnabled();
                startQuest = true;

                Debug.Log("OLD MINER - QUEST INIT - COMPLETED");
            }

            // If current quest completed
            else if (questManager.CurrentQuestCompleted())
            {
                Debug.Log("OLD MINER POSITIONED - QUEST INIT - STARTED");

                questManager.ChestQuestCompleted();
                questManager.InitQuest(10);
                other.gameObject.GetComponent<PlayerController>().AddTNT();

                Debug.Log("OLD MINER POSITIONED - QUEST INIT - COMPLETE");
            }

            // If current quest not complete
            else if (startQuest && !questManager.CurrentQuestCompleted())
            {
                questManager.QuestText(StaticStrings.questIncompleteText + questManager.CurrentQuestText(), 10);
            }
        }
    }

    ///////////////////////End of Functions/////////////////////////
}