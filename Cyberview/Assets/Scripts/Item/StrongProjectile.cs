using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongProjectile : MonoBehaviour
{
    public GameObject attached;

    void OnCollisionEnter2D(Collision2D target){
        if(target.gameObject.tag == "HeavyBlock"){
            attached = target.gameObject;
        }
    }
}
