using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// End menu class
public class EndMenu : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Script references")]
    public LevelChanger levelChanger;

    [Header("Menu GameObjects")]
    public GameObject selectionIcons;
    public GameObject currentSelectedGameObject;
    public GameObject lastSelectedGameObject;
    public Text gameOverText;
    public Text nameText;
    public Text scoreText;

    [Header("Player SFXs and audio sources")]
    public AudioSource soundFx;
    public AudioSource music;
    public AudioClip menuBeep;
    public float musicFadeTime;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Fade in music and highlight start button
        StartCoroutine(FadeMusicIn(music, musicFadeTime));
        currentSelectedGameObject.GetComponentInChildren<Text>().color = Color.yellow;
        SetMenuText();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the selection icons and update the selection objects
        MoveSelectionIcons();

        // If the current selected is not the stored current selected
        if (EventSystem.current.currentSelectedGameObject != currentSelectedGameObject)
            PlaySound(menuBeep);

        // Update selection
        UpdateSelected();
    }

    // Move the selection icons and match to size of selection object
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

    // Start the game - load game scene
    public void PlayAgain()
    {
        // Fade out music
        StartCoroutine(FadeMusicOut(music));

        // Load the next level
        levelChanger.FadeToLevel(0);
    }

    // Exit the game
    public void ExitGame()
    {
        // Fade out music
        StartCoroutine(FadeMusicOut(music));

        // Quit the game
        ArcadeGame.exit();

        // Quit game
        Debug.Log("EXIT GAME HAS BEEN CALLED");
    }

    // Play sound bite
    void PlaySound(AudioClip clip)
    {
        // Play sound bite
        soundFx.PlayOneShot(clip);
    }

    // Set the end text values
    void SetMenuText()
    {
        // Set the texts
        if (GameDataManager.instance.PlayerHealth() <= 0)
            gameOverText.text = "GAME OVER";
        else
            gameOverText.text = "YOU WIN!";

        // Set the texts
        scoreText.text = "SCORE: " + GameDataManager.instance.PlayerScore().ToString();
        nameText.text = "NAME: " + GameDataManager.instance.PlayerName();
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