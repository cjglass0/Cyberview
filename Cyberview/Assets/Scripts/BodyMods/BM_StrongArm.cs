using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_StrongArm : AbstractBodyMod
{
    bool holdingBox;
    bool droppingBox;
    GameObject heavyBox;
    GameObject blockColliderHelper;
    private static float offsetX = 5f;
    Vector3 blockOffset = new Vector3(offsetX, 0, 0);

    //------------------------------------------------------ Released Hold Box Button
    public override void DisableBodyMod()
    {
        if (holdingBox && !droppingBox) droppingBox = true;
        if (!holdingBox && droppingBox) droppingBox = false;
    }

    //------------------------------------------------------ Pressed Hold Box Button
    public override void EnableBodyMod()
    {
        if (holdingBox && droppingBox)
        {
                                                            //drop box
            GotoState(BodyModState.INACTIVE);
            holdingBox = false;
            //reset colliders
            blockColliderHelper.SetActive(false);
            //heavyBox.GetComponent<BoxCollider2D>().enabled = true;
            heavyBox.GetComponent<Rigidbody2D>().simulated = true;
            heavyBox = null;
        } else if (!heavyBox && !droppingBox)
        {
                                                            //Pick up box
            //clear heavy box
            heavyBox = null;

            List<GameObject> interactables = new List<GameObject>();
            interactables = owner.GetInteractables();

            //get all interactables in range
            List<GameObject> boxes = new List<GameObject>();

            //sort out HeavyBoxes
            foreach (GameObject go in interactables)
            {
                if (go.tag == "HeavyBlock") boxes.Add(go);
            }

            //get closest HeavyBox
            if (boxes.Count > 1)
            {

                float closestDistance = 10f;
                foreach (GameObject box in boxes)
                {
                    float distance = Vector2.Distance(owner.gameObject.transform.position, box.transform.position);
                    if (distance < closestDistance) heavyBox = box;
                }
                setUpBoxForTracking();

            }
            else if (boxes.Count == 1)
            {
                heavyBox = boxes[0];
                setUpBoxForTracking();
            }
        }  
    }

    private void setUpBoxForTracking ()
    {
        //enable helper collider
        if (blockColliderHelper == null) blockColliderHelper = FindObject(owner.gameObject, "BlockColliderHelper");
        blockColliderHelper.SetActive(true);

        //disable box collider
        //heavyBox.GetComponent<BoxCollider2D>().enabled = false;
        heavyBox.GetComponent<Rigidbody2D>().simulated = false;

        GotoState(BodyModState.ACTIVE);
        holdingBox = true;
    }


    //(usually, we could use: GameObject.Find(name), but that only returns active gameobjects. This also gets inactive ones.)
    //(from: https://answers.unity.com/questions/890636/find-an-inactive-game-object.html )
    public GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    //------------------------------------------------------ Holding Box
    private void Update()
    {
        if (macroState == BodyModState.ACTIVE && heavyBox != null)
        {
            //flip if necessary
            float xVel = owner.gameObject.GetComponent<Rigidbody2D>().velocity.x;
            if (xVel > 0.5f) blockOffset = new Vector3(offsetX, 0, 0);
            else if (xVel < -0.5f) blockOffset = new Vector3(-offsetX, 0, 0);

            //position box
            heavyBox.transform.position = (owner.transform.position + blockOffset);
        }
    }
}
