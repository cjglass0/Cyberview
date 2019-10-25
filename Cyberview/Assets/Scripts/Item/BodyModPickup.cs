using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModPickup : MonoBehaviour
{
    //Check if Player picks up Body Mod Collectible. If that is the case, call PlayerManager's UnlockBodyMod(). Also display Message.

    public GameObject bodyModPrefab;
    private AbstractBodyMod bodyMod;

    void Start()
    {
        bodyMod = bodyModPrefab.GetComponent<AbstractBodyMod>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            collision.gameObject.GetComponent<PlayerManager>().UnlockBodyMod(bodyMod);
            GameObject.Find("_HUD").GetComponent<HUD>().ShowTmpMsg("New Body Mod unlocked: " + bodyMod.name + ". Pause to equip.");
            Destroy(gameObject);
        }
    }
}
