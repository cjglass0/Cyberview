using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
            collision.gameObject.GetComponent<PlayerManager>().gameManager.LoadScene(sceneToLoad);
    }

}
