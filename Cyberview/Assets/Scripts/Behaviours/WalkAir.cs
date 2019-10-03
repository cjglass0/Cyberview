using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAir : AbstractBehaviour
{
    public float airSpeedAccel = 50f;
    public float airSpeedMax = 100;
    public float airFriction = 0.99f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(collisionState.standing){
            return;
        }

        var right = inputState.GetButtonValue(inputButtons[0]);
        var left = inputState.GetButtonValue(inputButtons[1]);
        var direction = 1;
        var absVelX = Mathf.Abs(body2d.velocity.x);

        //sets whether accel should be added or subtracted
        if(right){
            direction = 1;
        }
        else if(left){
            direction = -1;
        }


        if(right || left){
            var tmpSpeed = body2d.velocity.x + (airSpeedAccel * direction);
            if(Mathf.Abs(tmpSpeed) > airSpeedMax){
                tmpSpeed = airSpeedMax * direction;
            }

            body2d.velocity = new Vector2(tmpSpeed, body2d.velocity.y);
        }
        else{
            body2d.velocity = new Vector2(body2d.velocity.x * airFriction, body2d.velocity.y);
        }
    }
}
