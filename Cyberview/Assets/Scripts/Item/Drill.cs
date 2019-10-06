using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public float zRotation = 0;
    public float rotationIncrement = -1;
    public bool right = true;
    // Start is called before the first frame update
    void Start()
    {
        if(!right){
            rotationIncrement *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,0,zRotation);
        zRotation += rotationIncrement;
    }
}
