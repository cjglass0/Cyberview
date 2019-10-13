using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : AbstractBehaviour
{
    private Vector3 origScale;

    // Start is called before the first frame update
    void Start()
    {
        origScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        var right = inputState.GetButtonValue(inputButtons[0]);
        var left = inputState.GetButtonValue(inputButtons[1]);

        var standing = GetComponent<PlayerManager>().isGrounded;
        if(!standing){
            return;
        }

        if(right){
            inputState.direction = Directions.Right;
        }
        else if(left){
            inputState.direction = Directions.Left;
        }
        
        transform.localScale = new Vector3((float)inputState.direction* origScale.x, 1* origScale.y, 1* origScale.z);
    }
}
