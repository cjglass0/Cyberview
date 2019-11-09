using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LvlManager : MonoBehaviour
{
    // Level Manager is placed once per level. GameManager looks for Level Manager on Level Load. It then calls InitLevel, so all initial
    // setting up for a level should be done in InitLevel. Other functionality includes time challenge, spawning rewards (so we can possibly
    // limit type of rewards for certain levels if we want), setting up camera.

    GameObject spawnPoint, player;
    GameManager gameManager;
    int curSpawnPoint = 0;
    private HUD hud;
    PlayerManager playerManager;

    //for time challenge
    float levelStartTime;
    public int doorKeyTimeChallenge;
    private DoorKey[] doorKeyArray;
    int keysCollected = 0;
    public GameObject[] rewardArray;

    void Start()
    {
        spawnPoint = GameObject.Find("SpawnPoint");
        hud = GameObject.Find("_HUD").GetComponent<HUD>();
        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        doorKeyArray = Object.FindObjectsOfType<DoorKey>();
    }


    public void InitLevel(GameManager gameManager)
    {
        this.gameManager = gameManager;
        player = gameManager.player;
        playerManager = player.GetComponent<PlayerManager>();
        spawnPoint = GameObject.Find("SpawnPoint");
        // set player pos
        player.transform.position = spawnPoint.transform.position;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        playerManager.Recharge(50);
        playerManager.isFacingRight = true;

        //setup camera target
        GameObject.Find("Cinemachine Controller").GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        Debug.Log("LevelManager -> InitLevel");

        levelStartTime = Time.time;
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
