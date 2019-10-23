using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public abstract class AbstractEnemy : AbstractCharacter
{
    public int damageToPlayerPerHit;
    public abstract void UpdateMovement();

    protected virtual void Update()
    {
        UpdateMovement();

        if (health <= 0)
        {
            EnemyDeathStart();
        }
    }

    public void HitBy(GameObject weapon)
    {
        health--;
    }

    public void EnemyDeathStart()
    {
        EnemyDeathEnd();
    }

    public void EnemyDeathEnd()
    {
        Destroy(gameObject);
    }
}
