using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : AbstractEnemy
{
   public BoxCollider2D leftFloorDetector;
    public BoxCollider2D rightFloorDetector;
     private Animator animator;
    private Transform player; //holds the enemies target
    public float speed;
    public bool checkRight = true;
    [SerializeField]
    GameObject Bullets;
    float bulletRate;
    float nextBullet;

    // Start is called before the first frame update


    void Start()
    {
    	player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); 
        animator = GetComponent<Animator>();
        bulletRate = 3f;
        nextBullet = Time.time;
    }

    // Update is called once per frame
    protected override void Update()
    {
      //If the player is farther than 15 away and NOT farther than 30 away, follow the player. Else, don't.
        if (Vector2.Distance(transform.position, player.position) > 15 && Vector2.Distance(transform.position, player.position) < 30) {
        	animator.SetBool("walk", true);
        	transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

		if(player.position.x > transform.position.x && !checkRight) //if the target is to the right of enemy and the enemy is not facing right
		        lookAtPlayer(); //enemy is now facing correctly
                if (Vector2.Distance(transform.position, player.position) > 15 && Vector2.Distance(transform.position, player.position) < 30) {
                    //check if player in attacking distance
                    //check if its time to shoot again
                    fireBullet();
                }
		if(player.position.x < transform.position.x && checkRight) //if the target is to the left of enemy and the enemy is not facing left
		        lookAtPlayer();//enemy is now facing correctly
                if (Vector2.Distance(transform.position, player.position) > 15 && Vector2.Distance(transform.position, player.position) < 30) {
                    //check if player in attacking distance
                    //check if its time to shoot again
                   fireBullet();
                 }


		} else {
			animator.SetBool("walk", false);
		}
    }

    void fireBullet() {
    	if (Time.time > nextBullet)  {
    		Instantiate (Bullets, transform.position, Quaternion.identity);
    		nextBullet = Time.time + bulletRate;
    	}
    }

     void lookAtPlayer(){
      Vector3 scale = transform.localScale;
      scale.x *= -1;
      transform.localScale = scale;
      checkRight = !checkRight;
}
    public override void UpdateMovement()
    {
        //TODO: Improve logic to check whether enemy got stuck (for some reason doesn't always work)
        //if (body2d.IsSleeping()) { speed = -speed; Debug.Log("stuck"); }

        //walk
	     if (animator.GetBool("walk")){
	        body2d.velocity = new Vector2(speed, body2d.velocity.y);
	        if (speed < 3 && speed > -3) Debug.Log("Basic Enemy -> speed = " + speed);
	    } 
    }



    public override void SetIsGrounded(bool newGroundedState, string colliderObjectName)
    {
        base.SetIsGrounded(newGroundedState, colliderObjectName);

        //turn around if hitting wall or about to drop off a ledge

        if (colliderObjectName == "Left Floor Box" && !newGroundedState) speed = -speed;
        if (colliderObjectName == "Right Floor Box" && !newGroundedState) speed = -speed;
        if (colliderObjectName == "Left Wall Box" && newGroundedState) speed = -speed;
        if (colliderObjectName == "Right Wall Box" && newGroundedState) speed = -speed;

        //Debug.Log("BasicEnemy -> SetIsGrounded(" + newGroundedState + colliderObjectName);
    }
}
/*
Citation
https://www.youtube.com/watch?v=rhoQd6IAtDo
https://stackoverflow.com/questions/53488507/unity-2d-rotate-the-ai-enemy-to-look-at-player
*/


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TestEnemy : AbstractEnemy
// {
//    public BoxCollider2D leftFloorDetector;
//     public BoxCollider2D rightFloorDetector;
//      private Animator animator;
    
//     public float speed;
//     // Start is called before the first frame update


//     void Start()
//     {
//         animator = GetComponent<Animator>();
//     }

//     // Update is called once per frame
//     protected override void Update()
//     {
//         base.Update();

//         if (Input.GetKeyDown(KeyCode.T)) animator.SetBool("walk", true);
//     }

//     public override void UpdateMovement()
//     {
//         //TODO: Improve logic to check whether enemy got stuck (for some reason doesn't always work)
//         //if (body2d.IsSleeping()) { speed = -speed; Debug.Log("stuck"); }

//         //walk
// 	     if (animator.bool("walk")){
// 	        body2d.velocity = new Vector2(speed, body2d.velocity.y);
// 	        if (speed < 3 && speed > -3) Debug.Log("Basic Enemy -> speed = " + speed);
// 	    }
//     }

//     public override void SetIsGrounded(bool newGroundedState, string colliderObjectName)
//     {
//         base.SetIsGrounded(newGroundedState, colliderObjectName);

//         //turn around if hitting wall or about to drop off a ledge

//         if (colliderObjectName == "Left Floor Box" && !newGroundedState) speed = -speed;
//         if (colliderObjectName == "Right Floor Box" && !newGroundedState) speed = -speed;
//         if (colliderObjectName == "Left Wall Box" && newGroundedState) speed = -speed;
//         if (colliderObjectName == "Right Wall Box" && newGroundedState) speed = -speed;

//         //Debug.Log("BasicEnemy -> SetIsGrounded(" + newGroundedState + colliderObjectName);
//     }
// }
