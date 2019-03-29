using UnityEngine;

// Key controller class
public class Key : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Key settings")]
    public float rotationSpeed;
    bool keyCollected = false;
    int index;

    // Key Items
    public ParticleSystem collectable;
    GameObject chest;
    Color keyColour;
    HUDItem keyItem;
    HUDItem artefactItem;

    // References
    public QuestManager questManager;

    // Event callbacks
    public delegate void OnKeyCollectedEvent(GameObject pickup);
    public static event OnKeyCollectedEvent onKeyCollected;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Set the rotation
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(90.0f, transform.rotation.y, transform.rotation.z);
        transform.rotation = rotation;

        // Assign reference
        //questManager = QuestManager.instance;
        questManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the key
        transform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime));

        // Set the colour
        GetComponent<SpriteRenderer>().color = keyColour;

        // If the pickup has been collected
        if (keyCollected)
        {
            // Call back
            if (onKeyCollected != null)
                onKeyCollected.Invoke(gameObject);

            // Play sfx
            questManager.PlaySFX();
            ParticleSystemController.InstaniateParticleSystem(collectable, transform.position, Quaternion.identity);

            // Key has been collected
            chest.GetComponent<Chest>().KeyCollected(true);

            // Remove the key
            gameObject.SetActive(false);
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // If the other collider is the player collect the pickup
        if (other.gameObject.tag == Tags.playerTag)
        {
            // Collect the key - remove minimap icon
            keyCollected = true;
            MiniMapController.RemoveMapObject(gameObject);
        }
    }

    //////////////////////////Set and Gets//////////////////////////

    public void DestroyKey()
    {
        Destroy(gameObject);
    }

    public void SetChest(GameObject chestGO)
    {
        chest = chestGO;
    }

    public void SetColour(Color colour)
    {
        keyColour = colour;
    }

    public Color GetColour()
    {
        return keyColour;
    }

    public HUDItem GetHUDItemKey()
    {
        return keyItem;
    }

    public HUDItem GetHUDItemArtefact()
    {
        return artefactItem;
    }

    public void SetHUDItem(int id, Sprite keySprite, Sprite artefactSprite)
    {
        keyItem = new HUDItem() { ID = id, HUDSprite = keySprite, };
        artefactItem = new HUDItem() { ID = id, HUDSprite = artefactSprite, };
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
