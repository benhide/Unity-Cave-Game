/**
 * @file    FirebaseLogger.cs
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
/// FirebaseLogger
/// </summary>
public partial class FirebaseLogger
{
    /// <summary>
    /// The firebase instance.
    /// </summary>
    private Firebase firebase;

    /// <summary>
    /// The queue.
    /// </summary>
    private FirebaseQueue queue;

    /// <summary>
    /// Setups the firebase instance.
    /// </summary>
    private void setupFirebaseInstance()
    {
        //Firebase instance
        firebase = Firebase.CreateNew(settings.url, settings.apiKey);

        //Create a new queue
        queue = new FirebaseQueue(true);
    }
}
