using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    //string name of the layer the hitbox checks for collisions with
    public LayerMask layer;

    //number of objects this object is colliding with
    public int touchPoints;

    public bool touching;

    void Update(){
        if(touchPoints > 0){
            touching = true;
        }
        else{
            touching = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(layer == (layer | (1 << other.gameObject.layer))){
            touchPoints++;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(layer == (layer | (1 << other.gameObject.layer))){
            touchPoints--;
            //Debug.Log("sub");
        }
    }
}
