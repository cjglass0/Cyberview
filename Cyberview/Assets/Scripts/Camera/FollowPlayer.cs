using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float innerWidth = 100f;
    public float innerHeight = 50;
    public float cameraSpeedScale = 1500f;
    public Transform playerBody;
    public InputState inputState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerBody.transform.position.x, playerBody.transform.position.y, -10);
        return;


        var playerX = playerBody.transform.position.x;
        var cameraX = transform.position.x;
        var playerY = playerBody.transform.position.y;
        var cameraY = transform.position.y;
        var xAdjust = 0;
        var yAdjust = 0;
        
        if(cameraX + innerWidth < playerX){
            //move camera to the right
            xAdjust = 1;
        }
        else if(cameraX - innerWidth > playerX){
            //move camera to the left
            xAdjust = -1;
        }

        if(cameraY + innerHeight < playerY){
            //move camera up
            yAdjust = 1;
        }
        else if(cameraY - innerHeight > playerY){
            //move camera down
            yAdjust = -1;
        }

        transform.position = transform.position + new Vector3(xAdjust*cameraSpeedScale, yAdjust*cameraSpeedScale,0);
    }
}
