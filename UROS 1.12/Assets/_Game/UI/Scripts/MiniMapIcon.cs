using UnityEngine;
using UnityEngine.UI;

// MiniMapIcon class
public class MiniMapIcon : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("MiniMap icon settings")]
    public Image miniMapImage;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        MiniMapController.RegisterMapObject(gameObject, miniMapImage);
    }

    // When the gameobject is destroyed
    void OnDestroy()
    {
        MiniMapController.RemoveMapObject(gameObject);
    }

    ///////////////////////End of Functions////////////////////////
}