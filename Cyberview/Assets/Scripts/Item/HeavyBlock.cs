using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBlock : MonoBehaviour
{
    public AbstractCharacter attached;
    private bool attachedFlag = false;
    public Vector3 distanceBetween;
    private Rigidbody2D body2d;

    // Start is called before the first frame update
    void Start()
    {
        body2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
        if(attached != null){
            body2d.mass = 10;
            body2d.velocity = new Vector2(attached.GetComponent<Rigidbody2D>().velocity.x, attached.GetComponent<Rigidbody2D>().velocity.y);
        }
        else{
            body2d.mass = 1000;
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        
    }

    void MoveObject(){

    }
}
