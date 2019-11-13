using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleProjectile : MonoBehaviour
{
    private Rigidbody2D body2d;
    private float lifetime = 10f;
    private float damage = 1;
    private float speed = 7f;
    private bool right = true;
    private int rightFactor;
    public bool projectileDone = false;

    public GameObject attachedTerrain;
    public PlayerManager owner;
    float ownerGravityScale;
    public Vector2 playerVel;
    public bool grappling = false;

    private void Awake()
    {
        body2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer> ();
 
        lineRenderer.SetPosition (0, transform.localPosition);
        lineRenderer.SetPosition (1, owner.transform.localPosition);
        lifetime -= Time.deltaTime;
        if(attachedTerrain == null){
            body2d.velocity = new Vector2(speed*rightFactor, 0);
            if(lifetime < 0){
                Destroy(gameObject);
            }
        }
        else{
            body2d.velocity = new Vector2(0,0);
            owner.GetComponent<Rigidbody2D>().velocity = playerVel;

            if(right && owner.transform.position.x > transform.position.x - 3){
                projectileDone = true;
            }
            else if(!right && owner.transform.position.x < transform.position.x + 3){
                projectileDone = true;
            }

            if(projectileDone){
                owner.GetComponent<Rigidbody2D>().gravityScale = ownerGravityScale;
                owner.GetComponent<PlayerManager>().disableInputs = false;
                Destroy(gameObject);
            }
        }
    }

    public void SetupProjectile(float pLifetime, float pDamage, float pSpeed, bool pRight)
    {
        lifetime = pLifetime;
        damage = pDamage;
        speed = pSpeed;
        right = pRight;
        if (!right) { rightFactor = -1; } else { rightFactor = 1; }
        if(!right){
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.layer == 12)
        {
            //do something to the enemy
            var script = target.gameObject.GetComponent<BasicEnemy>();
            if (script != null)
            {
                script.HitBy(gameObject);
            }
            Destroy(gameObject);
        }
        else if(target.gameObject.layer == 8){
            attachedTerrain = target.gameObject;
            ownerGravityScale = owner.GetComponent<Rigidbody2D>().gravityScale;
            float angle = Mathf.Atan2(transform.position.y-owner.transform.position.y, transform.position.x-owner.transform.position.x);
            playerVel = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            playerVel *= 33;
            grappling = true;
        }
    }
}
