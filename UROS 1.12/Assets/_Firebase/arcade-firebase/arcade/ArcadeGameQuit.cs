/**
 * @file    ArcadeGameQuit.cs
 * @author  Benjamin Williams <bwilliams@lincoln.ac.uk>
 *
 * @license CC 3.0 <https://creativecommons.org/licenses/by-nc-nd/3.0/>
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ArcadeGameQuit
/// </summary>
public partial class ArcadeGame
{
    /// <summary>
    /// The back URL.
    /// </summary>
    public const string backURL = "../..";


    /// <summary>
    /// Quit this instance.
    /// </summary>
    public static void quit()
    {
        //This is an alias for exit
        exit();
    }


    /// <summary>
    /// Exit this instance.
    /// </summary>
    public static void exit()
    {
        //Open up the URL to get back to the arcade menu
        Application.OpenURL(backURL);
    }
}
