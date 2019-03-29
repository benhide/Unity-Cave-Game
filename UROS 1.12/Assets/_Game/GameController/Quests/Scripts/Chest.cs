using UnityEngine;

// Pickup controller class
public class Chest : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    // Chest settings
    public int points;
    public ParticleSystem collectable;
    GameObject key;
    Color chestColour;
    bool chestOpened = false;
    bool completed = false;
    bool keyCollected = false;
    int index;

    // References
    public QuestManager questManager;

    // References
    public Animator anim;

    // Event callbacks
    public delegate void OnChestOpenedEvent(GameObject chest);
    public static event OnChestOpenedEvent onChestOpened;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Set the rotation 
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(90.0f, transform.rotation.y, transform.rotation.z);
        transform.GetChild(0).gameObject.transform.rotation = rotation;

        // Assign reference
        questManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<QuestManager>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the colour
        GetComponentInChildren<SpriteRenderer>().material.color = chestColour;

        // If the pickup has been collected
        if (chestOpened && !completed)
        {
            if (onChestOpened != null)
                onChestOpened.Invoke(gameObject);

            // Play sfx
            questManager.PlaySFX();
            ParticleSystemController.InstaniateParticleSystem(collectable, transform.position, Quaternion.identity);

            // Chest opened
            completed = true;
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // If the other collider is the player
        if (other.gameObject.tag == Tags.playerTag)
        {
            // If the key has been collected
            if (keyCollected)
            {
                // Open the chest 
                chestOpened = true;
                anim.SetBool("Key", true);

                // Aadd points 
                other.gameObject.GetComponent<PlayerController>().AddToScore(points);

                // Remove minimap icon
                MiniMapController.RemoveMapObject(gameObject);
            }
            // Key not collected
            else if (!keyCollected && !chestOpened)
                anim.SetTrigger("NoKey");
        }
    }

    //////////////////////////Set and Gets//////////////////////////

    public void KeyCollected(bool collected)
    {
        keyCollected = collected;
    }

    public bool KeyCollected()
    {
        return keyCollected;
    }

    public void SetKey(GameObject keyGO)
    {
        key = keyGO;
    }

    public Key GetKey()
    {
        return key.GetComponent<Key>();
    }

    public void SetColour(Color colour)
    {
        chestColour = colour;
    }

    public Color GetColour()
    {
        return chestColour;
    }

    public void Index(int index)
    {
        this.index = index;
    }

    public int Index()
    {
        return index;
    }

    ///////////////////////End of Functions/////////////////////////
}
