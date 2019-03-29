using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Map object class
public class MapObject
{
    // The icon image and owner
    public Image icon { get; set; }
    public GameObject owner { get; set; }
}

// Minimap controller class
public class MiniMapController : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("MiniMap controller settings")]
    public GameObject player;
    public Camera miniMapCamera;

    [Header("List of map objects")]
    public static List<MapObject> mapObjects = new List<MapObject>();

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the player
        player = GameObject.FindGameObjectWithTag(Tags.playerTag);

        // Assign the camera
        miniMapCamera = GameObject.FindGameObjectWithTag(Tags.miniMapCameraTag).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Draw the map icons
        DrawMapIcons();
    }

    // Register map objects
    public static void RegisterMapObject(GameObject go, Image img)
    {
        // Insatantiate the map image icon
        Image image = Instantiate(img);
        mapObjects.Add(new MapObject() { owner = go, icon = image });
    }

    // Remove map objects
    public static void RemoveMapObject(GameObject go)
    {
        // New list of map objects
        List<MapObject> newList = new List<MapObject>();

        // Loop through mapobjects
        for (int i = 0; i < mapObjects.Count; i++)
        {
            // If the owner is the passed gameobject destroy the icon and continue
            if (mapObjects[i].owner == go)
            {
                Destroy(mapObjects[i].icon);
                continue;
            }

            // Add it to the list
            else
                newList.Add(mapObjects[i]);
        }

        // Remove old list and add new list
        mapObjects.RemoveRange(0, mapObjects.Count);
        mapObjects.AddRange(newList);
    }

    // Draw the map icons to th UI
    void DrawMapIcons()
    {
        // Loop through the map icons
        foreach (MapObject mapObject in mapObjects)
        {
            // Get the screen position and set the parent gameobject
            Vector3 screenPos = miniMapCamera.WorldToViewportPoint(mapObject.owner.transform.position);
            mapObject.icon.transform.SetParent(transform);

            // Get the rect transform attached to this game obect
            RectTransform rectTransform = GetComponent<RectTransform>();

            // Get the corners of the rect
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            // Set the screen pos x, y and z
            screenPos.x = Mathf.Clamp(screenPos.x * rectTransform.rect.width + corners[0].x, corners[0].x, corners[2].x);
            screenPos.y = Mathf.Clamp(screenPos.y * rectTransform.rect.height + corners[0].y, corners[0].y, corners[1].y);
            screenPos.z = 0;

            // Assign the position
            mapObject.icon.transform.position = screenPos;

            // Get the transforms of the icon and owner
            Transform iconTransform = mapObject.icon.transform;
            Transform ownerTransform = mapObject.owner.transform;

            // If the icon is for the player - match the players rotation
            if (mapObject.owner == player || mapObject.owner.tag == Tags.monsterTag || mapObject.owner.tag == Tags.oldMinerTag)
                iconTransform.eulerAngles = new Vector3(iconTransform.rotation.x, iconTransform.rotation.y, -ownerTransform.eulerAngles.y);
        }
    }

    ///////////////////////End of Functions/////////////////////////
}