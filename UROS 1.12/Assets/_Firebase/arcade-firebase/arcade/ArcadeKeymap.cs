/**
 * @file    KeymapArcade.cs
 * @author  Benjamin Williams <bwilliams@lincoln.ac.uk>
 *
 * @license CC 3.0 <https://creativecommons.org/licenses/by-nc-nd/3.0/>
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArcadeKeymap
{
    /// <summary>
    /// An arcade key code, with some extra functionality
    /// </summary>
    [System.Serializable]
    public class ArcadeKeyCode
    {
        /// <summary>
        /// The actual keycode for this key
        /// </summary>
        public KeyCode key;

        /// <summary>
        /// Whether or not this key has been pressed down
        /// </summary>
        public bool isDown
        {
            get { return Input.GetKeyDown(this.key); }
        }

        /// <summary>
        /// Whether or not this key has been released
        /// </summary>
        public bool isUp
        {
            get { return Input.GetKeyUp(this.key); }
        }

        /// <summary>
        /// Whether or not this key is being held
        /// </summary>
        public bool isHeld
        {
            get { return Input.GetKey(this.key); }
        }

        /// <summary>
        /// Constructs a new arcade key code from a 
        /// unity keycode
        /// </summary>
        /// <param name="key"></param>
        public ArcadeKeyCode(KeyCode key)
        {
            this.key = key;
        }

        /// <summary>
        /// When implicitly converted to a string, return the
        /// string conversion on the wrapped enum type
        /// </summary>
        /// <returns>A string representing this key code</returns>
        public override string ToString()
        {
            return this.key.ToString();
        }

        /// <summary>
        /// Constructs a new arcade key code from implicit casting
        /// </summary>
        /// <param name="key"></param>
        public static implicit operator ArcadeKeyCode(KeyCode key)
        {
            return new ArcadeKeyCode(key);
        }
    }

    /// <summary>
    /// Represents a single player's keys. If you get confused
    /// about where the buttons are, then look at the following image
    /// for a reference:
    /// 
    /// http://www.researcharcade.com/blog/wp-content/uploads/2018/02/26941055_10155997393509254_278121034_n.png
    /// 
    /// </summary>
    [System.Serializable]
    public struct PlayerKeys
    {
        /// <summary>
        /// The start button
        /// </summary>
        public ArcadeKeyCode buttonStart;

        /// <summary>
        /// The exit button
        /// </summary>
        public ArcadeKeyCode buttonExit;

        /// <summary>
        /// The A button
        /// </summary>
        public ArcadeKeyCode A;

        /// <summary>
        /// The B button
        /// </summary>
        public ArcadeKeyCode B;

        /// <summary>
        /// The C button
        /// </summary>
        public ArcadeKeyCode C;

        /// <summary>
        /// The D button
        /// </summary>
        public ArcadeKeyCode D;

        /// <summary>
        /// The E button
        /// </summary>
        public ArcadeKeyCode E;

        /// <summary>
        /// The F button
        /// </summary>
        public ArcadeKeyCode F;

        /// <summary>
        /// The up button (joystick)
        /// </summary>
        public ArcadeKeyCode up;

        /// <summary>
        /// The down button (joystick)
        /// </summary>
        public ArcadeKeyCode down;

        /// <summary>
        /// The left button (joystick)
        /// </summary>
        public ArcadeKeyCode left;

        /// <summary>
        /// The right button (joystick)
        /// </summary>
        public ArcadeKeyCode right;
    }


    /// <summary>
    /// The first player's key mappings
    /// </summary>
    [SerializeField]
    public static readonly PlayerKeys player1 = new PlayerKeys
    {
        buttonExit = KeyCode.Q,
        buttonStart = KeyCode.Return,
        //--
        A = KeyCode.Z,
        B = KeyCode.X,
        C = KeyCode.C,
        D = KeyCode.V,
        E = KeyCode.B,
        F = KeyCode.N,
        //--
        up    = KeyCode.UpArrow,
        down  = KeyCode.DownArrow,
        left  = KeyCode.LeftArrow,
        right = KeyCode.RightArrow
    };


    /// <summary>
    /// The second player's key mappings
    /// </summary>
    [SerializeField]
    public static readonly PlayerKeys player2 = new PlayerKeys
    {
        buttonExit = KeyCode.Q,
        buttonStart = KeyCode.Backspace,
        //--
        A = KeyCode.F,
        B = KeyCode.G,
        C = KeyCode.H,
        D = KeyCode.J,
        E = KeyCode.K,
        F = KeyCode.L,
        //--
        up = KeyCode.W,
        down = KeyCode.S,
        left = KeyCode.A,
        right = KeyCode.D
    };
}
