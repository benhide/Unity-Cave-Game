/**
 * @file    ArcadeGameTest.cs
 * @author  Benjamin Williams <bwilliams@lincoln.ac.uk>
 *
 * @license CC 3.0 <https://creativecommons.org/licenses/by-nc-nd/3.0/>
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SimpleFirebaseUnity;

/// <summary>
/// ArcadeGameTest
/// </summary>
public class FirebaseTest : MonoBehaviour 
{
    /// <summary>
    /// The settings.
    /// </summary>
    [SerializeField]
    public FirebaseSettings firebaseSettings;

    /// <summary>
    /// Just a list of data about when the user pressed the "jump" button --
    /// every time the user presses space, a "jump" entry is added to this list. Each
    /// entry contains a timestamp and a random number (for no reason).
    /// </summary>
    [SerializeField]
    public List<Jump> jumps = new List<Jump>();

    [System.Serializable]
    public class Jump
    {
        /// <summary>
        /// The time the player jumped
        /// </summary>
        public float time;

        /// <summary>
        /// The random number
        /// </summary>
        public float randNum;
    }

    public void Update()
    {
        //Just add a jump if up is pressed
        if (ArcadeKeymap.player1.up.isDown)
            jumps.Add(new Jump { time = Time.time, randNum = Random.value });

        //If the user has pressed the a key, then push data 
        //to firebase
        if (ArcadeKeymap.player1.A.isDown)
        {
            //Just push the data
            FirebaseManager.PushList<Jump>(firebaseSettings, jumps, (firebase, snapshot) =>
            {
                //Log the response
                Debug.Log("<b>response:</b> " + snapshot.RawJson);
            });
        }

        //Has escape been pressed? If so.. then exit
        if (ArcadeKeymap.player1.buttonExit.isDown)
            ArcadeGame.exit();
    }
}
