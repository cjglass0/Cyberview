using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseController : MonoBehaviour {

    public Button btn_Resume;
    private GameController gc;

    // Use this for initialization
    void Start () {
        btn_Resume.onClick.AddListener(BtnClick_Resume);
        gc = GameObject.Find("_GameController").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void BtnClick_Resume() {
        gc.BtnClick_Resume();
    }
}
