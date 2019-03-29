/**
 * @file    SampleResponse.cs
 * @author  Benjamin Williams <bwilliams@lincoln.ac.uk>
 *
 * @license CC 3.0 <https://creativecommons.org/licenses/by-nc-nd/3.0/>
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// SampleResponse
/// </summary>
[System.Serializable]
public class MouseLogger
{
    [System.Serializable]
    public struct MousePress
    {
        //The time the key was pressed
        public float time;

        //What key was pressed
        public int button;

        //Mouse x and y
        public Vector3 position;
    }

    /// <summary>
    /// The data.
    /// </summary>
    public List<MousePress> mousePressData = new List<MousePress>();
 
    /// <summary>
    /// Initializes a new instance of the <see cref="SampleResponse"/> class.
    /// </summary>
    /// <param name="name">Name.</param>
    public MouseLogger()
    {
        
    }

    /// <summary>
    /// Called once-per-frame (invoked from other script)
    /// </summary>
    public void update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            var mousePress = new MousePress();

            mousePress.button = 1;
            mousePress.time = Time.time;
            mousePress.position = Input.mousePosition;
            mousePressData.Add(mousePress);
        }
    }
}
