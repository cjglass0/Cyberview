using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShovelHelper : MonoBehaviour
{
    public FinalBoss finalBoss;
    public BossAnimationHelper bossAnimationHelper;
    [System.NonSerialized]
    public int damageToPlayerPerHit;

    private void Start()
    {
        damageToPlayerPerHit = finalBoss.damageToPlayerPerHit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            collision.gameObject.GetComponent<PlayerManager>().HitByEnemy(gameObject);
        }
        else if (collision.gameObject.tag == "HeavyBlock")
        {
            bossAnimationHelper.GrabBox(collision.gameObject);
        }
    }
}
