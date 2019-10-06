using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private InputState inputState;
    private Walk walkBehaviour;
    private Animator animator;
    private CollisionState collisionState;

    public int health = 5;
    private float invincMax = 3f;
    private float currInvinc = 3f;
    private bool invincible = false;

    private void Awake(){
        inputState = GetComponent<InputState>();
        walkBehaviour = GetComponent<Walk>();
        //animator = GetComponent<Animator>();
        collisionState = GetComponent<CollisionState>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(collisionState.standing){
            //ChangeAnimationState(0);
        }
        if(inputState.absVelX > 0){
            //ChangeAnimationState(1);
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

        if(invincible){
            currInvinc -= Time.deltaTime;
        }

        if(currInvinc < 0){
            invincible = false;
            currInvinc = invincMax;
        }
    }
}
