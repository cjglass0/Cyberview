using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : AbstractCharacter
{
    public HitBox leftFloorDetector;
    public HitBox rightFloorDetector;
    
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        var body2d = GetComponent<Rigidbody2D>();
        body2d.velocity = new Vector2(speed,0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
        var body2d = GetComponent<Rigidbody2D>();
        //if hitting something that stops movement (like a wall) then the object will turn around
        if(Mathf.Abs(body2d.velocity.x) < 1){
            isFacingRight = !isFacingRight;
        }

        if(isFacingRight && !rightFloorDetector.touching){
            isFacingRight = false;
        }
        else if(!isFacingRight && !leftFloorDetector.touching){
            isFacingRight = true;
        }

        if(isFacingRight){
            body2d.velocity = new Vector2(speed,0);
        }
        else{
            body2d.velocity = new Vector2(-speed,0);
        }
        
        if(health <= 0){
            Destroy(gameObject);
        }
    }

    public void HitBy(GameObject projectile){
        health--;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == 8){
            groundContactPoints++;
        }
    }

    void OnTriggerStay2D(Collider2D other){
        
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.layer == 8){
            groundContactPoints--;
        }
    }

}
