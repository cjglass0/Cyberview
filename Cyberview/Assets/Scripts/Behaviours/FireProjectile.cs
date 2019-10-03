using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : AbstractBehaviour
{
    public float shootDelay = 0.5f;
    public GameObject projectilePrefab;
    private float timeElapsed = 0f;

    // Update is called once per frame
    void Update()
    {
        if(projectilePrefab != null){
            var canFire = inputState.GetButtonValue(inputButtons[0]);

            if(canFire && timeElapsed > shootDelay){
                CreateProjectile(transform.position);
                timeElapsed = 0;
            }

            timeElapsed += Time.deltaTime;
        }
    }

    void CreateProjectile(Vector2 pos){
        if(inputState.direction == Directions.Right){
            var clone = Instantiate(projectilePrefab, new Vector2(pos.x + 200, pos.y), Quaternion.identity) as GameObject;
        }
        else{
            var clone = Instantiate(projectilePrefab, new Vector2(pos.x - 200, pos.y), Quaternion.identity) as GameObject;
            var missileScript = clone.GetComponent<Missile>();
            missileScript.right = false;
        }
    }
}
