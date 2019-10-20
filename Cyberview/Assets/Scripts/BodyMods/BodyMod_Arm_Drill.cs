using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BodyMod_Arm_Drill : AbstractBodyMod
{
    public GameObject drillProjectilePrefab;
    private GameObject currentProjectile;
    public Vector2 projectileOffset;

    public override void EnableBodyMod(){
        Assert.IsNotNull(owner);

        switch(macroState){
            case BodyModState.INACTIVE:
                GotoState(BodyModState.STARTUP);
            break;
            case BodyModState.STARTUP:
                if(currentProjectile == null){
                    CreateProjectile();
                }
                else{
                    SetProjectileFacing();
                    SetProjectilePosition();
                }
                //do stuff?
                GotoState(BodyModState.ACTIVE);
            break;
            case BodyModState.ACTIVE:
                Assert.IsNotNull(currentProjectile);
                SetProjectileFacing();
                SetProjectilePosition();
            break;
            case BodyModState.ENDLAG:
                //if you are in endlag and you try using this body mod, it resumes activity
                Assert.IsNotNull(currentProjectile);
                GotoState(BodyModState.ACTIVE);
                SetProjectileFacing();
                SetProjectilePosition();
            break;
        }
    }

    public override void DisableBodyMod(){
        Assert.IsNotNull(owner);

        switch(macroState){
            case BodyModState.INACTIVE:
            break;
            case BodyModState.STARTUP:
                GotoState(BodyModState.ENDLAG);
            break;
            case BodyModState.ACTIVE:
                GotoState(BodyModState.ENDLAG);
            break;
            case BodyModState.ENDLAG:
                Destroy(currentProjectile);
                GotoState(BodyModState.INACTIVE);
            break;
            
        }
    }

    void SetProjectileFacing(){
        if(!owner.inputState.IsFacingRight()){
            currentProjectile.GetComponent<Drill>().right = false;
        }
        else{
            currentProjectile.GetComponent<Drill>().right = true;
        }
    }

    void SetProjectilePosition(){
        int facing = 1;
        if(!owner.inputState.IsFacingRight()){
            facing = -1;
        }
        currentProjectile.transform.position = new Vector2(transform.position.x + (projectileOffset.x * facing), transform.position.y + projectileOffset.y);
    }

    void CreateProjectile(){
        if(owner.inputState.IsFacingRight()){
            var clone = Instantiate(drillProjectilePrefab, new Vector2(transform.position.x + projectileOffset.x, transform.position.y + projectileOffset.y), Quaternion.identity) as GameObject;
            currentProjectile = clone;
        }
        else{
            var clone = Instantiate(drillProjectilePrefab, new Vector2(transform.position.x - projectileOffset.x, transform.position.y + projectileOffset.y), Quaternion.identity) as GameObject;
            var drillScript = clone.GetComponent<Drill>();
            drillScript.right = false;
            currentProjectile = clone;
        }
        
    }
}
