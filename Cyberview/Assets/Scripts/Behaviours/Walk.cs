using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : AbstractBehaviour
{
    public float speed = 50f;
    public float runMultiplier = 2f;
    public float friction = 0.9f;
    public bool running;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        running = false;

        var right = inputState.GetButtonValue(inputButtons[0]);
        var left = inputState.GetButtonValue(inputButtons[1]);
        var run = inputState.GetButtonValue(inputButtons[2]);

        if(!collisionState.standing){
            return;
        }
        if(right || left){
            var tmpSpeed = speed;

            if(run && runMultiplier > 0){
                tmpSpeed *= runMultiplier;
                running = true;
            }

            var velX = tmpSpeed * (float)inputState.direction;

            body2d.velocity = new Vector2(velX, body2d.velocity.y);
        }
        else{
            body2d.velocity = new Vector2(body2d.velocity.x * friction, body2d.velocity.y);
        }

        animator.SetFloat("speed", System.Math.Abs(body2d.velocity.x));
    }
}
