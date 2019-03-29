/**
 * @file    ArcadeGame.cs
 * @author  Benjamin Williams <bwilliams@lincoln.ac.uk>
 *
 * @license CC 3.0 <https://creativecommons.org/licenses/by-nc-nd/3.0/>
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Arcade game class, for functionality with firebase
/// </summary>
public partial class ArcadeGame 
{
    /// <summary>
    /// The firebase settings.
    /// </summary>
    public FirebaseSettings settings;


    /// <summary>
    /// The logger.
    /// </summary>
    public FirebaseLogger logger;


    /// <summary>
    /// Initializes a new instance of the <see cref="ArcadeGame"/> class.
    /// </summary>
    public ArcadeGame(FirebaseSettings settings)
    {
        //Set up settings
        this.settings = settings;

        //Set up logger
        logger = new FirebaseLogger(settings);
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="ArcadeGame"/> class.
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="apiKey">API key.</param>
    public ArcadeGame(string url, string apiKey)
    {
        //Make some new settings
        var settings = new FirebaseSettings(url, apiKey);

        //Set up settings for this instance
        this.settings = settings;

        //Set up logger
        logger = new FirebaseLogger(settings);
    }
}
