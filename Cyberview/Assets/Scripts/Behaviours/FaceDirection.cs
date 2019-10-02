using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : AbstractBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var right = inputState.getButtonValue(inputButtons[0]);
        var left = inputState.getButtonValue(inputButtons[1]);

        if(right){
            inputState.direction = Directions.Right;
        }
        else if(left){
            inputState.direction = Directions.Left;
        }
        
        transform.localScale = new Vector3((float)inputState.direction, 1, 1);
    }
}
