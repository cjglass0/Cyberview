using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
	float bulletSpeed = 5f;
	Rigidbody2D rigidbody2d;
	private Transform target; //holds the enemies target
	Vector2 moveHere;
    // Start is called before the first frame update
    void Start()
    {
    	rigidbody2d = GetComponent<Rigidbody2D>();
    	target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); 
    	moveHere = (target.transform.position - transform.position).normalized * bulletSpeed;
    	rigidbody2d.velocity = new Vector2(moveHere.x, moveHere.y);
    	Destroy (gameObject, 3f);
        
    }

    void OnTriggerEnter2D (Collider2D col){
    	if (col.gameObject.name.Equals("player")){
    		Destroy (gameObject);
    	}
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
