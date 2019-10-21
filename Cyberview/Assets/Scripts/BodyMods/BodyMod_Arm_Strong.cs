using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BodyMod_Arm_Strong : AbstractBodyMod
{

    ///  @ CARTER - CAN WE DELETE THIS? I left it just in case we still need any of the code below. RN BM_StrongArm.cs handles the lifting of the box
    ///  










    //generates a prefab with a collider right in front of the player
    //if prefab collides with one of the heavy blocks, sends a signal
    //attach the heavy block to this script

    public GameObject strongProjectilePrefab;
    private GameObject currentProjectile;
    public Vector2 projectileOffset;
    public GameObject attachedBlock;
    public Vector3 distanceBetween;

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
                GotoState(BodyModState.ACTIVE);
            break;
            case BodyModState.ACTIVE:
                Assert.IsNotNull(currentProjectile);
                if(microState == 0 && currentProjectile.GetComponent<StrongProjectile>().attached){
                    attachedBlock = currentProjectile.GetComponent<StrongProjectile>().attached;
                    distanceBetween = attachedBlock.transform.position - transform.position;
                    currentProjectile.GetComponent<StrongProjectile>().attached.GetComponent<HeavyBlock>().distanceBetween = distanceBetween;
                    microState = 1;
                }

                if(attachedBlock && microState == 1){
                    
                }
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
            var clone = Instantiate(strongProjectilePrefab, new Vector2(transform.position.x + projectileOffset.x, transform.position.y + projectileOffset.y), Quaternion.identity) as GameObject;
            currentProjectile = clone;
        }
        else{
            var clone = Instantiate(strongProjectilePrefab, new Vector2(transform.position.x - projectileOffset.x, transform.position.y + projectileOffset.y), Quaternion.identity) as GameObject;
            var strongScript = clone.GetComponent<StrongProjectile>();
            currentProjectile = clone;
        }
        
    }
}
