using UnityEngine;

// Camera follow player class
public class FollowPlayerCamera : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Gameobject to follow")]
    public Transform player;

    [Header("Camera Settings")]
    public float speed = 2.0f;
    public float maxDist = 2.0f;

    [Header("Noise variables")]
    public float frequency = 1.0f;
    public float magnitude = 1.0f;

    [Header("Map boundaries for camera")]
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Find the players transform
        player = GameObject.FindGameObjectWithTag(Tags.playerTag).transform;
    }

    // Adds perlin noise to the camera position
    Vector2 perlinNoise2D(Vector2 sample, float frequency = 1.0f, float magnitude = 1.0f)
    {
        // x and y coord noise -  controlled by the frequecny and magnitude
        float xNoise = (Mathf.PerlinNoise(sample.x * frequency, 0) - 0.5f) * magnitude;
        float zNoise = (Mathf.PerlinNoise(0, sample.y * frequency) - 0.5f) * magnitude;

        // Returns the noise as a vector2
        return new Vector2(xNoise, zNoise);
    }

    // Update is called once per frame
    void /*Late*/Update()
    {
        // Take a sample from the current time
        Vector2 timeSample = new Vector2(Time.time, Time.time);

        // Create the perlin noise from the time sample 
        Vector2 noise = perlinNoise2D(timeSample, frequency, magnitude);

        // Get the players position with added perlin noise
        Vector2 playerPos = new Vector2(player.position.x, player.position.z);

        // Left and right
        if (playerPos.x <= minX) playerPos = new Vector2(minX, player.position.z);
        if (playerPos.x >= maxX) playerPos = new Vector2(maxX, player.position.z);

        // Top and bottom
        if (playerPos.y <= minZ) playerPos = new Vector2(player.position.x, minZ);
        if (playerPos.y >= maxZ) playerPos = new Vector2(player.position.x, maxZ);

        // Corners
        if (playerPos.x <= minX && playerPos.y <= minZ) playerPos = new Vector2(minX, minZ);
        if (playerPos.x <= minX && playerPos.y >= maxZ) playerPos = new Vector2(minX, maxZ);
        if (playerPos.x >= maxX && playerPos.y <= minZ) playerPos = new Vector2(maxX, minZ);
        if (playerPos.x >= maxX && playerPos.y >= maxZ) playerPos = new Vector2(maxX, maxZ);

        // Add plerlin noise
        playerPos += noise;

        // Get the position of the camera
        Vector3 position = transform.position;

        // Calculate the distance from player to camera
        float dist = Vector2.Distance(playerPos, position);

        // Calculate the new camera speed from the defualt speed and clamped distance from the player
        float newSpeed = speed * Mathf.Clamp(dist / maxDist, 0, 1);

        // Sets the x coord of the cameras position
        position.x = Mathf.Lerp(transform.position.x, playerPos.x, newSpeed * Time.deltaTime);

        // Sets the x and z coord of the cameras position
        position.z = Mathf.Lerp(transform.position.z, playerPos.y, newSpeed * Time.deltaTime);

        // Sets the position of the camera
        transform.position = position;
    }

    ///////////////////////End of Functions/////////////////////////
}