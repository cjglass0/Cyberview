using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemy : AbstractEnemy
//issue here: how can I do damage to player?
//is the player able to do damage to this enemy?
{
    public BoxCollider2D leftFloorDetector;
    public BoxCollider2D rightFloorDetector;
   
    private Transform player; //holds the enemies target
    public float speed;
    public bool checkRight = true;
    public float explosionDelay = 2f;
    public bool selfDestruct = true;
    
    // Start is called before the first frame update
    void Start()
    {
    	player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); 
        
    }

    // Update is called once per frame
     protected override void Update()
    {
		base.Update();
    //If the player is farther than 15 away and NOT farther than 30 away, follow the player. Else, don't.
        if (Vector2.Distance(transform.position, player.position) > 5 && Vector2.Distance(transform.position, player.position) < 30) {
       
        	transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

		if(player.position.x > transform.position.x && !checkRight) //if the target is to the right of enemy and the enemy is not facing right
		        lookAtPlayer(); //enemy is now facing correctly
               
		if(player.position.x < transform.position.x && checkRight) //if the target is to the left of enemy and the enemy is not facing left
		        lookAtPlayer();//enemy is now facing correctly
          
		}
		if (Vector2.Distance(transform.position, player.position) <= 5) {

			StartCoroutine(ExplosionDelay());
		} 

    }

	IEnumerator ExplosionDelay(){
        yield return new WaitForSeconds(explosionDelay);
        selfDestruct = true;
        Destroy(gameObject);
        selfDestruct = false;
	}
 

    public override void UpdateMovement()
    {

        //walk
        body2d.velocity = new Vector2(speed, body2d.velocity.y);
        if (speed < 3 && speed > -3) Debug.Log("Basic Enemy -> speed = " + speed);
    }


	void lookAtPlayer(){
      Vector3 scale = transform.localScale;
      scale.x *= -1;
      transform.localScale = scale;
      checkRight = !checkRight;
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

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player"){
        	var script = collision.gameObject.GetComponent<PlayerManager>();
            if (script != null && selfDestruct)
            {
                script.HitByEnemy(gameObject);
            }
        }
    }

}
