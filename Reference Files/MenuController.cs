using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////////////// FIELDS /////////////////////////////////////////////////////////////////
    //Make sure to attach these Buttons in the Inspector
    public Button btn_Play, btn_Options, btn_Levels;
    private GameController gc;

    ////////////////////////////////////////////////////////////////////////////////////////// START () ///////////////////////////////////////////////////////////////////
    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        btn_Play.onClick.AddListener(BtnClick_Play);
        btn_Options.onClick.AddListener(BtnClick_Options);
        btn_Levels.onClick.AddListener(BtnClick_Levels);
    }

    public void setGC (GameController inpt)
    {
        gc = inpt;
    }

    /////////////////////////////////////////////////////////////////////////////////////// NAV BUTTON CLICKS //////////////////////////////////////////////////////////////
    void BtnClick_Play()
    {
        Debug.Log("Button clicked: Play");
        gc.LoadScene(GameController.FIRST_LVL);
    }

    void BtnClick_Options()
    {
        Debug.Log("Button clicked: Options");
        gc.LoadScene(GameController.OPTIONS);
    }

    void BtnClick_Levels()
    {
        Debug.Log("Button clicked: Levels");
        gc.LoadScene(GameController.LEVELS);
    }


}