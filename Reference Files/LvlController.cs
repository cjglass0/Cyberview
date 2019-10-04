using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlController : MonoBehaviour {
    /////////////////////////////////////////////////////////////////////////////////////////// FIELDS ///////////////////////////////////////////////////////////////////
    //////////// PUBLIC fields
    public GameObject player;
    public PlayerCtrl playerCtrl;
    public GameController gc;
    public static LvlController instance = null;

    //////////// LEVEL Properties
    private int curLvl;
    private GameObject[] spawnPoints;
    private GameObject curSP;
    private GameObject deathDetector;
    private GameObject finshLine;

    //////////////////////////////////////////////////////////////////////////////////////// CONSTRUCTOR //////////////////////////////////////////////////////////////////
    public LvlController(GameObject p, PlayerCtrl pc)
    {
        player = p;
        playerCtrl = pc;
    }

    ////////////////////////////////////////////////////////////////////////////////////////// AWAKE () ///////////////////////////////////////////////////////////////////
    private void Awake()
    {
        if (instance == null) { instance = this; } else if (instance != this) { Destroy(gameObject); } //Singleton Pattern
    }

    ////////////////////////////////////////////////////////////////////////////////////// CUSTOM METHODS /////////////////////////////////////////////////////////////////
    ///
    ///////////////////////////////////////////////////////////////////////////////// FindGO () ####################
    GameObject FindGO(GameObject[] g, string name)
    {
        for (int i = 0; i < g.Length; i++)
        {
            if (g[i].name == name)
                return g[i];
        }
        return null;
    }

    ///////////////////////////////////////////////////////////////////////////////// InitLvl (_lvl) ####################
    public void InitLvl (int _lvl)
    {
        curLvl = _lvl;
        //Get Spawnpoints
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        curSP = FindGO(spawnPoints, "SP_1");
        playerCtrl.SetSpawnPoint(curSP);
        //Get DeathDetector
        deathDetector = GameObject.Find("DeathDetector");
        playerCtrl.SetDeathDetector(deathDetector);
        //Get FinishLine
        finshLine = GameObject.Find("FinishLine");
        playerCtrl.SetFinishLine(finshLine);
        //Spawn Player
        player.SetActive(true);
        playerCtrl.Spawn(this);

        Debug.Log("LvlController -> Level Init Complete: Level " + curLvl);
    }

    ///////////////////////////////////////////////////////////////////////////////// DisableLevelElements () ####################
    public void DisableLevelElements ()
    {
        player.SetActive(false);
    }

    ///////////////////////////////////////////////////////////////////////////////// PlayerEnteredSpawnPoint (sp) ####################
    public void PlayerEnteredSpawnPoint (GameObject sp)
    {
        curSP = FindGO(spawnPoints, sp.name);
        playerCtrl.SetSpawnPoint(curSP);
        Debug.Log("LvlController -> New SP: " + curSP.name);
    }

    ///////////////////////////////////////////////////////////////////////////////// PlayerEnteredFinishLine () ####################
    public void PlayerEnteredFinishLine ()
    {
        gc.LoadNextLevel();
    }
}
