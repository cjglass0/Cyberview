using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LvlManager : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject spawnPoint, player;
    GameManager gameManager;
    int curSpawnPoint = 0;

    void Start()
    {
        spawnPoint = GameObject.Find("SpawnPoint");
    }


    public void InitLevel(GameManager gameManager)
    {
        this.gameManager = gameManager;
        player = gameManager.player;
        spawnPoint = GameObject.Find("SpawnPoint");
        // set player pos
        player.transform.position = spawnPoint.transform.position;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        //setup camera target
        GameObject.Find("Cinemachine Controller").GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
    }
}
