//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DangerZone : MonoBehaviour
//{
//    public List<GameObject> monstersList;
//    public int maxNumberOfMonsters;
//    public float seperationDistance;

//    // Use this for initialization
//    void Start()
//    {
//        monstersList = new List<GameObject>();
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    // OnTriggerStay is called when the Collider other stays the trigger
//    void OnTriggerEnter(Collider other)
//    {
//        // If the other collider is a monster
//        if (other.gameObject.tag == Tags.monsterTag)
//        {
//            if (!monstersList.Contains(other.gameObject) && monstersList.Count < maxNumberOfMonsters)
//            {
//                monstersList.Add(other.gameObject);
//                other.gameObject.GetComponent<MonsterMovement>().attack = true;
//                // monster in danger zone -- monster react!!!!
//            }
//            else
//            {
//                // too many monsters -- monsters react to this????
//                //Vector3 offsetVector = (other.gameObject.transform.position - transform.position).normalized;
//                //other.gameObject.GetComponent<MonsterMovement>().AddOffsetVector(offsetVector);
//            }
//        }
//    }

//    // OnTriggerExit is called when the Collider other exits the trigger
//    void OnTriggerExit(Collider other)
//    {
//        // If the other collider is the player
//        if (other.gameObject.tag == Tags.monsterTag)
//        {
//            if (monstersList.Contains(other.gameObject))
//            {
//                monstersList.Remove(other.gameObject);
//                //other.gameObject.GetComponent<MonsterMovement>().attack = false;
//            }
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Slot manager class
public class SlotManager : MonoBehaviour
{
    [Header("Slot manager lists")]
    public List<GameObject> attackSlots;
    public List<float> positionOffsets;
    public List<float> rotationOffsets;
    public List<bool> positionUpdates;
    public List<bool> rotationUpdates;

    public float positionOffset;
    public float rotationOffset;
    public int slotCount;

    public int currentNumberOfMonsters;
    public int maxNumberOfMonsters;

    [Header("Slot distance and offset settings")]
    public float slotDistance;
    public float minPositionOffset;
    public float maxPositionOffset;
    public float minRotationOffset;
    public float maxRotationOffset;

    [Header("Slot distance time settings")]
    public float minChangeTime;
    public float maxChangeTime;


    // Use this for initialization
    void Start()
    {
        // Assign the attack slot lists
        attackSlots = new List<GameObject>();

        // Assign the random position and rotation offset lists
        positionOffsets = new List<float>();
        rotationOffsets = new List<float>();

        // Assign the position and rotation update bools lists
        positionUpdates = new List<bool>();
        rotationUpdates = new List<bool>();

        // Loop thorough the attack slots 
        for (int index = 0; index < slotCount; ++index)
        {
            // Set all slots as null
            attackSlots.Add(null);

            // Generate a radom position and rotation offset
            positionOffsets.Add(Random.Range(minPositionOffset, maxPositionOffset));
            rotationOffsets.Add(Random.Range(minPositionOffset, maxPositionOffset));

            // Set the position and rotation updates bools as false
            positionUpdates.Add(false);
            rotationUpdates.Add(false);
        }

        // Set the max number of monsters
        maxNumberOfMonsters = slotCount;
    }

    // Update is called once per frame
    public void Update()
    {
        // Assign the number of monsters
        int numberOfMonsters = 0;

        // Loop through and count all the filled attack slots
        for (int i = 0; i < attackSlots.Count; i++)
        {
            if (attackSlots[i] != null)
                numberOfMonsters++;
        }

        // Set the current number of monsters as the monster count
        currentNumberOfMonsters = numberOfMonsters;
    }

    // Get the position of the slot
    public Vector3 GetSlotPosition(int index)
    {
        // Calculate the degrees around the player of the slot
        float degreesPerIndex = 360.0f / slotCount;

        // Assign the initial position of the slot
        Vector3 position = transform.position;

        // Calculate a new position and rotation offset
        PositionOffsetChange(index, Random.Range(minChangeTime, maxChangeTime));
        RotationOffsetChange(index, Random.Range(minChangeTime, maxChangeTime));

        // Calculate the offset of the slot position
        Vector3 positionOffset = new Vector3(0.0f, 0.0f, slotDistance + positionOffsets[index]);

        // Calculate the offset of the slot rotation
        float rotationOffset = (degreesPerIndex * index) + rotationOffsets[index];

        // Return the slot position
        return position + (Quaternion.Euler(new Vector3(0.0f, rotationOffset, 0.0f)) * positionOffset);
    }

    // Reserve a slot for an attacker
    public int Reserve(GameObject attacker)
    {
        // Assign the initial position
        Vector3 bestPosition = transform.position;

        // Calculate the offset - closet slot to the attacker
        Vector3 offset = (attacker.transform.position - bestPosition).normalized * slotDistance;

        // Calculate the new best position
        bestPosition += offset;

        // Set the best slot index as -1 and the best distance 
        int slotToTake = -1;
        float bestDistance = 99999.0f;

        // Loop through the attack slots
        for (int i = 0; i < attackSlots.Count; ++i)
        {
            // If the attack slot at the current index is taken
            if (attackSlots[i] != null)
                continue;

            // Calculate the distance from the slot position and the best position
            float distance = Vector3.Distance(GetSlotPosition(i), bestPosition);

            // If the distance from the slot position and the best position is less thean the best distance 
            if (distance < bestDistance)
            {
                // Assign the attack slot to take as the slot at the current index and update the best distance
                slotToTake = i;
                bestDistance = distance;
            }
        }

        // If the slot to take is not -1 - assign the attacker to the attack slot
        if (slotToTake != -1)
        {
            // Check to see if the attacker is spaced out in the attack slots and then assign to attack slot
            slotToTake = CheckAdjoiningSlots(slotToTake);
            attackSlots[slotToTake] = attacker;
        }

        // Return the slot to take
        return slotToTake;
    }

    // Check to see if adjoining slots are null
    int CheckAdjoiningSlots(int index)
    {
        // Assign the up and down index
        int upIndex = 0;
        int downIndex = 0;

        // if the index is greater than 0 and less than slot count -1 
        if (index >= 0 && index < slotCount - 1)
            upIndex = index + 1;

        // If the index is 0
        else if (index == 0)
        {
            // Set the up and down index
            upIndex = index + 1;
            downIndex = slotCount;
        }

        // If the index is slotCount
        else
        {
            // Set the up and down index
            upIndex = 0;
            downIndex = index - 1;
        }

        // If the index below the current index is null decrease the index
        if (attackSlots[upIndex] != null && attackSlots[downIndex] == null)
            index = downIndex;

        // If the index above the current index is null increase the index
        if (attackSlots[upIndex] == null && attackSlots[downIndex] != null)
            index = upIndex;

        // Return the updated index
        return index;
    }

    // Release the attack slot
    public void Release(int slot)
    {
        // Set the attack slot to null
        attackSlots[slot] = null;
    }

    // Calculate a new position offset
    void PositionOffsetChange(int index, float changeTime)
    {
        // If the position is not updating already - get a new position
        if (!positionUpdates[index])
            StartCoroutine(NewPosition(index, changeTime));
    }

    // Coroutine to calculate a random position offset
    IEnumerator NewPosition(int index, float changeTime)
    {
        // Set the position update to true and generate a new offset
        positionUpdates[index] = true;
        positionOffsets[index] = Random.Range(minPositionOffset, maxPositionOffset);

        // Wait to calculate new position offset
        yield return new WaitForSeconds(changeTime);

        // Position offset completed
        positionUpdates[index] = false;
    }

    // Calculate a new position offset
    void RotationOffsetChange(int index, float changeTime)
    {
        // If the roatation is not updating already - get a new roatation
        if (!rotationUpdates[index])
            StartCoroutine(NewRotation(index, changeTime));
    }

    // Coroutine to calculate a random rotation offset
    IEnumerator NewRotation(int index, float changeTime)
    {
        // Set the position update to true and generate a new offset
        rotationUpdates[index] = true;
        rotationOffsets[index] = Random.Range(minRotationOffset, maxRotationOffset);

        // Wait to calculate new position offset
        yield return new WaitForSeconds(changeTime);

        // Position offset completed
        rotationUpdates[index] = false;
    }

    //// NOT NEEDED !!!!!!!!
    //void OnDrawGizmos()
    //{
    //    for (int index = 0; index < slotCount; ++index)
    //    {
    //        if (attackSlots == null || attackSlots.Count <= index || attackSlots[index] == null)
    //            Gizmos.color = Color.black;
    //        else
    //            Gizmos.color = Color.red;

    //        Gizmos.DrawWireSphere(GetSlotPosition(index), 0.25f);
    //        //Gizmos.DrawCube (GetSlotPosition (index), new Vector3((index * 2.0f) / count,
    //        //	(index * 2.0f) / count, (index * 2.0f) / count));
    //    }
    //}
}
