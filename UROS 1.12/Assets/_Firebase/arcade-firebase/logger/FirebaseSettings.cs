/**
 * @file    FirebaseSettings.cs
 * @author  Benjamin Williams <bwilliams@lincoln.ac.uk>
 *
 * @license CC 3.0 <https://creativecommons.org/licenses/by-nc-nd/3.0/>
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// FirebaseSettings
/// </summary>
[System.Serializable]
public class FirebaseSettings
{
    //
    // The public firebase details for the arcade
    // machines is as follows:
    //
    // url:     arcade-a85a0.firebaseio.com
    // api_key: O2qgoTxUhEOrL9FE2aThj6cfe3GP0kIhr7gagXmc
    // path:    <your game name>
    // 

    /// <summary>
    /// The URL to the firebase database.
    /// </summary>
    [SerializeField]
    [Tooltip("This is the URL to the firebase database. You can find this my looking in the settings of your firebase project. (Project Settings > Add Firebase to your web app)")]
    public string url = "arcade-a85a0.firebaseio.com";
    //public string url = "unity-test-33b1b.firebaseio.com";

    /// <summary>
    /// The secret api key
    /// </summary>
    [SerializeField]
    [Tooltip("This is your API key secret. You can find this by navigating to your firebase project console and going to Project Settings > Service Accounts > Database Secrets")]
    public string apiKey = "O2qgoTxUhEOrL9FE2aThj6cfe3GP0kIhr7gagXmc";
    //public string apiKey = "0euLe4SHSRmxU6QFcAMpOcSJzk6PlDJxstpuLzpR";

    [Tooltip("The path in the firebase database to push responses under (usually, the name of your game)")]
    public string path = "your-game-name";

    /// <summary>
    /// Initializes a new instance of the <see cref="FirebaseSettings"/> class.
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="apiKey">API key.</param>
    public FirebaseSettings(string url, string apiKey)
    {
        this.apiKey = apiKey;
        this.url = url;
    }


    /// <param name="inst">Inst.</param>
    public static implicit operator KeyValuePair<string, string> (FirebaseSettings inst)
    {
        return new KeyValuePair<string, string>(inst.url, inst.apiKey);
    }
}
