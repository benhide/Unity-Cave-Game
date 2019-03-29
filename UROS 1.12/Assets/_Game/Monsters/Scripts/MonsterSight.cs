using UnityEngine;

// Monster sight class
public class MonsterSight : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Monster attack settings")]
    [Range(1, 10)]
    public float detectionDistance;
    [Range(1, 10)]
    public float detectionDistanceMultiplier;
    [Range(30, 360)]
    public float fieldOfViewAngle;
    public bool playerInSight = false;

    [Header("Script references")]
    public MonsterController monsterController;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Script references
        monsterController = GetComponent<MonsterController>();
    }

    // Update is called once per frame
    public void SightUpdate()
    {
        // Detect the player
        PlayerSighting();
    }

    // Detect the player
    void PlayerSighting()
    {
        // Get the direction of the player and angle from forward direction
        Vector3 direction = monsterController.player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        // Raycast to player
        RaycastHit hitToPlayer;

        // If the angle between the player and forward is less than the half the FOV angle
        if (angle < fieldOfViewAngle * 0.5f)
        {
            // Raycast to the player (detection distance)
            if (Physics.Raycast(transform.position, direction, out hitToPlayer, detectionDistance))
            {
                // If the ray hits the player set the player in sight and set the chase timer
                if (hitToPlayer.collider.gameObject == monsterController.player)
                {
                    playerInSight = true;
                    Debug.DrawLine(transform.position, hitToPlayer.point, Color.red);
                }
            }
        }

        // Calculate the player distance from monster
        float playerDist = Vector3.Distance(transform.position, monsterController.player.transform.position);

        // If the player has escaped the monster sight
        if (playerDist > detectionDistance * detectionDistanceMultiplier)
            playerInSight = false;
        else
            Debug.DrawLine(transform.position, monsterController.player.transform.position, Color.blue);
    }

    // Has the player been sighted
    public bool PlayerInSight()
    {
        return playerInSight;
    }

    // Set the player sighting
    public void PlayerInSight(bool inSight)
    {
        playerInSight = inSight;
    }

    ///////////////////////End of Functions/////////////////////////
}
