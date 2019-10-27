using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int sceneToLoad;
    public DoorKey doorKey;
    public bool isPermanentlyLocked;
    //(for some reason there's a chance the code will execute twice before destroying, that's why there's the bool)
    bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected && !isPermanentlyLocked)
        {
            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
            if (doorKey == null)
            {
                collected = true;
                playerManager.gameManager.LoadScene(sceneToLoad);
            } else if (playerManager.HasKey(doorKey))
            {
                collected = true;
                playerManager.gameManager.LoadScene(sceneToLoad);
            }

        }
    }

}
