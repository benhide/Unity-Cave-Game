using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Player name select class
public class NameCharSelect : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Player name select index and character array")]
    public int nameCharIndex = 0;
    public char[] nameChars = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    [Header("Player SFXs and audio sources")]
    public AudioSource soundFx;
    public AudioClip menuBeep;
    public MainMenu mainMenu;

    [Header("Script references")]
    public Controls controls;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the reference to the controls and game data manager
        controls = mainMenu.controls;

        // Initialise the audio clip
        soundFx = mainMenu.soundFx;
        menuBeep = mainMenu.menuBeep;
    }

    // Update is called once per frame
    void Update()
    {
        // If the currently selected gameobject is this object
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            // If down change the inputfield char
            if (controls.down)
            {
                // Play audio and increase index
                soundFx.PlayOneShot(menuBeep);
                nameCharIndex++;

                // Loop back to the start
                if (nameCharIndex > nameChars.Length - 1)
                    nameCharIndex = 0;
            }

            // If up change the inputfield char
            if (controls.up)
            {
                // Play audio and decrease index
                soundFx.PlayOneShot(menuBeep);
                nameCharIndex--;

                // Loop back to the end
                if (nameCharIndex < 0)
                    nameCharIndex = nameChars.Length - 1;
            }
        }

        // Set the text field
        GetComponent<InputField>().text = nameChars[nameCharIndex].ToString();
    }

    ///////////////////////End of Functions/////////////////////////
}