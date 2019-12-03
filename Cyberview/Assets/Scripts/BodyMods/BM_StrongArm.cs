using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_StrongArm : AbstractBodyMod
{
    // Strong arm body mod. Picks up box by manually setting its position to be player Pos + blockOffset. Collisions are handled by
    // enabling the player's blockColliderHelper. Otherwise, the player could push the box into the wall, as setting the position manually
    // overrides in-game physics (and therefore the box collider's). Script also releases hold on the box if stuck on a ledge.

    bool holdingBox;
    bool droppingBox;
    GameObject heavyBox;
    GameObject blockColliderHelper;
    private static float offsetX = 4.3f;
    Vector3 blockOffset = new Vector3(offsetX, 0, 0);
    float originalBodyBoneYPos;
    int framesStuckOnLedge;

    void Start()
    {
        originalBodyBoneYPos = owner.bodyBone.transform.position.y;
    }

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
            heavyBox.transform.parent = null;
            blockColliderHelper.SetActive(false);
            //heavyBox.GetComponent<BoxCollider2D>().enabled = true;
            heavyBox.GetComponent<Rigidbody2D>().simulated = true;
            heavyBox = null;
            owner.GetComponentInChildren<Animator>().SetBool("grab", false);
            owner.strongArmsInUse = false;

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
                if (go.tag == "HeavyBlock" || go.tag == "Orb") boxes.Add(go);
            }

            //get closest HeavyBox
            if (boxes.Count > 1)
            {
                //box detected
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
                //no box detected
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

        owner.DecreaseHealth(energyCostPerTick, false);

        owner.GetComponentInChildren<Animator>().SetBool("grab", true);
        owner.strongArmsInUse = true;

        heavyBox.transform.SetParent(owner.bodyBone.gameObject.transform);

        if (heavyBox.gameObject.tag == "Orb") GameObject.Find("FinalBossCutscene").GetComponent<FinalBossCutscene>().PickedUpOrb();
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
            if (owner.transform.localScale.x > 0) blockOffset = new Vector3(offsetX, 1, 0);
            else if (owner.transform.localScale.x < 0) blockOffset = new Vector3(-offsetX, 1, 0);

            //position box
            heavyBox.transform.position = (owner.bodyBone.transform.position + blockOffset);//(owner.transform.position + blockOffset);

            //decrease health every tick
            if (!tickDelay) StartCoroutine(DecreaseHealthAfterTick());

            //let go of box if stuck on ledge
            if (!owner.isGrounded && Mathf.Abs(owner.gameObject.GetComponent<Rigidbody2D>().velocity.y) < 1f)
            {
                framesStuckOnLedge += 5;
            }
            if (framesStuckOnLedge > 80) EnableBodyMod();
        }
        if (framesStuckOnLedge > 0) framesStuckOnLedge--;
    }

    public override void EquipBodyMod()
    {
    }

    public override void UnequipBodyMod()
    {
    }

    public GameObject GetBox() { return heavyBox; }

    public void ResetArm()
    {
        if (blockColliderHelper == null) blockColliderHelper = FindObject(owner.gameObject, "BlockColliderHelper");

        GotoState(BodyModState.INACTIVE);
        holdingBox = false;
        //reset colliders
        if (heavyBox!=null) heavyBox.transform.parent = null;
        blockColliderHelper.SetActive(false);
        //heavyBox.GetComponent<BoxCollider2D>().enabled = true;
        if (heavyBox != null) heavyBox.GetComponent<Rigidbody2D>().simulated = true;
        heavyBox = null;
        owner.GetComponentInChildren<Animator>().SetBool("grab", false);
        owner.strongArmsInUse = false;
    }
}
