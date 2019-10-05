using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Advertisements; //TMP

public class PlayerCtrl : MonoBehaviour {

    /////////////////////////////////////////////////////////////////////////////////////////// FIELDS ///////////////////////////////////////////////////////////////////
    [SerializeField] private bool airControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask isGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform groundCheck;                           // A position marking where to check if the player is grounded.

    private float jumpForce = 390f;                          // Amount of force added when the player jumps.
    const float groundCheckRadius = .45f; // Radius of the overlap circle to determine if grounded
    private bool grounded;            // Whether or not the player is grounded.
    private Rigidbody2D rb;
    private bool facingRight = true;  // For determining which way the player is currently facing.

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private GameObject spawnPoint;
    private GameObject deathDetector;
    private GameObject finishLine;

    private LvlController lvlController;
    public GameController gameController;



    /////////////////////////////////////////////////////////////////////////////////////////// TESTING ///////////////////////////////////////////////////////////////////




    /////////////////////////////////////////////////////////////////////////////////////////// AWAKE () ///////////////////////////////////////////////////////////////////
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

    }

    ////////////////////////////////////////////////////////////////////////////////////////// START () ///////////////////////////////////////////////////////////////////
    private void Start()
    {
    }


    ////////////////////////////////////////////////////////////////////////////////////////// UPDATE () ///////////////////////////////////////////////////////////////////
    void Update()
    {
        //RESPAWN
        if (rb.position.y < deathDetector.transform.position.y)
        {
            Spawn(lvlController);
            gameController.DeathOcurred();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////// FIXED UPDATE () /////////////////////////////////////////////////////////////////
    private void FixedUpdate()
    {
        //////////// Check if grounded
        bool wasGrounded = grounded;
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Vector2 pointA = new Vector2(groundCheck.position.x - groundCheckRadius, groundCheck.position.y);
        Vector2 pointB = new Vector2(groundCheck.position.x + groundCheckRadius, groundCheck.position.y);
        Collider2D[] colliders = Physics2D.OverlapAreaAll(pointA, pointB, isGround);//Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, isGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////// CUSTOM METHODS /////////////////////////////////////////////////////////////////
    ///
    ///////////////////////////////////////////////////////////////////////////////// Move () ####################
    public void Move(float move, bool jump)
    {
        //only control the player if grounded or airControl is turned on
        if (grounded || airControl)
        {
            if (Mathf.Abs(rb.velocity.x) < 8) rb.AddForce(new Vector2(move, 0));

            //Debug.Log("added force on x: " + move);
            //Debug.Log("current vel on x: " + rb.velocity.x);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (grounded && jump && !(rb.velocity.y<0) && rb.velocity.y<1)
        {
            // Add a vertical force to the player.
            grounded = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    ///////////////////////////////////////////////////////////////////////////////// Flip () ####################
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    ///////////////////////////////////////////////////////////////////////////////// Spawn () ####################
    public void Spawn(LvlController _lvlCtrl)
    {
        lvlController = _lvlCtrl;
        rb.velocity = new Vector2(0, 0);
        rb.position = spawnPoint.transform.position;
    }

    ////////////////////////////////////////////////////////////////////////////////////// SET METHODS /////////////////////////////////////////////////////////////////
    ///
    ///////////////////////////////////////////////////////////////////////////////// SetSpawnPoint () ####################
    public void SetSpawnPoint(GameObject input)
    {
        spawnPoint = input;
    }

    ///////////////////////////////////////////////////////////////////////////////// SetDeathDetector () ####################
    public void SetDeathDetector(GameObject input)
    {
        deathDetector = input;
    }

    ///////////////////////////////////////////////////////////////////////////////// SetFinishLine () ####################
    public void SetFinishLine(GameObject input)
    {
        finishLine = input;
    }

    ////////////////////////////////////////////////////////////////////////////////////// COLLIDERS /////////////////////////////////////////////////////////////////
    ///
    ///////////////////////////////////////////////////////////////////////////////// OnTriggerStay2D () ####################
    void OnTriggerEnter2D(Collider2D col)
    {
        //Spawn Point Trigger
        if (col.gameObject.tag == "SpawnPoint")
        {
            Debug.Log("PlayerCtrl -> collision with: " + col.gameObject.name);
            lvlController.PlayerEnteredSpawnPoint(col.gameObject);
        }

        //Finish Line Trigger
        if (col.gameObject == finishLine)
        {
            Debug.Log("PlayerCtrl -> collision with: " + col.gameObject.name);
            lvlController.PlayerEnteredFinishLine();
        }
    }

}
