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

    //// BUILD INDEXES ////
    public readonly static int _BASE = 0;
    public readonly static int MENU = 1;
    public readonly static int PAUSE = 2;
    public readonly static int LVL_1 = 3;
    public readonly static int LVL_2 = 4;
    public readonly static int TEST = 5;

    //Scenes
    private int sceneToLoad = TEST; //<--- Set first Scene to load (should eventually be set by loading saved progress)
    private Scene curScene;
    private bool sceneCurrentlyLoading = false;

    //booleans
    bool paused;



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
    }

    /////////////////////////////////////////////////////////////// UPDATE () ///////////////////////////////////////////////////////
    void Update()
    {
        UpdateScene();
    }

    //Checks which Scene is currently loaded and updates curScene Variable if the Scene has changes.
    //Maybe we can figure something better out down the line, perhaps there is a way to do it using SceneManager.
    //Also handles deactivation / activation and placement of Player so that Menu is safe to use.
    private void UpdateScene()
    {
        //check which Scene is actually loaded right now & get Index
        int thisSceneIdx = SceneManager.GetActiveScene().buildIndex;
        if (thisSceneIdx == _BASE)
        {
            try
            {
                //switch to currently active additive scene
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneToLoad));
                thisSceneIdx = sceneToLoad;
            }
            catch (System.ArgumentException ae) { }

        }

        if (curScene.buildIndex != thisSceneIdx) /////// -- New Scene has completed loading -- ///////
        {
            //(Index not the same -> new Scene has completed loading)
            //(update curScene to match the current Scene)
            curScene = SceneManager.GetActiveScene();
            Debug.Log("GameController -> Scene Load Complete: Scene " + thisSceneIdx);
            sceneCurrentlyLoading = false;

            if (thisSceneIdx > PAUSE) // -- Level Loaded --
            {
                //prepare GameObjects in _Base for Gameplay. Ex: Activate player.
                lvlManager = GameObject.Find("LevelManager").GetComponent<LvlManager>();
                lvlManager.InitLevel(this);
            }
            else if (thisSceneIdx < PAUSE) // -- Menu Screen Loaded --
            {
                //prepare GameObjects in _Base for Menu. Ex: Deactivate player.
            }
        }
    }
}
