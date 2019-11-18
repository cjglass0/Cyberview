using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class LvlManager : MonoBehaviour
{
    // Level Manager is placed once per level. GameManager looks for Level Manager on Level Load. It then calls InitLevel, so all initial
    // setting up for a level should be done in InitLevel. Other functionality includes time challenge, spawning rewards (so we can possibly
    // limit type of rewards for certain levels if we want), setting up camera.

    private GameObject defaultSpawnPoint; //used to spawn player if not coming form another level

    GameObject player;
    GameManager gameManager;
    private HUD hud;
    PlayerManager playerManager;

    //for time challenge
    float levelStartTime;
    public int doorKeyTimeChallenge;
    private DoorKey[] doorKeyArray;
    private Door[] doorArray;
    int keysCollected = 0;
    public GameObject[] rewardArray;

    public void InitLevel(GameManager gameManager, string lastSceneName)
    {
        this.gameManager = gameManager;
        player = gameManager.player;
        playerManager = player.GetComponent<PlayerManager>();

        defaultSpawnPoint = GameObject.Find("SpawnPoint");
        hud = GameObject.Find("_HUD").GetComponent<HUD>();
        doorKeyArray = Object.FindObjectsOfType<DoorKey>();
        doorArray = Object.FindObjectsOfType<Door>();

        Debug.Log("DEBUG: LvlManager -> Came from Scene : " + lastSceneName);
        if (lastSceneName == "") Debug.Log("WARNING: LvlManager -> lastScene is null");

        //clear HUD
        hud.HideTmpMsg();

        //figure out which Spawn Point to use
        if (lastSceneName != null)
        {

            //if coming from another level, override default spawn point with the door to that level
            foreach (Door door in doorArray){
                if (door.sceneToLoad == lastSceneName)
                {
                    defaultSpawnPoint = door.gameObject;
                    door.SetJustSpawned();
                }
            }
        }

        Debug.Log("DEBUG: LvlManager -> Using Spawn Point : " + defaultSpawnPoint.name);

        // set player pos
        player.transform.position = defaultSpawnPoint.transform.position;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //playerManager.Recharge(50);
        playerManager.isFacingRight = true;

        //setup camera target
        GameObject.Find("Cinemachine Controller").GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        Debug.Log("LevelManager -> InitLevel");

        levelStartTime = Time.time;

        //Set saved states
    }

    public void CollectedDoorKey()
    {
        keysCollected++;
        if (keysCollected == doorKeyArray.Length && Time.time - levelStartTime < doorKeyTimeChallenge)
        {
            SpawnRandomReward(GameObject.Find("TimeChallengeRewardSP").transform.position);
            hud.ShowTmpMsg("Congratulations, you won the time Challenge!");
        }
            
    }

    public void SpawnRandomReward(Vector2 rewardSpawnPoint)
    {
        int randomIdx = (int)(Random.Range(0, rewardArray.Length));
        //(because random is inclusive for max value)
        if (randomIdx == rewardArray.Length) randomIdx = rewardArray.Length - 1;
        
        Instantiate(rewardArray[randomIdx], rewardSpawnPoint, Quaternion.identity);
    }
}
