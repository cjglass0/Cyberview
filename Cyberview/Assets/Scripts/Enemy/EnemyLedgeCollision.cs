using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLedgeCollision : MonoBehaviour
{
    //this class is kind of unnecessary, at least as a standalone
    //see CollisionState.cs for explanation
    public LayerMask collisionLayer;
    public Collider2D colliderLeft;
    public Collider2D colliderRight;
    public Vector2 leftBottomPosition = Vector2.zero;
    public Vector2 rightBottomPosition = Vector2.zero;
    public float leftCollisionRadius = 10f;
    public float rightCollisionRadius = 10f;
    
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
        var enemyScript = GetComponent<BasicEnemy>();
        var leftPos = leftBottomPosition;
        var rightPos = rightBottomPosition;

        leftPos.x += transform.position.x;
        leftPos.y += transform.position.y;
        rightPos.x += transform.position.x;
        rightPos.y += transform.position.y;
        
        
        colliderLeft = Physics2D.OverlapCircle(leftPos, leftCollisionRadius, collisionLayer);
        colliderRight = Physics2D.OverlapCircle(rightPos, rightCollisionRadius, collisionLayer);
        if( (enemyScript.isFacingRight && !colliderRight) || (!enemyScript.isFacingRight && !colliderLeft) ){
            enemyScript.isFacingRight = !enemyScript.isFacingRight;
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = debugCollisionColor;
        var leftPos = leftBottomPosition;
        var rightPos = rightBottomPosition;
        leftPos.x += transform.position.x;
        leftPos.y += transform.position.y;
        rightPos.x += transform.position.x;
        rightPos.y += transform.position.y;

        Gizmos.DrawWireSphere(leftPos, leftCollisionRadius);
        Gizmos.DrawWireSphere(rightPos, rightCollisionRadius);
        
    }
}
