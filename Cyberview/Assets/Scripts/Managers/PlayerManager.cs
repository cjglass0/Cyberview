using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : AbstractCharacter
{
    private Walk walkBehaviour;
    private Animator animator;
    private GameObject playerObject;

    private float invincMax = 3f;
    public float currInvinc = 3f;
    private bool invincible = false;

    public AbstractBodyMod armOneMod;
    public AbstractBodyMod armTwoMod;
    public AbstractBodyMod legs;

    //Booleans
    private bool rightPressed, leftPressed, armOnePressed, armTwoPressed, legsPressed, actionPressed, crouchPressed, pausePressed;

    //Vectors
    Vector3 originalScale;

    //should change this to be some kind of ground movement object
    public float walkSpeed = 15;
    public float friction = 0.9f;
    //should change this to be some kind of air movement object
    public float airSpeedAccel = 50f;
    public float airSpeedMax = 100;
    public float airFriction = 0.99f;

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
       if(legs != null){
           legs.SetOwner(this);
       }
       playerObject = gameObject;
       originalScale = gameObject.transform.localScale;
    }

    //---------------------------------------------------------------- UPDATE -------------------------------------------
    void Update()
    {
        //Get all inputs
        InputsUpdate();
    
        if(invincible){
            currInvinc -= Time.deltaTime;
        }

        if(currInvinc < 0){
            invincible = false;
            currInvinc = invincMax;
        }

        //TODO: if (in state that allows body mod usage) {...}
        if(actionPressed){
            //do attack thing
        }

        //TODO: if (in state that allows body mod usage) {...}
        if(armOnePressed){
            if(armOneMod != null){
                armOneMod.EnableBodyMod();
            }
        }
        else{
            if(armOneMod != null){
                armOneMod.DisableBodyMod();
            }
        }

        if(armTwoPressed){
            if(armTwoMod != null){
                armTwoMod.EnableBodyMod();
            }
        }
        else{
            if(armTwoMod != null){
                armTwoMod.DisableBodyMod();
            }
        }
        if(legsPressed){
            if(legs != null){
                legs.EnableBodyMod();
            }
        } else {
            if (legs != null)
            {
                legs.DisableBodyMod();
            }
        }

        MovementUpdate();

        if(health <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //---------------------------------------------------------------- CUSTOM METHODS -------------------------------------------

    void ChangeAnimationState(int value){
        animator.SetInteger("AnimState", value);
    }

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

    private void MovementUpdate()
    {
        //horizontal movement, grounded then aerial
        if (isGrounded)
        {
            if (leftPressed || rightPressed)
            {
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
            }
        }
        else
        {
            if (leftPressed || rightPressed)
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
            }
        }

        //flip visually
        if (body2d.velocity.x > .1f) playerObject.transform.localScale = originalScale;
        if (body2d.velocity.x < -.1f) playerObject.transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }

    //--------------------------------------------- Triggers
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == 12){
            HitByEnemy(other.gameObject);
        }
        /*
        //set strong arm's heavy box
        if (other.gameObject.tag == "HeavyBlock" && armOneMod)
        {

        }*/
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.layer == 12){
            HitByEnemy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other){

        //remove strong arm's heavy box
    }

    //--------------------------------------------- Colliders (solid)


    //------------------------------------------------------------- PUBLIC INTERFACE ----------------------------------------
    public void setIsGrounded(bool newGroundedState)
    {
        isGrounded = newGroundedState;
    }

    public void HitByEnemy(GameObject enemy)
    {
        if (!invincible)
        {
            health--;
            invincible = true;
        }
    }

}
