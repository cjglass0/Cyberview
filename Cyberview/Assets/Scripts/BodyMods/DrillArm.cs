using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillArm : AbstractBehaviour
{
    public GameObject drillProjectilePrefab;
    private GameObject liveDrillProjectile;
    public float projectileRange = 3;

    // Update is called once per frame
    void Update()
    {
        if(drillProjectilePrefab != null){
            var canFire = inputState.GetButtonValue(inputButtons[0]);
            if(liveDrillProjectile != null){
                //projectile should follow player around
                var facingOffset = projectileRange;
                if(inputState.direction != Directions.Right){
                    facingOffset *= -1;
                    liveDrillProjectile.GetComponent<Drill>().right = false;
                }
                else{
                    liveDrillProjectile.GetComponent<Drill>().right = true;
                }
                liveDrillProjectile.transform.position = new Vector2(transform.position.x + facingOffset, transform.position.y);
                
                //projectile checks collision
                //if the player lets go destroy the drill
                if(!canFire){
                    Destroy(liveDrillProjectile);
                }
            }
            else if(canFire){
                CreateProjectile(transform.position);
            }

        }
    }

    void CreateProjectile(Vector2 pos){
        if(inputState.direction == Directions.Right){
            var clone = Instantiate(drillProjectilePrefab, new Vector2(pos.x + projectileRange, pos.y), Quaternion.identity) as GameObject;
            liveDrillProjectile = clone;
        }
        else{
            var clone = Instantiate(drillProjectilePrefab, new Vector2(pos.x - projectileRange, pos.y), Quaternion.identity) as GameObject;
            var drillScript = clone.GetComponent<Drill>();
            drillScript.right = false;
            liveDrillProjectile = clone;
        }
        
    }
}
