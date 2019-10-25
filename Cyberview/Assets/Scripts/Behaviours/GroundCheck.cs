using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public AbstractCharacter abstractCharacter;
    public bool groundCheckIsSolid = false;
    public bool isEnemyGroundCheck = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && groundCheckIsSolid) {
            abstractCharacter.SetIsGrounded(true, gameObject.name);
            //Debug.Log("GroundCheck -> grounded");
        }
            
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && groundCheckIsSolid)
        {
            abstractCharacter.SetIsGrounded(false, gameObject.name);
            //Debug.Log("GroundCheck -> not grounded");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && !groundCheckIsSolid)
        {
            abstractCharacter.SetIsGrounded(true, gameObject.name);
        }/*
        if (isEnemyGroundCheck && collision.gameObject.layer == 12)
        {

        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && !groundCheckIsSolid)
        {
            abstractCharacter.SetIsGrounded(false, gameObject.name);
        }
    }
}
