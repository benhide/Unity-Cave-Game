using UnityEngine;
using UnityEngine.UI;

public class CivillianMiniMapIcon : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("MiniMap icon settings")]
    public Image miniMapImageMale;
    public Image miniMapImageFemale;
    public Image miniMapImageCat;

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
        if (GameDataManager.instance.CivillianType() == CIVILLIANTYPE.MALE)
            MiniMapController.RegisterMapObject(gameObject, miniMapImageMale);

        // Set the character icons and characters -- female selected
        else if (GameDataManager.instance.CivillianType() == CIVILLIANTYPE.FEMALE)
            MiniMapController.RegisterMapObject(gameObject, miniMapImageFemale);

        // Set the character icons and characters -- female selected
        else if (GameDataManager.instance.CivillianType() == CIVILLIANTYPE.CAT)
            MiniMapController.RegisterMapObject(gameObject, miniMapImageCat);
    }

    // When the gameobject is destroyed
    void OnDestroy()
    {
        MiniMapController.RemoveMapObject(gameObject);
    }

    ///////////////////////End of Functions/////////////////////////
}