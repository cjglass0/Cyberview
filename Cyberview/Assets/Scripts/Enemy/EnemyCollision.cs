using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    //this class is kind of unnecessary, at least as a standalone
    //see CollisionState.cs for explanation
    public LayerMask collisionLayer;
    public Collider2D collider;
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

    void FixedUpdate(){
        var pos = bottomPosition;
        pos.x += transform.position.x;
        pos.y += transform.position.y;
        
        collider = Physics2D.OverlapCircle(pos, collisionRadius, collisionLayer);
        if(collider){
            collider.gameObject.GetComponent<PlayerManager>().HitByEnemy(gameObject);
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = debugCollisionColor;
        var pos = bottomPosition;
        pos.x += transform.position.x;
        pos.y += transform.position.y;

        Gizmos.DrawWireSphere(pos, collisionRadius);
    }
}
