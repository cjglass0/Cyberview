using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    /////////////////////////////////////////////////////////////////////////////////////////// FIELDS /////////////////////////////////////////////////////////////////
    public static CameraScript instance = null;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    public GameObject player;

    ////////////////////////////////////////////////////////////////////////////////////////// AWAKE () ///////////////////////////////////////////////////////////////////
    private void Awake()
    {
        if (instance == null) { instance = this; } else if (instance != this) { Destroy(gameObject); } //Singleton Pattern
    }

    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {
        if (player.activeInHierarchy)
        {
            transform.position = new Vector3(player.transform.position.x, -10, -10);
        } 
    }
}
