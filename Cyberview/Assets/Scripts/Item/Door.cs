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
    private bool loadingScene = false;
    private bool justSpawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !isPermanentlyLocked && !justSpawned)
        {
            GameObject.Find("_HUD").GetComponent<HUD>().ShowTmpMsg("Press TAB to open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !isPermanentlyLocked && justSpawned)
        {
            justSpawned = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && Input.GetKeyDown(KeyCode.Tab) && !isPermanentlyLocked && !loadingScene)
        {
            loadingScene = true;

            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
            //load Door's scene if the door is not locked or has been unlocked by collecting the relevant key
            if (doorKey == null || playerManager.HasKey(doorKey))
            {
                playerManager.gameManager.LoadScene(sceneToLoad);
                playerManager.GetPlayerSound().SoundDoor();
            }
        }
    }

    public void SetJustSpawned()
    {
        justSpawned = true;
    }



}
