using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_Drill : AbstractBodyMod
{
    //Takes care mostly of animations. Boulder digging logic is in -> Boulder.cs. The activation / deactivation of the drill
    //occurs by enabling / disabling the drill trigger collider that Boulder.cs checks for.

    Collider2D myCollider;
    private static float offsetX = 5f;
    bool checkingForBoulder = false;


    void Start()
    {
        myCollider = GetComponent<CircleCollider2D>();
        myCollider.enabled = false;
    }


    public override void EnableBodyMod()
    {
        myCollider.enabled = true;
        if (!checkingForBoulder)
        {
            //raise Arm & begin checking if it should be lowered again
            animator.SetBool("raiseArm", true);
            StartCoroutine(DelayedBoulderCheck());
        }
        checkingForBoulder = true;
    }

    IEnumerator DelayedBoulderCheck()
    {
        yield return new WaitForSeconds(.5f);

        //lower Arm if no boulder in range
        ContactFilter2D contactFilter = new ContactFilter2D();
        Collider2D[] colliderList = new Collider2D [10];
        Physics2D.OverlapCollider(myCollider, contactFilter, colliderList);
        bool hitBoulder = false;

        //check if collider has hit Boulder
        for (int i=0; i< colliderList.Length; i++)
        {
            if (colliderList[i] != null)
            {
                if (colliderList[i].gameObject.tag == "Boulder")
                {
                    hitBoulder = true;
                }
            }
        }
        if (hitBoulder)
        {
            GotoState(BodyModState.ACTIVE);
        } else
        {
            //lower arm & delay again to let lower Arm animation finish
            animator.SetBool("raiseArm", false);
            yield return new WaitForSeconds(.3f);
        }
        checkingForBoulder = false;
    }

    public override void DisableBodyMod()
    {
        myCollider.enabled = false;
        animator.SetBool("raiseArm", false);
        GotoState(BodyModState.INACTIVE);
    }

    public override void EquipBodyMod()
    {
    }

    public override void UnequipBodyMod()
    {
    }
}
