using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public abstract class AbstractEnemy : AbstractCharacter
{
    public int damageToPlayerPerHit;
    protected bool updateMovement = true;
   

    [System.NonSerialized]
    public string objectID;

    public abstract void UpdateMovement();

    protected override void Awake()
    {
        base.Awake();

        objectID = gameObject.scene.name + ", x=" + gameObject.transform.position.x + ", y=" + gameObject.transform.position.y;
        if (PlayerPrefs.HasKey(objectID)) gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (updateMovement) UpdateMovement();

        if (health <= 0)
        {
            EnemyDeathStart();
        }
    }

    //Push Back the Enemy when its hit by a weapon
    public void HitBy(GameObject weapon)
    {
        health--;

        if (updateMovement) StartCoroutine(HitThrowback(weapon, false));
    }

    //Push Back Enemy when it hits the Player
    public void PlayerCollision(GameObject player)
    {
        if (updateMovement) StartCoroutine(HitThrowback(player, true));
    }

    IEnumerator HitThrowback(GameObject weapon, bool playerCollision)
    {
        updateMovement = false;
        body2d.velocity = new Vector2(0,0);
        //use varying force for weapon vs player hits
        if (!playerCollision) { body2d.AddForce(-(weapon.transform.position - gameObject.transform.position).normalized * 2000); }
        else { body2d.AddForce(-(weapon.transform.position - gameObject.transform.position).normalized * 1500); }
        //Debug.Log("Enemy -> HitThrowback");
        yield return new WaitForSeconds(0.5f);
        updateMovement = true;
    }

    //split so that we can add vfx for enemy death. Example: EnemyDeathStart() could start a dying animation. Once the animation ends, EnemyDeathEnd() could be called
    public virtual void EnemyDeathStart()
    {
        EnemyDeathEnd();
    }

    public void EnemyDeathEnd()
    {
        int coinflip = (int)(Random.Range(0, 2));
        LvlManager lvlManager = GameObject.Find("LevelManager").GetComponent<LvlManager>();

        //Spawn Reward
        if (coinflip < 1)
        {
            lvlManager.SpawnRandomReward(gameObject.transform.position);
        }

        //save state
        PlayerPrefs.SetInt(objectID, 1);
        lvlManager.EnemyKilled();

        Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "")
    }

}
