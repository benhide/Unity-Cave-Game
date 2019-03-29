using UnityEngine;

// Minimap camera class
public class MinimapCamera : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Transform to follow")]
    public Transform player;

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

    // Update is called once per frame
    void /*Late*/Update()
    {
        // Get the desired camera position from the players position
        Vector3 camPosition = player.position;

        // Adjust the camera positions y component
        camPosition.y = transform.position.y;

        // Keep the minimap within the map boundaries Left and right
        if (player.transform.position.x <= minX) camPosition = new Vector3(minX, transform.position.y, player.transform.position.z);
        if (player.transform.position.x >= maxX) camPosition = new Vector3(maxX, transform.position.y, player.transform.position.z);

        // Top and bottom
        if (player.transform.position.z <= minZ) camPosition = new Vector3(player.transform.position.x, transform.position.y, minZ);
        if (player.transform.position.z >= maxZ) camPosition = new Vector3(player.transform.position.x, transform.position.y, maxZ);

        // Corners
        if (player.transform.position.x <= minX && player.transform.position.z <= minZ) camPosition = new Vector3(minX, transform.position.y, minZ);
        if (player.transform.position.x <= minX && player.transform.position.z >= maxZ) camPosition = new Vector3(minX, transform.position.y, maxZ);
        if (player.transform.position.x >= maxX && player.transform.position.z <= minZ) camPosition = new Vector3(maxX, transform.position.y, minZ);
        if (player.transform.position.x >= maxX && player.transform.position.z >= maxZ) camPosition = new Vector3(maxX, transform.position.y, maxZ);

        // Assign the calculated camera position to the camera
        transform.position = camPosition;
    }

    ///////////////////////End of Functions/////////////////////////
}
