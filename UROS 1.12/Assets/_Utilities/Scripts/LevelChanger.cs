using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Change level class
public class LevelChanger : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    // Reference to the animator
    public Animator anim;

    // Level to load index
    private int levelToLoad;

    // Static LevelChanger instance
    public static LevelChanger instance;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Set static instance to this
        instance = this;
    }

    // Fade out to the level
    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        anim.SetTrigger(AnimationNames.levelFadeOut);
    }

    // When the fade out has completed
    public void OnFadeComplete()
    {
        // Start the load game coroutine
        StartCoroutine(LoadScene());
    }

    // Load scene coroutine
    IEnumerator LoadScene()
    {
        // Load game asynchronously (in the background)
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            // Calculate the loading process percentage
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // Loading completed
            if (asyncLoad.progress == 0.9f)
                asyncLoad.allowSceneActivation = true;

            yield return null;
        }
    }

    ///////////////////////End of Functions/////////////////////////
}