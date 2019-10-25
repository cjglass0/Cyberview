using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModPickup : MonoBehaviour
{
    public GameObject bodyModPrefab;
    private AbstractBodyMod bodyMod;

    // Start is called before the first frame update
    void Start()
    {
        bodyMod = bodyModPrefab.GetComponent<AbstractBodyMod>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player") collision.gameObject.GetComponent<PlayerManager>().UnlockBodyMod(bodyMod);
    }
}
