using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public PlayerManager playerManager;


    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("grounded");
        if (collision.gameObject.layer == 8) { 
            playerManager.setIsGrounded(true);
            //Debug.Log("grounded");
        }
            
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            playerManager.setIsGrounded(false);
            //Debug.Log("not grounded");
        }
    }
}
