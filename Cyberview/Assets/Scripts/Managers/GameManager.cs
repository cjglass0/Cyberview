using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //// This Script will be used to keep everything together. ////

    public static GameManager instance = null;
    public GameObject player;
    public LvlManager lvlManager;
    private PlayerManager playerManager;

    //// BUILD INDEXES ////
    public readonly static int FIRST_LVL = 0;
    public readonly static int LAST_LVL = 2;
    public readonly static int MENU = 3;
    public readonly static int PAUSE = 4;
    public readonly static int CREDITS = 5;
    public readonly static int _BASE = 6;

    //Scenes
    private int sceneToLoad = FIRST_LVL; //<--- Set first Scene to load (should eventually be set by loading saved progress)
    private Scene curScene;
    private bool sceneCurrentlyLoading = false;

    //booleans
    public bool paused;



    ///////////////////////////////////////////////////////////// AWAKE () //////////////////////////////////////////////////////////

    private void Awake()
    {
        if (instance == null) { instance = this; } else if (instance != this) { Destroy(gameObject); } //Singleton Pattern
    }

    void Start()
    {
        //Set up stuff for Pausing
        Time.timeScale = 1;
        paused = false;

        //Set curScene to be active Scene (_Base)
        curScene = SceneManager.GetActiveScene();

        //Load First Scene
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);

        //Listen for Scene Loads
        SceneManager.sceneLoaded += OnSceneFinishedLoading;

        playerManager = player.GetComponent<PlayerManager>();
    }

    /////////////////////////////////////////////////////////////// OnSceneFinishedLoading () /////////////////////////////////////////
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        int newSceneIdx = scene.buildIndex;
        Debug.Log("Scene Loaded: idx=" + newSceneIdx + ", name=" + scene.name + ", loadMode=" + mode);

        if (newSceneIdx < MENU) // -- Level Loaded --
        {
            //prepare GameObjects in _Base for Gameplay. Ex: Activate player.
            if (!player.activeInHierarchy) player.SetActive(true);
            playerManager.ResetPlayer();
            lvlManager = GameObject.Find("LevelManager").GetComponent<LvlManager>();
            lvlManager.InitLevel(this);
        }
        else if (newSceneIdx == MENU || newSceneIdx == CREDITS) // -- Menu Screen Loaded --
        {
            if (player.activeInHierarchy) player.SetActive(false);
        }
    }



    /////////////////////////////////////////////////////////////////////////////////////////// LoadScene (newSceneToLoad) ####################
    public void LoadScene(int newSceneToLoad)
    {
        SceneManager.UnloadSceneAsync(sceneToLoad);
        sceneToLoad = newSceneToLoad;
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }
}
