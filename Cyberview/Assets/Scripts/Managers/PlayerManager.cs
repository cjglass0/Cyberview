using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : AbstractCharacter
{
    private Walk walkBehaviour;
    private Animator animator;

    private float invincMax = 3f;
    public float currInvinc = 3f;
    private bool invincible = false;

    public AbstractBodyMod armOneMod;
    public AbstractBodyMod armTwoMod;
    public AbstractBodyMod legs;

    //should change this to be some kind of ground movement object
    public float walkSpeed = 15;
    public float friction = 0.9f;
    //should change this to be some kind of air movement object
    public float airSpeedAccel = 50f;
    public float airSpeedMax = 100;
    public float airFriction = 0.99f;

    void Awake(){
        base.Awake();
        body2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        //Get all inputs
        var rightPressed = inputState.GetButtonValue(Buttons.Right);
        var leftPressed = inputState.GetButtonValue(Buttons.Left);
        var armOnePressed = inputState.GetButtonValue(Buttons.ArmOne);
        var armTwoPressed = inputState.GetButtonValue(Buttons.ArmTwo);
        var legsPressed = inputState.GetButtonValue(Buttons.Legs);
        var actionPressed = inputState.GetButtonValue(Buttons.Action);
        var crouchPressed = inputState.GetButtonValue(Buttons.Crouch);
        var pausePressed = inputState.GetButtonValue(Buttons.Pause);
    
        if(groundContactPoints < 0){
            Debug.LogWarning("WARNING!  Player groundContactPoints negative!");
            groundContactPoints = 0;
        }
        if(groundContactPoints == 0){
            isGrounded = false;
        }
        else{
            isGrounded = true;
        }

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
            else{
                legs.DisableBodyMod();
            }
        }
        
        //horizontal movement, grounded then aerial
        if(isGrounded){
            if(leftPressed || rightPressed){
                body2d.velocity = new Vector2(walkSpeed * (float)inputState.direction, body2d.velocity.y);
            }
            else{
                body2d.velocity = new Vector2(body2d.velocity.x * friction, body2d.velocity.y);
            }
        }
        else{
            if(leftPressed || rightPressed){
                int accelMultiplier = 1;
                if(leftPressed){
                    accelMultiplier = -1;
                }
                var tmpSpeed = body2d.velocity.x + (airSpeedAccel * accelMultiplier);
                if(Mathf.Abs(tmpSpeed) > airSpeedMax){
                    tmpSpeed = airSpeedMax * accelMultiplier;
                }
                body2d.velocity = new Vector2(tmpSpeed, body2d.velocity.y);
            }
            else{
                body2d.velocity = new Vector2(body2d.velocity.x * airFriction, body2d.velocity.y);
            }
        }

        if(health <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void ChangeAnimationState(int value){
        animator.SetInteger("AnimState", value);
    }

    public void HitByEnemy(GameObject enemy){
        if(!invincible){
            health--;
            invincible = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == 8){
            groundContactPoints++;
        }
        else if(other.gameObject.layer == 12){
            HitByEnemy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.layer == 12){
            HitByEnemy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.layer == 8){
            groundContactPoints--;
        }
    }

}
