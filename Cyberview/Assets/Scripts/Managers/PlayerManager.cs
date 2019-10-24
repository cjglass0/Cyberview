using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : AbstractCharacter
{
    ///// PUBLIC
    public BM_Gun bm_Gun;
    public BM_Legs bm_Legs;
    public BM_StrongArm bm_StrongArm;
    public BM_Drill bm_Drill;
    public GameManager gameManager;

    //should change this to be some kind of ground movement object
    public float walkSpeed = 15;
    public float friction = 0.9f;
    //should change this to be some kind of air movement object
    public float airSpeedAccel = 50f;
    public float airSpeedMax = 100;
    public float airFriction = 0.99f;

    ///// PRIVATE
    public Animator animator;
    private GameObject playerObject;
    private List <GameObject> interactables;
    private HUD hud;

    private AbstractBodyMod armOneMod, armTwoMod, legsMod;

    //Booleans
    private bool rightPressed, leftPressed, armOnePressed, armTwoPressed, legsPressed, actionPressed, crouchPressed, pausePressed;
    //TODO: possibly remove invincibility if pushing away enemy works?
    private bool invincible = false;
    private bool pushback = false;

    //Vectors
    private Vector3 originalScale;

    //---------------------------------------------------------------- AWAKE -------------------------------------------
    void Awake(){
        base.Awake();
        body2d = GetComponent<Rigidbody2D>();
    }

    //---------------------------------------------------------------- START -------------------------------------------
    void Start()
    {
        if(armOneMod != null){
            armOneMod.SetOwner(this);
        }
        if(armTwoMod != null){
            armTwoMod.SetOwner(this);
        }
        if(legsMod != null){
            legsMod.SetOwner(this);
        }
        playerObject = gameObject;
        originalScale = gameObject.transform.localScale;
        animator = GetComponentInChildren<Animator>();

        //init lists
        interactables = new List<GameObject>();

        armOneMod = bm_Drill;
        armTwoMod = bm_StrongArm;
        legsMod = bm_Legs;

        hud = GameObject.Find("_HUD").GetComponent<HUD>();
        hud.InitializeHUD();
    }

    //---------------------------------------------------------------- UPDATE -------------------------------------------
    void Update()
    {
        //Get all inputs
        InputsUpdate();

        BodyModsUpdate();

        MovementUpdate();

        if(health <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //---------------------------------------------------------------- UPDATE METHODS -------------------------------------------

    private void InputsUpdate()
    {
        rightPressed = inputState.GetButtonValue(Buttons.Right);
        leftPressed = inputState.GetButtonValue(Buttons.Left);
        armOnePressed = inputState.GetButtonValue(Buttons.ArmOne);
        armTwoPressed = inputState.GetButtonValue(Buttons.ArmTwo);
        legsPressed = inputState.GetButtonValue(Buttons.Legs);
        actionPressed = inputState.GetButtonValue(Buttons.Action);
        crouchPressed = inputState.GetButtonValue(Buttons.Crouch);
        pausePressed = inputState.GetButtonValue(Buttons.Pause);
    }

    private void BodyModsUpdate()
    {
        //TODO: if (in state that allows body mod usage) {...}
        if (actionPressed)
        {
            //do attack thing
        }

        //TODO: if (in state that allows body mod usage) {...}
        if (armOnePressed)
        {
            if (armOneMod != null)
            {
                armOneMod.EnableBodyMod();
                //Debug.Log("ArmOne");
            }
        }
        else
        {
            if (armOneMod != null)
            {
                armOneMod.DisableBodyMod();
            }
        }

        if (armTwoPressed)
        {
            if (armTwoMod != null)
            {
                armTwoMod.EnableBodyMod();
                //Debug.Log("ArmTwo");
            }
        }
        else
        {
            if (armTwoMod != null)
            {
                armTwoMod.DisableBodyMod();
            }
        }
        if (legsPressed)
        {
            if (legsMod != null)
            {
                legsMod.EnableBodyMod();
            }
        }
        else
        {
            if (legsMod != null)
            {
                legsMod.DisableBodyMod();
            }
        }
    }

    private void MovementUpdate()
    {
        //horizontal movement, grounded then aerial
        if (isGrounded)
        {
            if ((leftPressed || rightPressed) && !pushback)
            {
                animator.SetBool("run", true);
                if (leftPressed)
                { //(left)
                    body2d.velocity = new Vector2(-walkSpeed * (float)inputState.direction, body2d.velocity.y);
                }
                else
                { //(right)
                    body2d.velocity = new Vector2(walkSpeed * (float)inputState.direction, body2d.velocity.y);
                }
            }
            else
            {
                body2d.velocity = new Vector2(body2d.velocity.x * friction, body2d.velocity.y);
                animator.SetBool("run", false);
            }
        }
        else
        {
            if ((leftPressed || rightPressed) && !pushback)
            {
                int accelMultiplier = 1;
                if (leftPressed)
                {
                    accelMultiplier = -1;
                }
                var tmpSpeed = body2d.velocity.x + (airSpeedAccel * accelMultiplier);
                if (Mathf.Abs(tmpSpeed) > airSpeedMax)
                {
                    tmpSpeed = airSpeedMax * accelMultiplier;
                }
                body2d.velocity = new Vector2(tmpSpeed, body2d.velocity.y);
            }
            else
            {
                body2d.velocity = new Vector2(body2d.velocity.x * airFriction, body2d.velocity.y);
                animator.SetBool("run", false);
            }
        }

        //flip visually
        if (body2d.velocity.x > .5f && !pushback) { playerObject.transform.localScale = originalScale;
            isFacingRight = true; }
        if (body2d.velocity.x < -.5f && !pushback) { playerObject.transform.localScale = 
                new Vector3(-originalScale.x, originalScale.y, originalScale.z); isFacingRight = false; }
    }

    //----------------------------------------------------------------- OTHER METHODS -------------------------------------------

    //------------------------------------------------------------------- Triggers ----------------------------------------------
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == 12){
            HitByEnemy(other.gameObject);
        }

        interactables.Add(other.gameObject);
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.layer == 12){
            HitByEnemy(other.gameObject);
        }

        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        interactables.Remove(other.gameObject);
    }

    //----------------------------------------------------------------- Colliders -------------------------------------------


    //------------------------------------------------------------- PUBLIC INTERFACE ----------------------------------------
    public override void SetIsGrounded(bool newGroundedState, string colliderObjectName)
    {
        base.SetIsGrounded(newGroundedState, colliderObjectName);
        if (isGrounded) animator.SetBool("jump", false);
    }

    public void HitByEnemy(GameObject enemy)
    {
        //decrease player health based on enemy's set damage
        health -= enemy.GetComponent<AbstractEnemy>().damageToPlayerPerHit;
        hud.SetHealth(health);

        //bump away enemy
        enemy.GetComponent<AbstractEnemy>().PlayerCollision(gameObject);
        Debug.Log("PlayerManager -> HitByEnemy:" + enemy.name + ". New Player Health:" + health);

        //bump player
        StartCoroutine(PlayerHitThrowback(enemy));
    }

    IEnumerator PlayerHitThrowback(GameObject weapon)
    {
        pushback = true;
        body2d.velocity = new Vector2(0, 0);
        body2d.AddForce((gameObject.transform.position - weapon.transform.position).normalized * 4000);
        Debug.Log("PlayerManager -> HitThrowback");
        yield return new WaitForSeconds(0.3f);
        pushback = false;
    }

    public List <GameObject> GetInteractables()
    {
        //Debug.Log("PlayerManager -> Interactables n = " + interactables.Count);
        return interactables;
    }

    public void ResetPlayer()
    {
        interactables.Clear();
    }

    public AbstractBodyMod GetArmOneMod () { return armOneMod; }
    public AbstractBodyMod GetArmTwoMod() { return armTwoMod; }
    public AbstractBodyMod GetLegsMod() { return legsMod; }
    public int GetHealth() { return health; }
}
