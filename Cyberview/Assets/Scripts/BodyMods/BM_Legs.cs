using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_Legs : AbstractBodyMod
{
    private bool jump;
    private Rigidbody2D rb;
    private PlayerManager playerManager;

    private float jumpSpeed = 35f;
    private float fallMultiplier = 2f; //sets factor of speed at which player falls back down
    private float lowJumpMultiplier = 1.5f; //sets factor of how hard low jumps break while going up (should be >1)(1 = low and high jumps are same)


    void Update()
    {
        //(sloppy) set references if not set yet
        if (rb == null || playerManager == null)
        {
            rb = owner.gameObject.GetComponent<Rigidbody2D>();
            playerManager = owner.gameObject.GetComponent<PlayerManager>();

        } else
        {
            //Jump
            if (jump && playerManager.isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }


            //better jump physics during jump
            if (rb.velocity.y < -.5f)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > .5f && !jump)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    public override void DisableBodyMod()
    {
        jump = false;
    }

    public override void EnableBodyMod()
    {
        jump = true;
    }
}
