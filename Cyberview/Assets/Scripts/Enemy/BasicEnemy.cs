using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : AbstractEnemy
{
    public BoxCollider2D leftFloorDetector;
    public BoxCollider2D rightFloorDetector;
    
    public float speed;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void UpdateMovement()
    {
        //TODO: Improve logic to check whether enemy got stuck (for some reason doesn't always work)
        //if (body2d.IsSleeping()) { speed = -speed; Debug.Log("stuck"); }

        //walk
        body2d.velocity = new Vector2(speed, body2d.velocity.y);
        if (speed < 3 && speed > -3) Debug.Log("Basic Enemy -> speed = " + speed);
    }

    public override void SetIsGrounded(bool newGroundedState, string colliderObjectName)
    {
        base.SetIsGrounded(newGroundedState, colliderObjectName);

        //turn around if hitting wall or about to drop off a ledge

        if (colliderObjectName == "Left Floor Box" && !newGroundedState) speed = -speed;
        if (colliderObjectName == "Right Floor Box" && !newGroundedState) speed = -speed;
        if (colliderObjectName == "Left Wall Box" && newGroundedState) speed = -speed;
        if (colliderObjectName == "Right Wall Box" && newGroundedState) speed = -speed;

        //Debug.Log("BasicEnemy -> SetIsGrounded(" + newGroundedState + colliderObjectName);
    }
}
