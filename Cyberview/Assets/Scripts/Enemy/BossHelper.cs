using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHelper : MonoBehaviour
{
    public FinalBoss finalBoss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            finalBoss.HitPlayer();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            finalBoss.HitPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            finalBoss.PlayerLeft();
        }
    }
}
