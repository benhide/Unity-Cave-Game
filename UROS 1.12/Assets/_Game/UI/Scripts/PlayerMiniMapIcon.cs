using UnityEngine;
using UnityEngine.UI;

public class PlayerMiniMapIcon : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("MiniMap icon settings")]
    public Image miniMapImageMale;
    public Image miniMapImageFemale;

    // References
    //public GameDataManager GameDataManager.instance;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the reference to the game data manager
        //GameDataManager.instance = GameDataManager.instance;

        // Set the character icons and characters -- male selected
        if (GameDataManager.instance.SelectedGender() == GENDER.MALE)
            MiniMapController.RegisterMapObject(gameObject, miniMapImageMale);

        // Set the character icons and characters -- female selected
        else if (GameDataManager.instance.SelectedGender() == GENDER.FEMALE)
            MiniMapController.RegisterMapObject(gameObject, miniMapImageFemale);
    }

    // When the gameobject is destroyed
    void OnDestroy()
    {
        MiniMapController.RemoveMapObject(gameObject);
    }

    ///////////////////////End of Functions/////////////////////////
}