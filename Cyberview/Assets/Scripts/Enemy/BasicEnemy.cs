using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : AbstractEnemy
{
    public BoxCollider2D leftFloorDetector;
    public BoxCollider2D rightFloorDetector;
    
    public float speed;
    private bool groundCheckDelay = false;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void UpdateMovement()
    {
        //walk
        body2d.velocity = new Vector2(speed, body2d.velocity.y);
        if (speed < 3 && speed > -3) Debug.Log("Basic Enemy -> speed = " + speed);
    }

    public override void SetIsGrounded(bool newGroundedState, string colliderObjectName)
    {
        if (colliderObjectName == "Left Floor Box" || colliderObjectName == "Right Floor Box")
        {
            isGrounded = newGroundedState;
            if (!newGroundedState)
            {
                //
                float xPosCheck;
                float yPosCheck = -3.4f;
                if (colliderObjectName == "Left Floor Box") { xPosCheck = -2.4f; } else { xPosCheck = 2.4f; }
                Vector2 checkPos = new Vector2(gameObject.transform.position.x + xPosCheck, gameObject.transform.position.y + yPosCheck);
                Vector2 size = new Vector2(1f, 1f);

                List<Collider2D> collidersAtCheckLocation = new List<Collider2D>(Physics2D.OverlapBoxAll(checkPos, size, 0));
                for (int i = collidersAtCheckLocation.Count - 1; i >= 0; i--)
                {
                    if (collidersAtCheckLocation[i].gameObject.layer != 8) collidersAtCheckLocation.Remove(collidersAtCheckLocation[i]);
                }

                if (collidersAtCheckLocation.Count == 0) speed = -speed;
                //
            }
        } else if (!groundCheckDelay)
        {
            groundCheckDelay = true;
            StartCoroutine(GroundCheckDelay(newGroundedState));
        }

        //turn around if hitting wall or about to drop off a ledge
    }

    IEnumerator GroundCheckDelay(bool newGroundedState)
    {
        if (newGroundedState) speed = -speed;
        yield return new WaitForSeconds(5f);
        groundCheckDelay = false;
    }
}
