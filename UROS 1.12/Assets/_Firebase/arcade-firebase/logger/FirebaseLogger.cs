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

/// <summary>
/// FirebaseLogger
/// </summary>
public partial class FirebaseLogger
{
    /// <summary>
    /// The settings.
    /// </summary>
    private FirebaseSettings settings;


    /// <summary>
    /// Gets the URL.
    /// </summary>
    /// <value>The URL.</value>
    public string url
    {
        get { return this.settings.url; }
    }


    /// <summary>
    /// Gets the API key.
    /// </summary>
    /// <value>The API key.</value>
    public string apiKey
    {
        get { return this.settings.apiKey; }
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="FirebaseLogger"/> class.
    /// </summary>
    /// <param name="settings">Settings.</param>
    public FirebaseLogger(FirebaseSettings settings)
    {
        //Set up settings
        this.settings = settings;

        //Sets up the firebase instance
        setupFirebaseInstance();
    }
}
