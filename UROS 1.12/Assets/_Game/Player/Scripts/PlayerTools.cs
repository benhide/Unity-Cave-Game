using System.Collections.Generic;
using UnityEngine;

// Player tools class
public class PlayerTools : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Player tools settings")]
    bool diggingAttacking = false;
    int attackDamage = 1;

    [Header("Player tools GameObjects lists")]
    public List<GameObject> cubes;
    public List<GameObject> monsters;

    // Script references
    private PlayerController playerController;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the reference to the player controller
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set if the player is digging
        diggingAttacking = playerController.IsDiggingAttacking();

        // Is the player digging through rocks
        RockDigging();

        // Is the player attacking monsters
        MonsterAttack();
    }

    // Check for player digging and destroy rocks
    void RockDigging()
    {
        // Loop through all cubes in the cubes list
        foreach (GameObject cube in cubes)
        {
            // Check that there are cubes
            if (cubes.Count > 0 && cube != null)
            {
                // Get the cube data
                CubeData cubeData = cube.GetComponent<CubeData>();

                // If the cube can be destroyed and the player is digging and if health of the cubes is greater than 0 
                if (cubeData.CubeDestructible() && diggingAttacking && cubeData.CurrentHealth() > 0)
                    cubeData.CubeDamaged(1);
            }
        }

        // If there are cubes in the cubes list
        if (cubes.Count > 0)
        {
            // Loop through each cube
            for (int i = 0; i < cubes.Count; i++)
            {
                // If the cube has been destroyed remove it from the cubes list
                if (cubes[i] == null || cubes[i].GetComponent<CubeData>().CubeDestroyed())
                    cubes.Remove(cubes[i].gameObject);
            }
        }
    }

    // Check if the player is attacking monsters
    void MonsterAttack()
    {
        // Loop through each monster in the monsters list
        foreach (GameObject monster in monsters)
        {
            // If monster exsists
            if (monster != null)
            {
                // Get the monster controller
                MonsterController monControl = monster.GetComponent<MonsterController>();

                // If the monster is not dead, not null and player is digging/attacking
                if (!monControl.monsterHealth.MonsterDead() && diggingAttacking)
                {
                    // If health is greater than 0
                    if (monControl.monsterHealth.CurrentHealth() > 0)
                    {
                        // Take some health off the monster
                        monControl.monsterHealth.DamageMonster(attackDamage);
                    }
                }
            }
        }

        // If there are monsters in the monsters list
        if (monsters.Count > 0)
        {
            // Loop through each monster
            for (int i = 0; i < monsters.Count; i++)
            {
                // If the monster is dead or null 
                if (monsters[i].GetComponent<MonsterController>().monsterHealth.MonsterDead() || monsters[i] == null)
                {
                    // Remove it from the list
                    monsters.Remove(monsters[i].gameObject);
                }
            }
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // If the other collider is a grid cube add it to the cubes list
        if (other.gameObject.tag == Tags.gridCubeTag)
            cubes.Add(other.gameObject);

        // If the other collider is a monster add it to the monsters list
        if (other.gameObject.tag == Tags.monsterTag)
        {
            // If there are monsters in the list
            if (monsters.Count > 0)
            {
                // If the monster is a unique monster in the monsters list add the monsters to the list
                if (!monsters.Contains(other.gameObject))
                    monsters.Add(other.gameObject);
            }

            // Monster list is empty so add the monster
            else
                monsters.Add(other.gameObject);
        }
    }

    // OnTriggerExit is called when the Collider other exits the trigger
    void OnTriggerExit(Collider other)
    {
        // If the other collider is a grid cube remove it from the cubes list
        if (other.gameObject.tag == Tags.gridCubeTag)
            cubes.Remove(other.gameObject);

        // If the other collider is a monster remove it from the monsters list
        if (other.gameObject.tag == Tags.monsterTag)
            monsters.Remove(other.gameObject);
    }

    ///////////////////////End of Functions/////////////////////////
}