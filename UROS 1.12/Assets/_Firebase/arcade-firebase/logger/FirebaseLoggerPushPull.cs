/**
 * @file    FirebasePushPull.cs
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
    /// Push the specified json string.
    /// </summary>
    /// <param name="json">Json.</param>
    public void set(string json)
    {
        //Data
        var paths = parsePaths(json);

        //Push the data
        foreach (var data in paths)
            queue.AddQueueSet(firebase, data, true);
    }

    public void push(object obj, System.Action<Firebase, DataSnapshot> callback = null)
    {
        //Serialise the object into JSON
        var serialisedJson = JsonUtility.ToJson(obj);

        if (callback != null)
            firebase.OnPushSuccess += callback;

        //Push serialised data
        queue.AddQueuePush(firebase.Child("unity/" + settings.path), serialisedJson, true);
    }

    /// <summary>
    /// Lists/arrays/ienumerables are not serialisable in unity,
    /// by default. So, this is just a wrapper class which can
    /// serialise them
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class SerializableArray<T>
    {
        /// <summary>
        /// The array we're gonna store
        /// </summary>
        public T[] data;

        /// <summary>
        /// Constructs a serialisable array with an array type
        /// </summary>
        /// <param name="obj"></param>
        public SerializableArray(T[] obj)
        {
            this.data = obj;
        }

        /// <summary>
        /// Constructs a serialisable array with an ienumerable
        /// collection (e.g. lists, arrays, etc)
        /// </summary>
        /// <param name="obj"></param>
        public SerializableArray(IEnumerable<T> obj)
        {
            this.data = obj.ToArray();
        }
    }

    /// <summary>
    /// Pushes a list to firebase, using a serialisable array
    /// </summary>
    /// <typeparam name="T">The type the collection contains</typeparam>
    /// <param name="obj">The collection to push</param>
    /// <param name="callback">The callback to invoke when firebase sends back confirmation</param>
    public void pushList<T>(List<T> obj, System.Action<Firebase, DataSnapshot> callback = null)
    {
        push(new SerializableArray<T>(obj), callback);
    }

    /// <summary>
    /// Pushes an array to firebase, using a serialisable array
    /// </summary>
    /// <typeparam name="T">The type the collection contains</typeparam>
    /// <param name="obj">The collection to push</param>
    /// <param name="callback">The callback to invoke when firebase sends back confirmation</param>
    public void pushArray<T>(T[] obj, System.Action<Firebase, DataSnapshot> callback = null)
    {
        push(new SerializableArray<T>(obj), callback);
    }
}
