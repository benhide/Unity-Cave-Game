using UnityEngine;

public class MiniMapScaler : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("MiniMap scaling settings")]
    public int spaceToAdd;
    public float sizeMultiplier;

    [Header("Component references")]
    public RectTransform rectTransform;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // assign the component
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Size the map item to the screen pixel percentage
        rectTransform.sizeDelta = new Vector2((Screen.width * sizeMultiplier) + spaceToAdd, (Screen.height * sizeMultiplier) + spaceToAdd);
    }

    ///////////////////////End of Functions/////////////////////////
}