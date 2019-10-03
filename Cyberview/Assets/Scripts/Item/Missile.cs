using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Vector2 initialVelocity = new Vector2(100,0);
    private Rigidbody2D body2d;

    private void Awake(){
        body2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        body2d.velocity = new Vector2(initialVelocity.x, initialVelocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
