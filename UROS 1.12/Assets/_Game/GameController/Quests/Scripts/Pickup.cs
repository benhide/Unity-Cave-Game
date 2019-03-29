using UnityEngine;

// Pickup controller class
public class Pickup : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    // Pickup settings
    public float rotationSpeed;
    public int points;
    public ParticleSystem collectable;
    bool pickupCollected = false;
    int index;

    // References
    public QuestManager questManager;

    // Event callbacks
    public delegate void OnPickupCollectedEvent(GameObject pickup);
    public static event OnPickupCollectedEvent onPickupCollected;

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
        questManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the pickup
        transform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime));

        // If the pickup has been collected
        if (pickupCollected)
        {
            // Call back
            if (onPickupCollected != null)
                onPickupCollected.Invoke(gameObject);

            // Play sfx
            questManager.PlaySFX();
            ParticleSystemController.InstaniateParticleSystem(collectable, transform.position, Quaternion.identity);

            // Destory the pickup
            Destroy(gameObject);
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // If the other collider is the player collect the pickup
        if (other.gameObject.tag == Tags.playerTag)
        {
            // Collect the pickup - add points - remove minimap icon
            pickupCollected = true;
            other.gameObject.GetComponent<PlayerController>().AddToScore(points);
            MiniMapController.RemoveMapObject(gameObject);
        }
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