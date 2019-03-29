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
using SimpleFirebaseUnity.MiniJSON;

using System.Text.RegularExpressions;

/// <summary>
/// FirebaseLogger
/// </summary>
public partial class FirebaseLogger
{
    /// <summary>
    /// The path pattern.
    /// </summary>
    private Regex pathPattern = new Regex(@"(\S+?)\.(.+)");

    /// <summary>
    /// The assignment pattern.
    /// </summary>
    private Regex assignmentPattern = new Regex(@"(\S+)\s*\=\s*(\S+)");

    /// <summary>
    /// Parses the path.
    /// </summary>
    /// <returns>The path.</returns>
    /// <param name="path">Path.</param>
    private List<string> parsePaths(string path)
    {
        //Create a return path
        var parsedPaths = new List<string>();

        //Split all paths by comma
        var splitPaths = path.Split(',');

        foreach (var splitPath in splitPaths)
        {
            //Build a temporary string
            var tempString = splitPath;
            
            while (pathPattern.IsMatch(tempString))
            {
                //Replace
                tempString = pathPattern.Replace(tempString, "{ \"$1\" : $2 }");
            }

            //Replace last
            tempString = assignmentPattern.Replace(tempString, "{ \"$1\" : $2 }");

            //Add the parsed path
            parsedPaths.Add(tempString);
        }

        //Return it
        return parsedPaths;
    }
}
