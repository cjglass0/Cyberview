using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_Gun : AbstractBodyMod
{
    public float shootDelay = 0.5f;
    public GameObject projectilePrefab;
    public float projectileLifetime = 10f;
    public float projectileDamage = 1;
    public float projectileSpeed = 10f;

    private bool canShoot = true;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Shoot()
    {
        Vector2 playerPos = owner.gameObject.transform.position;

        //account for direction the player is facing
        int xPosOffset;
        if (owner.isFacingRight) { xPosOffset = 1; } else { xPosOffset = -1; }
        Vector2 spawnPos = new Vector2(playerPos.x + (5 * xPosOffset), playerPos.y);
        Vector2 size = new Vector2(.5f, .5f);

        //only spawn bullets if there's space
        if(Physics2D.OverlapBoxAll(spawnPos, size, 0).Length == 0)
        {
            //spawn bullet
            GameObject projectileObject = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();

            //setup bullet properties
            projectile.SetupProjectile(projectileLifetime, projectileDamage, projectileSpeed, owner.isFacingRight);
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
        animator.SetBool("raiseArm", false);
    }

    public override void EnableBodyMod()
    {
        if (canShoot) Shoot();
        animator.SetBool("raiseArm", true);
    }

    public override void EquipBodyMod()
    {
    }

    public override void UnequipBodyMod()
    {
    }
}
