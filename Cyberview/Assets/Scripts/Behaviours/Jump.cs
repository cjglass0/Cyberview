using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : AbstractBehaviour
{
    public float jumpSpeed = 10000f;
    public bool standing;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var jump = inputState.GetButtonValue(inputButtons[4]);

        if(jump && collisionState.standing){
            body2d.velocity = new Vector2(body2d.velocity.x, jumpSpeed);
        }

    }
}
