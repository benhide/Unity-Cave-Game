using System.Collections.Generic;
using UnityEngine;

// TNT class
public class TNT : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("TNT Settings")]
    public float timer;
    public int playerDamage;
    public ParticleSystem explosion;
    public AudioSource audioSource;
    public AudioSource sfx;
    public AudioClip explosionClip;

    [Header("Player TNT GameObjects lists")]
    public List<GameObject> cubes;
    public List<GameObject> monsters;
    GameObject player;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.playerTag);
        sfx = GameObject.FindGameObjectWithTag(Tags.sfxsExtraTag).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Reduce the timer
        if (timer > 0)
            timer -= Time.deltaTime;

        // If the timer is finished - explosion
        if (timer < 0)
        {
            // Destroy grid cubes - kill monsters - hurt player
            KillMonster();
            HurtPlayer();
            DestroyGridCubes();
            sfx.PlayOneShot(explosionClip);
            Destroy(gameObject);
        }
    }

    // Check if explosion destroys rocks
    void DestroyGridCubes()
    {
        // Loop through all cubes in the cubes list
        foreach (GameObject cube in cubes)
        {
            // Check that there are cubes
            if (cubes.Count > 0 && cube != null)
            {
                // Get the cube data
                CubeData cubeData = cube.GetComponent<CubeData>();

                // If the cube can be destroyed 
                if (cubeData.CubeDestructible())
                    cubeData.CubeDamaged(cubeData.CurrentHealth());
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

        // Play the particles
        ParticleSystemController.InstaniateParticleSystem(explosion, transform.position, Quaternion.identity);
    }

    // Check if explosion kills monsters
    void KillMonster()
    {
        // Loop through each monster in the monsters list
        foreach (GameObject monster in monsters)
        {
            // If monster exsists
            if (monster != null)
            {
                // If the monster is not dead, not null and player is digging/attacking
                if (monster != null && !monster.GetComponent<MonsterController>().monsterHealth.MonsterDead())
                {
                    // If health is greater than 0
                    if (monster != null && monster.GetComponent<MonsterController>().monsterHealth.CurrentHealth() > 0)
                    {
                        // Kill the monster
                       monster.GetComponent<MonsterController>().monsterHealth.DamageMonster(monster.GetComponent<MonsterController>().monsterHealth.CurrentHealth());
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

    // Hurt the player
    void HurtPlayer()
    {
        // If the player is in explosion radius
        if (player != null)
            player.GetComponent<PlayerController>().PlayerDamaged(playerDamage);
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

        // If the other collider is the player add it to player gameobject
        if (other.gameObject.tag == Tags.playerTag)
            player = other.gameObject;
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

        // If the other collider is the player remove it to player gameobject
        if (other.gameObject.tag == Tags.playerTag)
            player = null;
    }

    ///////////////////////End of Functions/////////////////////////
}