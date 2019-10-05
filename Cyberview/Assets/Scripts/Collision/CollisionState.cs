﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionState : MonoBehaviour
{
    //this class can be repurposed into a general collisionlayer class
    //I just need to change the wording a little bit
    //Since I will still need specific collision state scripts to handle multiple different collisions,
    //  I will just subclass this class
    public LayerMask collisionLayer;
    public bool standing;   //this variable can be renamed to "colliding"
    public Vector2 bottomPosition = Vector2.zero;
    public float collisionRadius = 10f;
    public Color debugCollisionColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //how this works can be modified in each subclass
    void FixedUpdate(){
        var pos = bottomPosition;
        pos.x += transform.position.x;
        pos.y += transform.position.y;
        
        standing = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);
    }

    void OnDrawGizmos(){
        Gizmos.color = debugCollisionColor;
        var pos = bottomPosition;
        pos.x += transform.position.x;
        pos.y += transform.position.y;

        Gizmos.DrawWireSphere(pos, collisionRadius);
    }
}
