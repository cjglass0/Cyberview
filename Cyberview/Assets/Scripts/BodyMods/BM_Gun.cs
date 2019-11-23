using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_Gun : AbstractBodyMod
{
    //spawns projectiles to shoot. Handles placement, shoot delay and direction. It also sets up the projectile (Projectile.cs) properties.

    public float shootDelay = 0.5f;
    public GameObject projectilePrefab;
    public float projectileLifetime = 10f;
    public float projectileDamage = 1;
    public float projectileSpeed = 10f;

    private bool canShoot = true;
    private float distanceFromPlayer = 3.2f;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Shoot()
    {
        Vector2 playerPos = owner.gameObject.transform.position;

        //account for direction the player is facing
        int xPosFactor;
        if (owner.isFacingRight) { xPosFactor = 1; } else { xPosFactor = -1; }
        Vector2 spawnPos = new Vector2(playerPos.x + (distanceFromPlayer * xPosFactor), playerPos.y);
        Vector2 size = new Vector2(.5f, .5f);

        //only spawn bullets if there's space
        List<Collider2D> collidersAtSpawnLocation = new List<Collider2D>(Physics2D.OverlapBoxAll(spawnPos, size, 0));
        for (int i = collidersAtSpawnLocation.Count - 1; i >= 0; i--)
        {
            if (collidersAtSpawnLocation[i].gameObject.layer != 8) collidersAtSpawnLocation.Remove(collidersAtSpawnLocation[i]);
        }

        if (collidersAtSpawnLocation.Count == 0)
        {
            //spawn bullet
            GameObject projectileObject = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();

            //setup bullet properties
            projectile.SetupProjectile(projectileLifetime, projectileDamage, projectileSpeed, owner.isFacingRight);

            owner.DecreaseHealth(energyCostPerTick, false);

            //delay to avoid shooting once per frame
            StartCoroutine(ShootDelay());
        } else {
            Debug.Log("BM_Gun -> Shoot() -> No Bullet fired because of a lack of space");
        }
    }

    IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    public override void DisableBodyMod()
    {
        if (armSide == ArmSide.ARMTWO) { animator.SetBool("raiseArmL", false); } else { animator.SetBool("raiseArmR", false); }
    }

    public override void EnableBodyMod()
    {
        if (canShoot && !owner.strongArmsInUse) Shoot();
        if (armSide == ArmSide.ARMTWO) { animator.SetBool("raiseArmL", true); } else { animator.SetBool("raiseArmR", true); }
    }

    public override void EquipBodyMod()
    {
    }

    public override void UnequipBodyMod()
    {
    }
}
