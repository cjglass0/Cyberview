﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public MonoBehaviour target;
    public bool pressed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9 && 
            collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0.1 &&
            collision.gameObject.transform.position.y > transform.position.y + 2) 
        {
            if(target is ActivatedBySwitchInterface a){
            a.switchTurnedOn();
            pressed = true;
            }
        }   
    }

    public void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.layer == 9 && pressed) 
        {
            if(target is ActivatedBySwitchInterface a){
            a.switchTurnedOff();
            }
        } 
    }
}
