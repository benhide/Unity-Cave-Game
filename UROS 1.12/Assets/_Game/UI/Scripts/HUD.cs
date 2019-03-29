using UnityEngine;
using UnityEngine.UI;

// HUD class
public class HUD : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("HUD GameObjects and UI")]
    public Image questProgressItem;
    public Text questProgressText;

    [Header("HUD Images")]
    public Image[] itemImages = new Image[6];
    public Image[] tntImages = new Image[3];
    public Image civillianImage;

    [Header("HUD Text")]
    public Text playerHealthText;
    public Text playerScoreText;
    public Text playerPointsText;

    //// Static HUD instance
    //public static HUD instance;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        //// Set static instance to this
        //instance = this;
    }

    // Add key to the HUD
    public void AddKey(HUDItem key, Color keyColour)
    {
        itemImages[key.ID].sprite = key.HUDSprite;
        itemImages[key.ID].color = keyColour;
    }

    // Remove key from the HUD
    public void RemoveKey(HUDItem key, HUDItem artefact)
    {
        itemImages[key.ID].sprite = null;
        itemImages[key.ID].color = Color.clear;
        AddArtefact(key.ID, artefact);
    }

    // Add the artefact
    public void AddArtefact(int id, HUDItem artefact)
    {
        itemImages[id].sprite = artefact.HUDSprite;
        itemImages[id].color = Color.white;
    }

    // Remove the artefact
    public void RemoveArtefact(int id)
    {
        itemImages[id].sprite = null;
        itemImages[id].color = Color.clear;
    }

    // Add TNT to HUD
    public void AddTNT(HUDItem tnt)
    {
        tntImages[tnt.ID].sprite = tnt.HUDSprite;
        tntImages[tnt.ID].color = Color.white;
    }

    // Remove TNT from HUD
    public void RemoveTNT(HUDItem tnt)
    {
        tntImages[tnt.ID].sprite = null;
        tntImages[tnt.ID].color = Color.clear;
    }

    // Add civillian to HUD
    public void AddCivillian(HUDItem civillian)
    {
        civillianImage.sprite = civillian.HUDSprite;
        civillianImage.color = Color.white;
    }

    // Set the quest progress text
    public void SetQuestProgressText(string text)
    {
        questProgressText.text = text;
    }

    // Set the quest progress text
    public void SetQuestProgressItem(Sprite itemSprite)
    {
        questProgressItem.sprite = itemSprite;
    }

    // Set text alignment
    public void SetTextAlignment()
    {
        questProgressText.alignment = TextAnchor.MiddleLeft;
    }

    ///////////////////////End of Functions/////////////////////////
}
