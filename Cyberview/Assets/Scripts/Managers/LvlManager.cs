using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlManager : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject spawnPoint;
    GameManager gameManager;
    int curSpawnPoint = 0;

    void Start()
    {
        spawnPoint = GameObject.Find("SpawnPoint");
    }


    public void InitLevel(GameManager gameManager)
    {
        this.gameManager = gameManager;
        spawnPoint = GameObject.Find("SpawnPoint");
        // set player pos
        gameManager.player.transform.position = spawnPoint.transform.position;
    }
}
