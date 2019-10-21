using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractCharacter : MonoBehaviour
{
    //Different solid terrain that this object's foot box is in contact with
    protected int groundContactPoints = 0;
    //groundContactPoints > 0 ? true : false
    public bool isGrounded = false;
    public bool isFacingRight = true;

    //when zero, this dies
    public int health;

    //Keeps track of the character's "inputs"
    //Even enemies can use this, which can be used to guide their AI
    //  while reusing code and keeping them more easily understood conceptually
    public InputState inputState;

    //Convenient access to the rigidbody
    public Rigidbody2D body2d;

    protected void Awake(){
        inputState = GetComponent<InputState>();
    }
}
