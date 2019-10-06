using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Vector2 initialVelocity = new Vector2(100,0);
    private Rigidbody2D body2d;
    private float currLife = 0f;
    public float lifetime = 10f;
    public int damage = 1;
    public bool right = true;

    private void Awake(){
        body2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(right){
            body2d.velocity = new Vector2(initialVelocity.x, initialVelocity.y);
        }
        else{
            body2d.velocity = new Vector2(-initialVelocity.x, initialVelocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currLife += Time.deltaTime;
        if(lifetime < currLife){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D target){
        if(target.gameObject.layer == 12){
            //do something to the enemy
            var script = target.gameObject.GetComponent<BasicEnemy>();
            if(script != null){
                script.HitBy(gameObject);
            }
        }
        Destroy(gameObject);
    }
}
