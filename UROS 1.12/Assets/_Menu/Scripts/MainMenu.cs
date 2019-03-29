using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Main menu class
public class MainMenu : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Name InputField references")]
    public InputField nameInputField1;
    public InputField nameInputField2;
    public InputField nameInputField3;

    [Header("Menu GameObjects")]
    public GameObject selectionIcons;
    public GameObject currentSelectedGameObject;
    public GameObject lastSelectedGameObject;
    public GameObject male;
    public GameObject female;

    [Header("Player SFXs and audio sources")]
    public AudioSource soundFx;
    public AudioSource music;
    public AudioClip menuBeep;
    public float musicFadeTime;

    [Header("Script references")]
    public LevelChanger levelChanger;
    public Controls controls;

    // Has the next menu page been opened
    bool nextPage = true;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the reference to the game data manager
        levelChanger = GameObject.FindGameObjectWithTag(Tags.levelChangerTag).GetComponent<LevelChanger>();
        controls = Controls.instance;

        // Fade in music and highlight start button
        StartCoroutine(FadeMusicIn(music, musicFadeTime));
        currentSelectedGameObject.GetComponentInChildren<Text>().color = Color.yellow;

        // Set the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the selection icons and update the selection objects
        MoveSelectionIcons();

        // If the current selected is not the stored current selected
        if (EventSystem.current.currentSelectedGameObject != currentSelectedGameObject && !nextPage)
            PlaySound(menuBeep);

        // If the current gameobject is the male gameobject (character select menu)
        if (EventSystem.current.currentSelectedGameObject == male)
        {
            // Colour the character sprites (male highlighted - female grayed out)
            female.transform.GetChild(0).GetComponentInChildren<Image>().color = Color.gray;
            male.transform.GetChild(0).GetComponentInChildren<Image>().color = Color.white;
        }

        // The current gameobject is the female gameobject (character select menu)
        else
        {
            // Colour the character sprites (female highlighted - male grayed out)
            male.transform.GetChild(0).GetComponentInChildren<Image>().color = Color.gray;
            female.transform.GetChild(0).GetComponentInChildren<Image>().color = Color.white;
        }

        // Move input fields
        if (EventSystem.current.currentSelectedGameObject == nameInputField1 && controls.enter)
            SelectInput("NameInputField2");

        // Move input fields
        if (EventSystem.current.currentSelectedGameObject == nameInputField2 && controls.enter)
            SelectInput("NameInputField3");

        // Move to play button
        if (EventSystem.current.currentSelectedGameObject == nameInputField3 && controls.enter)
        {
            SelectedName();
            SelectButton("PlayButton");
            soundFx.PlayOneShot(menuBeep);
        }

        // Update selection
        nextPage = false;
        UpdateSelected();
    }

    // Start the game - load game scene
    public void PlayGame()
    {
        // Keep the game data manager for thr next scene
        DontDestroyOnLoad(GameDataManager.instance);
        DontDestroyOnLoad(Controls.instance);

        // Load the next level
        levelChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);

        // Fade out music
        StartCoroutine(FadeMusicOut(music));
    }

    // Highscore menu called
    public void HighScores()
    {
        // Quit game
        Debug.Log("HIGHSCORES BEEN CALLED");
    }

    // Exit the game
    public void ExitGame()
    {
        // Quit the game
        ArcadeGame.exit();

        // Quit game
        Debug.Log("EXIT GAME HAS BEEN CALLED");
    }

    // Select the button to highlight in menu
    public void SelectButton(string buttonTag)
    {
        // Select the first button to highlight - string of button tag passed
        GameObject.FindGameObjectWithTag(buttonTag).GetComponent<Button>().Select();

        // Next page selected
        nextPage = true;
    }

    // Select the input field to highlight in menu
    public void SelectInput(string buttonTag)
    {
        // Select the first button to highlight - string of button tag passed
        GameObject.FindGameObjectWithTag(buttonTag).GetComponent<InputField>().Select();

        // Next page selected
        nextPage = true;
    }

    // Select the input field to highlight in menu
    public void SelectNextInput(string buttonTag)
    {
        // Select the first button to highlight - string of button tag passed
        GameObject.FindGameObjectWithTag(buttonTag).GetComponent<InputField>().Select();
    }

    // Select the first character to be highligthed in menu
    public void SelectCharacterButton(string buttonTag)
    {
        // Get a list of all characters
        GameObject[] characters = GameObject.FindGameObjectsWithTag(buttonTag);

        // Choose character at random - male highlighted
        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            characters[0].GetComponent<Button>().Select();
            GameDataManager.instance.SetInitialGender(GENDER.MALE);
        }

        // Female hightlighted
        else
        {
            characters[1].GetComponent<Button>().Select();
            GameDataManager.instance.SetInitialGender(GENDER.FEMALE);
        }

        // Next page selected
        nextPage = true;
    }

    // Select the colour settings
    public void SelectColourButton(string buttonTag)
    {
        // Find all the colour buttons
        GameObject[] colours = GameObject.FindGameObjectsWithTag(buttonTag);

        // Select a random starting option
        if (Random.Range(0.0f, 1.0f) > 0.5f)
            colours[0].GetComponent<Button>().Select();

        else
            colours[1].GetComponent<Button>().Select();

        // Next page selected
        nextPage = true;
    }

    // Record which character has been selected
    public void SelectedCharacter(int character)
    {
        // Record the character selected by the player (0 = male, 1 = female)
        if (character == 0) GameDataManager.instance.SetGender(GENDER.MALE);
        else if (character == 1) GameDataManager.instance.SetGender(GENDER.FEMALE);
    }

    // Record the selected font size
    public void SelectedFontSize(int size)
    {
        // Set the font size (0 = small, 1 = medium, 2 = large)
        if (size == 0) GameDataManager.instance.SetFontSize(FONTSIZE.SMALL);
        if (size == 1) GameDataManager.instance.SetFontSize(FONTSIZE.MEDIUM);
        if (size == 2) GameDataManager.instance.SetFontSize(FONTSIZE.LARGE);
    }

    // Record the selected name
    public void SelectedName()
    {
        // Set the player namee in the game data manager
        GameDataManager.instance.SetPlayerName(nameInputField1.text + nameInputField2.text + nameInputField3.text);
    }

    // Select the colour settings
    public void SelectColours(int colour)
    {
        // Set colour scheme
        if (colour == 0) GameDataManager.instance.SetColourScheme(COLOURSCHEME.CVD);
        else if (colour == 1)
        {
            GameDataManager.instance.SetColourScheme(COLOURSCHEME.NORMAL);
            GameDataManager.instance.SetCVDColourScheme(CVDCOLOURSCHEME.NONE);
        }
    }

    // Select the colour settings
    public void SelectCVDColours(int colour)
    {
        // Set colour scheme
        if (colour == 0) GameDataManager.instance.SetCVDColourScheme(CVDCOLOURSCHEME.RED);
        else if (colour == 1) GameDataManager.instance.SetCVDColourScheme(CVDCOLOURSCHEME.GREEN);
        else if (colour == 2) GameDataManager.instance.SetCVDColourScheme(CVDCOLOURSCHEME.BLUE);
    }

    //Move the selection icons and match to size of selection object
    public void MoveSelectionIcons()
    {
        // X/Y/Z components
        float xPos = EventSystem.current.currentSelectedGameObject.transform.position.x;
        float yPos = EventSystem.current.currentSelectedGameObject.transform.position.y;
        float zPos = EventSystem.current.currentSelectedGameObject.transform.position.z;

        // Set the position and size
        selectionIcons.transform.position = new Vector3(xPos, yPos, zPos);
        selectionIcons.GetComponent<RectTransform>().sizeDelta = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().sizeDelta;
    }

    // Update the cureent and last selected gameobject
    void UpdateSelected()
    {
        // If the current selected is not the stored current selected
        if (EventSystem.current.currentSelectedGameObject != currentSelectedGameObject)
        {
            // Update the last and current selected objects
            lastSelectedGameObject = currentSelectedGameObject;
            currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

            // Update the colours
            if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>() != null)
                EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().color = Color.yellow;

            // Update the colours
            if (lastSelectedGameObject != null)
            {
                // Update the colours
                if (lastSelectedGameObject.GetComponentInChildren<Text>() != null)
                    lastSelectedGameObject.GetComponentInChildren<Text>().color = Color.white;
            }
        }
    }

    // Play sound bite
    void PlaySound(AudioClip clip)
    {
        // Play sound bite
        soundFx.PlayOneShot(clip);
    }

    // Fade music in coroutine
    IEnumerator FadeMusicIn(AudioSource source, float multiplier = 1.0f)
    {
        float vol = 0.0f;
        while (vol < 1.0f)
        {
            vol += Time.deltaTime * multiplier;
            source.volume = vol;
            yield return new WaitForSeconds(0);
        }
        source.volume = 1.0f;
    }

    // Fade music out coroutine
    IEnumerator FadeMusicOut(AudioSource source, float multiplier = 1.0f)
    {
        float vol = 1.0f;
        while (vol > 0.0f)
        {
            vol -= Time.deltaTime * multiplier;
            source.volume = vol;
            yield return new WaitForSeconds(0);
        }
        source.volume = 0.0f;
    }

    ///////////////////////End of Functions/////////////////////////
}