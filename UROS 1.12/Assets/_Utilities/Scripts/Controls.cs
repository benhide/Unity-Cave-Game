using UnityEngine;

// Controls class
public class Controls : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    // Catch the input
    public float horizontal;
    public float vertical;
    public bool diggingAttacking;
    public bool dropTNT;
    public bool otherAction;
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool enter;
    public float idleTimer;
    public float idleTime = 30.0f;

    // Static controls instance
    public static Controls instance;

    ///////////////////////End of Variables/////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Awake()
    {
        // Set static instance to this
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the player controls
        diggingAttacking = Input.GetKeyDown(ArcadeKeymap.player1.A.key);
        enter = Input.GetKeyDown(ArcadeKeymap.player1.A.key);
        dropTNT = Input.GetKeyDown(ArcadeKeymap.player1.B.key);
        otherAction = Input.GetKeyDown(ArcadeKeymap.player1.C.key);

        // Set if the up / down / left / right
        up = ArcadeKeymap.player1.up.isDown;
        down = ArcadeKeymap.player1.down.isDown;
        left = ArcadeKeymap.player1.left.isDown;
        right = ArcadeKeymap.player1.right.isDown;

        // Set the horzontal and vertical
        if (up) vertical = 1.0f;
        if (down) vertical = -1.0f;
        if (left) horizontal = -1.0f;
        if (right) horizontal = 1.0f;

        // Catch the horizontal and vertical input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Set if the player is digging
        diggingAttacking = Input.GetKeyDown(KeyCode.Space);
        dropTNT = Input.GetKeyDown(KeyCode.B);
        otherAction = Input.GetKeyDown(KeyCode.V);

        // Set if the up or down arrow has been pressed
        up = Input.GetKeyDown(KeyCode.UpArrow);
        down = Input.GetKeyDown(KeyCode.DownArrow);

        // Catch the horizontal and vertical input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // If the game is idle for 30 seconds exit
        if (!diggingAttacking && !dropTNT && !otherAction && !up && !down && !left && !right) idleTimer += Time.deltaTime;
        else idleTimer = 0.0f;
        if (idleTimer >= idleTime)
        {
            ArcadeGame.exit();
            Debug.Log("GAME IDLE - EXIT CALLED");
        }
    }

    ///////////////////////End of Functions/////////////////////////
}