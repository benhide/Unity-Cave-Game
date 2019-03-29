using System.Collections;
using UnityEngine;

// Monster movement class
public class MonsterMovement : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Monster movement settings")]
    [Range(0.1f, 5.0f)]
    public float movementSpeed;
    [Range(0.25f, 5.0f)]
    public float chaseMovementSpeed;
    [Range(0.1f, 10.0f)]
    public float rotationSpeed;
    [Range(1.0f, 10.0f)]
    public float rotationSpeedMultiplier;

    [Header("Monster wander settings")]
    public float heading;
    public float directionChangeInterval;
    [Range(-360.0f, 360.0f)]
    public float minHeadingChange;
    [Range(-360.0f, 360.0f)]
    public float maxHeadingChange;
    [Range(0.1f, 10.0f)]
    public float minDirectionChangeInterval;
    [Range(0.1f, 10.0f)]
    public float maxDirectionChangeInterval;
    [Range(0.1f, 5.0f)]
    public float distanceToObsticle;
    [Range(0.0f, 30.0f)]
    public float degreesFromCompletion;
    public bool rotatingFromObsticle = false;

    [Header("Monster chase/attack settings")]
    [Range(0.1f, 10.0f)]
    public float attackDistance;
    [Range(0.1f, 10.0f)]
    public float dangerZoneDistance;
    public bool attack = false;

    [Header("Monster wander timer settings")]
    public float wanderPauseTimer;
    public float wanderPauseTime;
    [Range(0.1f, 10.0f)]
    public float minWanderPauseTimer;
    [Range(0.1f, 10.0f)]
    public float maxWanderPauseTimer;
    [Range(0.1f, 10.0f)]
    public float minWanderPauseTime;
    [Range(0.1f, 10.0f)]
    public float maxWanderPauseTime;

    [Header("Monster chase timer settings")]
    public float chasePauseTimer;
    public float chasePauseTime;
    [Range(0.1f, 10.0f)]
    public float minChasePauseTimer;
    [Range(0.1f, 10.0f)]
    public float maxChasePauseTimer;
    [Range(0.1f, 10.0f)]
    public float minChasePauseTime;
    [Range(0.1f, 10.0f)]
    public float maxChasePauseTime;

    [Header("Monster vectors settings")]
    public Quaternion targetRotation;
    public Vector3 directionVector;
    public Vector3 offsetVector;

    [Header("Monster poition and rotation")]
    public Vector3 currentPosition = Vector3.zero;
    public Vector3 lastPosition = Vector3.zero;
    public Vector3 currentRotation = Vector3.zero;
    public Vector3 lastRotation = Vector3.zero;

    [Header("Script references")]
    public MonsterController monsterController;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Script references
        monsterController = GetComponent<MonsterController>();

        // Set the min and max heading change and direction change interval
        heading = Random.Range(0.0f, 360.0f);
        directionChangeInterval = Random.Range(minDirectionChangeInterval, maxDirectionChangeInterval);

        // Initialise the monster
        InitialiseMonster();
    }

    // Initialise the monster 
    void InitialiseMonster()
    {
        // Set the last and current position
        currentPosition = transform.position;
        lastPosition = currentPosition;

        // Set the last and current rotation
        currentRotation = transform.rotation.eulerAngles;
        lastRotation = currentRotation;

        // Start the new heading coroutine
        StartCoroutine(NewHeading());

        // Set the pause timers
        ResetWanderTimers();
        ResetChaseTimers();

        // Clamp the min max values
        ClampMinMaxValues();
    }

    // Update is called once per frame
    public void MovementUpdate()
    {
        // Set the current and last positions and rotations of the player
        SetPositionsAndRotations();

        // Should the movement animation play or not
        PlayAnimation();

        // If the monster is alive
        if (!monsterController.monsterHealth.MonsterDead())
        {
            // If the player is not in sight wander
            if (!monsterController.monsterSight.PlayerInSight())
            {
                // Wander
                Wander();
            }

            // If the player is in sight and not under sneak attack chase the player
            else if (monsterController.monsterSight.PlayerInSight() && !attack /*&& !underSneakAttack*/)
            {
                // Chase the player
                Chase();

                // Set the monster as attacking if in the players danger zone
                attack = DangerZone();
            }

            // If the monster is attacking
            else if (attack)
            {
                // Attack the player
                Attack();

                // Set the monster as attacking if in the players danger zone
                attack = DangerZone();
            }
        }

        // Lock rotation and position
        LockRoatationAndPosition();
    }

    // Monster wander
    void Wander()
    {
        // Avoid obsticle
        if (!AvoidObsticle())
        {
            // Reset the chase timers
            ResetChaseTimers();

            // Start the pause timer
            wanderPauseTimer -= Time.deltaTime;

            // If the pause timer is less than zero
            if (wanderPauseTimer <= 0.0f)
            {
                // Set the movement of the monster
                SetMovement(Vector3.zero, 0.0f, true);

                // Start the wander pause time
                wanderPauseTime -= Time.deltaTime;

                // If monster paused for long enough reset timers
                if (wanderPauseTime <= 0.0f)
                    ResetWanderTimers();
            }

            // Continue wander
            else
            {
                // Set the transform rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Set the movement of the monster
                SetMovement(Vector3.forward, movementSpeed, true);
            }
        }
    }

    // Chase the player
    void Chase()
    {
        // Reset the wander timers
        ResetWanderTimers();

        // Start the chase timer
        chasePauseTimer -= Time.deltaTime;

        // If the chase time is less than zero
        if (chasePauseTimer <= 0.0f)
        {
            // Set the movement of the monster
            SetMovement(Vector3.zero, 0.0f, true);

            // Start the chase pause time
            chasePauseTime -= Time.deltaTime;

            // If monster paused for long enough reset timers
            if (chasePauseTime <= 0.0f)
                ResetChaseTimers();
        }

        // Else the chase pause timer is greater than 0
        else
        {
            // Check the distance to player - rotate quickly if within attack distance
            if (Vector3.Distance(monsterController.player.transform.position, transform.position) < attackDistance)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(monsterController.player.transform.position - transform.position), (rotationSpeed * rotationSpeedMultiplier) * Time.deltaTime);

            // Rotate slower if the player is outside the attack distance
            else
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(monsterController.player.transform.position - transform.position), rotationSpeed * Time.deltaTime);

            // Set the movement of the monster
            SetMovement(Vector3.forward, chaseMovementSpeed, true);
        }
    }

    // Monster attacks the player
    void Attack()
    {
        // Set rotating from an obsticle as false
        rotatingFromObsticle = false;

        // Reset the chase and wander timers
        ResetChaseTimers();
        ResetWanderTimers();

        // Get the attack slot direction
        Vector3 direction = monsterController.player.transform.position - transform.position;

        // If the magnitude of the durection vector is greater than 0.01f - move to attack
        if (direction.magnitude > 0.001f)
        {
            // Set the movement of the monster
            monsterController.characterController.Move(direction.normalized * chaseMovementSpeed * Time.deltaTime);
        }

        // Set the rotation to look at the player
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(monsterController.player.transform.position - transform.position), (rotationSpeed * rotationSpeedMultiplier) * Time.deltaTime);

        // Get the difference in rotations between the current and target roatation
        float rotationDifference = transform.rotation.eulerAngles.y - Quaternion.LookRotation(monsterController.player.transform.position - transform.position).eulerAngles.y;

        // If the difference in roatations is less than 0.25f
        if (Mathf.Abs(rotationDifference) <= degreesFromCompletion)
        {
            // look at the player
            transform.LookAt(monsterController.player.transform.position);
        }
    }

    // Is the monster in the players danger zone
    public bool DangerZone()
    {
        // If the distance from the player to monster is less than the danger zone distance return true
        return Vector3.Distance(monsterController.player.transform.position, transform.position) < dangerZoneDistance;
    }

    // Avoid an obsticle function
    bool AvoidObsticle()
    {
        // Raycast in fornt of monster
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // If the monster is not already avoiding an obsticle
        if (!rotatingFromObsticle)
        {
            // If the ray hits an object
            if (Physics.Raycast(ray.origin, ray.direction, out hit, distanceToObsticle))
            {
                // If the hit is not the player 
                if (hit.transform.tag != Tags.playerTag)
                {
                    // Set the target rotation from the hit normal and set rotating as true
                    targetRotation = Quaternion.LookRotation(hit.normal);
                    rotatingFromObsticle = true;
                    Debug.DrawLine(transform.position, hit.point, Color.white);
                }
            }
        }

        // Else if the monster is avoiding an obsticle
        else
        {
            // Update the transfrom roatation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Set the movement of the monster
            SetMovement(Vector3.forward, movementSpeed, true);

            // Get the difference in rotations between the current and target roatation
            float rotationDifference = transform.rotation.eulerAngles.y - targetRotation.eulerAngles.y;

            // If the difference in roatations is less than 0.25f
            if (Mathf.Abs(rotationDifference) <= degreesFromCompletion)
            {
                // Set the roatation as complete
                rotatingFromObsticle = false;

                // Update the heading
                heading = transform.rotation.eulerAngles.y;

                // Set a new direction change interval and start new heading coroutine
                directionChangeInterval = Random.Range(minDirectionChangeInterval, maxDirectionChangeInterval);
                StartCoroutine(NewHeading());
            }
        }

        // Rotating from obsticle completed
        return rotatingFromObsticle;
    }

    // Set the monster movement
    void SetMovement(Vector3 direction, float speed, bool transformDirection)
    {
        // If transforming the direction
        if (transformDirection)
        {
            // Set the direction vector
            directionVector = transform.TransformDirection(direction);
        }

        // Caluclate the direction vector using movement speed
        directionVector *= speed;

        // Move the character controller
        monsterController.characterController.Move(directionVector * Time.deltaTime);
    }

    // Set the current and last positions and rotations of the player
    void SetPositionsAndRotations()
    {
        // Set the last and current position
        lastPosition = currentPosition;
        currentPosition = transform.position;

        // Set the last and current rotation
        lastRotation = currentRotation;
        currentRotation = transform.rotation.eulerAngles;
    }

    // Should the movement animation play or not
    void PlayAnimation()
    {
        // If the monster is moving (position or rotation changed)
        if (currentPosition != lastPosition || currentRotation != lastRotation)
        {
            // Play the moving animation
            monsterController.animator.SetBool(AnimationNames.monsterMoving, true);
        }

        // Else monster is not moving
        else
        {
            // Stop animation and looping
            monsterController.animator.SetBool(AnimationNames.playerMoving, false);
        }
    }

    // Coroutine to calculate and set the monsters heading
    IEnumerator NewHeading()
    {
        // Loop infinitly
        while (!rotatingFromObsticle)
        {
            // Calculate the new heading
            NewHeadingRoutine();

            // Wait to calculate new heading
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    // Calculate the monsters new heading
    void NewHeadingRoutine()
    {
        // Set the floor and ceil of the heading change
        float floor = Mathf.Clamp(heading - minHeadingChange, 0.0f, 360.0f);
        float ceil = Mathf.Clamp(heading + maxHeadingChange, 0.0f, 360.0f);

        // Set the new heading
        heading = Random.Range(floor, ceil);

        // Set the target rotation
        targetRotation = Quaternion.Euler(0.0f, heading, 0.0f);
    }

    // Reset the pause timers
    void ResetWanderTimers()
    {
        wanderPauseTime = Random.Range(minWanderPauseTime, maxWanderPauseTime);
        wanderPauseTimer = Random.Range(minWanderPauseTimer, maxWanderPauseTimer);
    }

    // Reset the pause timers
    void ResetChaseTimers()
    {
        chasePauseTime = Random.Range(minChasePauseTime, maxChasePauseTime);
        chasePauseTimer = Random.Range(minChasePauseTimer, maxChasePauseTimer);
    }

    // Lock the y position and x and z rotations
    void LockRoatationAndPosition()
    {
        // Lock x and z rotation and y position
        if (transform.eulerAngles.x != 0.0f || transform.eulerAngles.z != 0.0f)
            transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);

        if (transform.position.y > 0.0f)
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
    }

    // Clamp the min/max monster settings
    void ClampMinMaxValues()
    {
        minDirectionChangeInterval = Mathf.Clamp(minDirectionChangeInterval, 0.1f, maxDirectionChangeInterval);
        maxDirectionChangeInterval = Mathf.Clamp(maxDirectionChangeInterval, minDirectionChangeInterval, 10.0f);
        minHeadingChange = Mathf.Clamp(minHeadingChange, -360.0f, minDirectionChangeInterval);
        maxHeadingChange = Mathf.Clamp(maxHeadingChange, minHeadingChange, 360.0f);
    }

    ///////////////////////End of Functions/////////////////////////
}