using UnityEngine;

// Health pickup classs
public class HealthPickup : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    // Pickup settings
    public float rotationSpeed;
    public int health;
    public ParticleSystem collectable;
    bool healthCollected = false;
    int index;

    // Script reference
    public GameController gameController;
    public QuestManager questManager;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the references
        questManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<QuestManager>();
        gameController = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<GameController>();

        // Set the rotation
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(90.0f, transform.rotation.y, transform.rotation.z);
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the pickup
        transform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime));

        // If the pickup has been collected
        if (healthCollected)
        {
            // Health has been collectd
            gameController.HealthCollected();

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
            healthCollected = true;
            other.gameObject.GetComponent<PlayerController>().AddToHealth(health);
            MiniMapController.RemoveMapObject(gameObject);
            GameDataManager.instance.HealthCollected(Index());
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
