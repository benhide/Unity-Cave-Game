using UnityEngine;

// Civillian quest
public class Civillian : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Civillian settings")]
    GameObject player;
    HUDItem civillianHUDItem = new HUDItem();
    string saveMeText;
    public ParticleSystem savedCivillian;
    public ParticleSystem killedCivillian;
    public AudioClip killedClip;

    [Header("Civillian sprites")]
    public Sprite femaleCivillian;
    public Sprite maleCivillian;
    public Sprite catCivillian;
    public Sprite femaleCivillianIcon;
    public Sprite maleCivillianIcon;
    public Sprite catCivillianIcon;

    // Script references
    public QuestManager questManager;
    public HUD hud;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        // Assign the references
        player = GameObject.FindGameObjectWithTag(Tags.playerTag);
        questManager = GameObject.FindGameObjectWithTag(Tags.gameControllerTag).GetComponent<QuestManager>();
        hud = GameObject.FindGameObjectWithTag(Tags.uiTag).GetComponent<HUD>();

        // Get the animator and sprite renderer
        SpriteRenderer renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Animator anim = GetComponentInChildren<Animator>();

        // Set civillian type
        switch (GameDataManager.instance.CivillianType())
        {
            case CIVILLIANTYPE.MALE:
                civillianHUDItem = new HUDItem() { ID = 0, HUDSprite = maleCivillianIcon };
                renderer.sprite = maleCivillian;
                anim.SetBool("Human", true);
                break;
            case CIVILLIANTYPE.FEMALE:
                civillianHUDItem = new HUDItem() { ID = 0, HUDSprite = femaleCivillianIcon };
                renderer.sprite = femaleCivillian;
                anim.SetBool("Human", true);
                break;
            case CIVILLIANTYPE.CAT:
                civillianHUDItem = new HUDItem() { ID = 0, HUDSprite = catCivillianIcon };
                renderer.sprite = catCivillian;
                anim.SetBool("Cat", true);
                break;
        }

        // Set the stats
        GameDataManager.instance.SetCivillianStats(transform.position, false, false);
    }

    // Update is called once per frame
    void Update()
    {
        // Look at the player
        transform.LookAt(player.transform.position);
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerStay(Collider other)
    {
        // If the other collider is the player
        if (other.gameObject.tag == Tags.playerTag)
        {
            // If the player presses action saves civillian
            if (other.gameObject.GetComponent<PlayerController>().IsAction())
            {
                // Set the civillian stats
                GameDataManager.instance.SetCivillianStats(transform.position, false, true);

                // Add the civillian to the hud
                hud.AddCivillian(civillianHUDItem);

                // Play sound fx and particles - destroy gameobject
                questManager.PlaySFX();
                ParticleSystemController.InstaniateParticleSystem(savedCivillian, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            // Player kills civillian
            else if (other.gameObject.GetComponent<PlayerController>().IsDiggingAttacking())
            {
                // Set the civillian stats
                GameDataManager.instance.SetCivillianStats(transform.position, true, false);

                // Set the rotation
                Quaternion rotation = Quaternion.identity;
                rotation.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);

                // Play sound fx and particles - destroy gameobject
                questManager.PlaySFX(killedClip);
                ParticleSystemController.InstaniateParticleSystem(killedCivillian, transform.position, rotation);
                Destroy(gameObject);
            }
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // If the other collider is the player
        if (other.gameObject.tag == Tags.playerTag)
        {
            // Set civillian type
            switch (GameDataManager.instance.CivillianType())
            {
                case CIVILLIANTYPE.MALE:
                    saveMeText = StaticStrings.civillianQuestTextOne + " " + StaticStrings.civillianMaleText + " " + StaticStrings.civillianQuestTextTwo;
                    break;
                case CIVILLIANTYPE.FEMALE:
                    saveMeText = StaticStrings.civillianQuestTextOne + " " + StaticStrings.civillianFemaleText + " " + StaticStrings.civillianQuestTextTwo;
                    break;
                case CIVILLIANTYPE.CAT:
                    saveMeText = StaticStrings.civillianQuestTextOne + " " + StaticStrings.civillianCatText + " " + StaticStrings.civillianQuestTextTwo;
                    break;
            }

            // Display the quest text
            questManager.CivillianQuestText(saveMeText);
        }
    }

    ///////////////////////End of Functions/////////////////////////
}
