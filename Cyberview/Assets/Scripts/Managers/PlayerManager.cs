using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : AbstractCharacter
{
    private Walk walkBehaviour;
    private Animator animator;

    private float invincMax = 3f;
    public float currInvinc = 3f;
    private bool invincible = false;

    void Awake(){
        base.Awake();
        walkBehaviour = GetComponent<Walk>();
        //animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(groundContactPoints < 0){
            Debug.LogWarning("WARNING!  Player groundContactPoints negative!");
            groundContactPoints = 0;
        }
        if(groundContactPoints == 0){
            isGrounded = false;
        }
        else{
            isGrounded = true;
        }

        if(invincible){
            currInvinc -= Time.deltaTime;
        }

        if(currInvinc < 0){
            invincible = false;
            currInvinc = invincMax;
        }


        if(health <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //animator.speed = walkBehaviour.running ? walkBehaviour.runMultiplier : 1;
        
    }

    void ChangeAnimationState(int value){
        animator.SetInteger("AnimState", value);
    }

    public void HitByEnemy(GameObject enemy){
        if(!invincible){
            health--;
            invincible = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == 8){
            groundContactPoints++;
        }
        else if(other.gameObject.layer == 12){
            HitByEnemy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.layer == 12){
            HitByEnemy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.layer == 8){
            groundContactPoints--;
        }
    }

}
