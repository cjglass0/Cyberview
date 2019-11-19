using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_Grapple : AbstractBodyMod
{
    //spawns projectiles to shoot. Handles placement, shoot delay and direction. It also sets up the projectile (Projectile.cs) properties.

    public float shootDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileLifetime = 10f;
    public float projectileDamage = 0;
    public float projectileSpeed = 8f;
    public GameObject existingProjectile;

    private bool canShoot = true;
    public float distanceFromPlayer = 1.6f;

    // Update is called once per frame
    void Update()
    {
        if(existingProjectile){
            if(existingProjectile.GetComponent<GrappleProjectile>().hitEnemyFlag){
                Destroy(existingProjectile);
                StartCoroutine(ShootDelay());
            }
        }
    }

    private void Shoot()
    {
        Vector2 playerPos = owner.gameObject.transform.position;

        //account for direction the player is facing
        int xPosFactor;
        if (owner.isFacingRight) { xPosFactor = 1; } else { xPosFactor = -1; }
        Vector2 spawnPos = new Vector2(playerPos.x + (distanceFromPlayer * xPosFactor), playerPos.y);
        Vector2 size = new Vector2(.5f, .5f);

        if(existingProjectile == null)
        {
            //spawn bullet
            GameObject projectileObject = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            existingProjectile = projectileObject;
            GrappleProjectile projectile = projectileObject.GetComponent<GrappleProjectile>();
            projectile.owner = owner;

            //setup bullet properties
            projectile.SetupProjectile(projectileLifetime, projectileDamage, projectileSpeed, owner.isFacingRight);
            //delay to avoid shooting once per frame
        } else {
            Debug.Log("BM_Gun -> Shoot() -> No Grapple Hook fired because of a lack of space");
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
        animator.SetBool("raiseArm", false);
    }

    public override void EnableBodyMod()
    {
        if(existingProjectile && existingProjectile.GetComponent<GrappleProjectile>().grappling){
            Destroy(existingProjectile);
            StartCoroutine(ShootDelay());
        }
        else if (canShoot){
             Shoot();
        }
        
        animator.SetBool("raiseArm", true);
    }

    public override void EquipBodyMod()
    {
    }

    public override void UnequipBodyMod()
    {
    }
}
