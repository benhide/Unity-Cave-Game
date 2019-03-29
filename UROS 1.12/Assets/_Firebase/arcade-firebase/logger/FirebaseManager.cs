/**
 * @file    FirebaseManager.cs
 * @author  Benjamin Williams <bwilliams@lincoln.ac.uk>
 *
 * @license CC 3.0 <https://creativecommons.org/licenses/by-nc-nd/3.0/>
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFirebaseUnity;

public static class FirebaseManager
{
    /// <summary>
    /// Pushes data to firebase with a single line, instead of making a new instance
    /// of an ArcadeGame
    /// </summary>
    /// <param name="settings">The settings (location) to push to</param>
    /// <param name="obj">The serializable data</param>
    /// <param name="callback">The callback to call after pushing</param>
    public static void Push(FirebaseSettings settings, object obj, System.Action<Firebase, DataSnapshot> callback = null)
    {
        //Make a new arcade game
        var game = new ArcadeGame(settings);

        //And just call push
        game.logger.push(obj, callback);
    }

    /// <summary>
    /// Pushes a list to firebase with a single line, instead of making a new instance
    /// and all that jazz
    /// </summary>
    /// <param name="settings">The settings (location) to push to</param>
    /// <param name="obj">The list</param>
    /// <param name="callback">The callback to call after pushing</param>
    public static void PushList<T>(FirebaseSettings settings, List<T> obj, System.Action<Firebase, DataSnapshot> callback = null)
    {
        //Make a new arcade game
        var game = new ArcadeGame(settings);

        //And just call push
        game.logger.pushList<T>(obj, callback);
    }
}
