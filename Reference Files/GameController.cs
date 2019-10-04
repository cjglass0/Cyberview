using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    /////////////////////////////////////////////////////////////////////////////////////////// FIELDS ///////////////////////////////////////////////////////////////////
    //////////// PUBLIC fields
    public GameObject player;
    public PlayerCtrl playerCtrl;
    public GameObject camera;
    public GameObject gc_object;
    public static GameController instance = null;
    public Canvas hud;
    public Button btn_Pause;

    //////////// PRIVATE fields
    private LvlController lvlController;
    private MenuController menuController;
    private GameObject playerPlaceholder;
    private bool paused = false;
    private UnityAds uAds;
    private readonly static int adCountdownL = 3;
    int adCountdown = adCountdownL;

    //// BUILD INDEXES ////
    public readonly static int _BASE = 0;
    public readonly static int MENU = 1;
    public readonly static int OPTIONS = 2;
    public readonly static int LEVELS = 3;
    public readonly static int PAUSE = 4;
    public readonly static int FIRST_LVL = 5;
    public readonly static int LAST_LVL = 7;

    //Scenes 
    private int sceneToLoad = MENU;
    private Scene curScene;
    private bool sceneCurrentlyLoading = false;

    ////////////////////////////////////////////////////////////////////////////////////////// AWAKE () ///////////////////////////////////////////////////////////////////
    private void Awake()
    {
        if (instance == null) { instance = this; } else if (instance != this) { Destroy(gameObject); } //Singleton Pattern
    }

    ////////////////////////////////////////////////////////////////////////////////////////// START () ///////////////////////////////////////////////////////////////////
    void Start()
    {
        //Set up stuff for Pausing
        Time.timeScale = 1;
        paused = false;
        btn_Pause.onClick.AddListener(BtnClick_Pause);

        //Set curScene to be active Scene (_Base)
        curScene = SceneManager.GetActiveScene();

        //Init Objects
        uAds = GetComponent<UnityAds>();
        lvlController = GetComponent<LvlController>();
        player.SetActive(false);
        hud.enabled = false;

        //Load First Scene
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

    ////////////////////////////////////////////////////////////////////////////////////////// UPDATE () ///////////////////////////////////////////////////////////////////
    void Update()
    {
        UpdateScene();
    }

    ////////////////////////////////////////////////////////////////////////////////////// CUSTOM METHODS /////////////////////////////////////////////////////////////////
    ///
    /////////////////////////////////////////////////////////////////////////////////////////// UpdateScene () ####################
    //Checks which Scene is currently loaded and updates curScene Variable if the Scene has changes.
    //Also handles deactivation / activation and placement of Player so that Menu is safe to use.
    private void UpdateScene()
    {
        //check which Scene is actually loaded right now & get Index
        int thisSceneIdx = SceneManager.GetActiveScene().buildIndex;
        if (thisSceneIdx == _BASE)
        {
            try {
                //switch to currently active additive scene
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneToLoad));
                thisSceneIdx = sceneToLoad;
            } catch (System.ArgumentException ae) {}
            
        }

        if (curScene.buildIndex != thisSceneIdx) /////// -- New Scene has completed loading -- ///////
        {
            //(Index not the same -> new Scene has completed loading)
            //(update curScene to match the current Scene)
            curScene = SceneManager.GetActiveScene();
            Debug.Log("GameController -> Scene Load Complete: Scene " + thisSceneIdx);
            sceneCurrentlyLoading = false;

            if (thisSceneIdx >= FIRST_LVL) // -- Level Loaded --
            {
                hud.enabled = true;
                lvlController.InitLvl(FIRST_LVL - thisSceneIdx + 1);
            } else if (thisSceneIdx < PAUSE) // -- Menu Screen Loaded --
            {
                //(init menuController)
                menuController = GameObject.Find("MenuController").GetComponent<MenuController>();
                menuController.setGC(this);
                //(disable hud)
                hud.enabled = false;
                //(disable Level elements)
                lvlController.DisableLevelElements();
            } 
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////////// BtnClick_Pause () ####################
    private void BtnClick_Pause()
    {
        hud.enabled = false;
        paused = true;
        Debug.Log("GameController -> Pause");
        Time.timeScale = 0;
        SceneManager.LoadScene(PAUSE, LoadSceneMode.Additive);
    }

    ////////////////////////////////////////////////////////////////////////////////////// INTERFACE METHODS //////////////////////////////////////////////////////
    /// 
    /////////////////////////////////////////////////////////////////////////////////////////// BtnClick_Resume () ####################
    public void BtnClick_Resume()
    {
        hud.enabled = true;
        paused = false;
        Debug.Log("GameController -> Resume");
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(PAUSE);
    }

    /////////////////////////////////////////////////////////////////////////////////////////// LoadScene (newSceneToLoad) ####################
    public void LoadScene(int newSceneToLoad)
    {
        SceneManager.UnloadSceneAsync(sceneToLoad);
        sceneToLoad = newSceneToLoad;
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

    public void LoadNextLevel()
    {
        if (!sceneCurrentlyLoading)
        {
            Debug.Log("GameController -> LoadNextLevel. sceneToLoad=" + sceneToLoad);
            if (sceneToLoad != LAST_LVL)
            {
                //load next Level
                LoadScene(sceneToLoad + 1);
            }
            else
            {
                //player finished last level -> show credits
                LoadScene(MENU);
            }
            sceneCurrentlyLoading = true;
        } 
    }

    /////////////////////////////////////////////////////////////////////////////////////////// DeathOcurred () ####################
    /////Counts down for Ads
    public void DeathOcurred()
    {
        adCountdown--;
        if (adCountdown < 0)
        {
            adCountdown = adCountdownL;
            uAds.ShowRewardedAd();
        }
    }
}
