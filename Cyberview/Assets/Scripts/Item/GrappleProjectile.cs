using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleProjectile : MonoBehaviour
{
    private Rigidbody2D body2d;
    private float lifetime = 10f; //overwritten by BM_Grapple in Setup
    public float damage = 0;
    private float speed = 7f;
    private bool right = true;
    private int rightFactor;
    public bool projectileDone = false;
    public bool hitEnemyFlag = false;

    public GameObject attachedTerrain;
    public PlayerManager owner;
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

        if(attachedTerrain == null){            //Hook is flying
            body2d.velocity = new Vector2(speed*rightFactor, 0);
            if(lifetime < 0){
                Destroy(gameObject);
            }
            //Debug.Log(lifetime);
        }
        else{                                   //Hook has attached
            body2d.velocity = new Vector2(0,0);
            owner.GetComponent<Rigidbody2D>().velocity = playerVel;

            if(right && owner.transform.position.x > transform.position.x - 3){
                projectileDone = true;
            }
            else if(!right && owner.transform.position.x < transform.position.x + 3){
                projectileDone = true;
            }

            if(projectileDone){
                owner.GetComponent<Rigidbody2D>().velocity = new Vector2(0,33);
                Destroy(gameObject);

                //HERE
            }

            int layerMask = 1 << 8;
            Vector2 direction = transform.localPosition - owner.transform.localPosition;
            float maxDistance = direction.magnitude - 2;
            if (maxDistance < 0) maxDistance = 0;

            Debug.Log("Attached");
            if (Physics2D.Raycast(owner.transform.localPosition, direction, maxDistance, layerMask, -Mathf.Infinity, Mathf.Infinity))
            {
                Debug.Log("GrappleProjectile -> Path blocked. Detach.");
                owner.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 33);
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
                //script.HitBy(gameObject);
            }
            hitEnemyFlag = true;
        }
        else if(target.gameObject.layer == 8){
            attachedTerrain = target.gameObject;
            float angle = Mathf.Atan2(transform.position.y-owner.transform.position.y, transform.position.x-owner.transform.position.x);
            playerVel = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            playerVel *= 33;
            grappling = true;
        }
    }
}
