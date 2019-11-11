using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // To use doors: place door prefab in scene. If you wish to use a key, place the key prefab as well and set it as the
    // DoorKey variable of the door game object.

    // Doors transport the player to the level specified as a string (use the scene name) via the sceneToLoad field.

    // For correct setup, place a door in that level that leads back to the origin level. For example: Door A in level 1 leads
    // to door B in level 2 and door B in level 2 leads back to Door A in level 1. That way the player spawns at the correct position.

    // If you wish to prevent the player from traveling back, simply set the door in the new level (door B in the example) to be
    // permanently locked.

    // PUBLIC
    public string sceneToLoad;
    public DoorKey doorKey;
    public bool isPermanentlyLocked;

    // PRIVATE
    //(for some reason there's a chance the code will execute twice before destroying, that's why there's the bool)
    bool collected = false;
    private bool tmpLocked = false;
    private float tmpUnlockDelay = .5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected && !isPermanentlyLocked && !tmpLocked)
        {
            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
            //load Door's scene if the door is not locked or has been unlocked by collecting the relevant key
            if (doorKey == null || playerManager.HasKey(doorKey))
            {
                collected = true;
                playerManager.gameManager.LoadScene(sceneToLoad);

            }
        }
    }

    //unlock temporarily locked door when the player leaves the door trigger. Use a small delay.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && tmpLocked)
        {
            StartCoroutine(TmpUnlockDelay());
        }
    }

    IEnumerator TmpUnlockDelay()
    {
        yield return new WaitForSeconds(tmpUnlockDelay);
        tmpLocked = false;
    }

    //lock door temporarily if it is being used as a spawn point
    public void setTmpLocked()
    {
        tmpLocked = true;
    }

}
