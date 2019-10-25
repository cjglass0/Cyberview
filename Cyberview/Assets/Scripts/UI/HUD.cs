using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI armLValue, armRValue, legsValue, healthValue;
    public TMP_Dropdown armLDropdown, armRDropdown, legsDropdown;
    public PlayerManager playerManager;
    public GameManager gameManager;
    public CanvasGroup playerHUD, pauseMenu;

    private void Start()
    {
        armLDropdown.onValueChanged.AddListener(delegate {
            DropdownBodyMod(armLDropdown, 0);
        });
        armRDropdown.onValueChanged.AddListener(delegate {
            DropdownBodyMod(armRDropdown, 1);
        });
        legsDropdown.onValueChanged.AddListener(delegate {
            DropdownBodyMod(legsDropdown, 2);
        });
    }

    public void InitializeHUD ()
    {
        armLValue.text = playerManager.GetArmOneMod().gameObject.name;
        armRValue.text = playerManager.GetArmTwoMod().gameObject.name;
        legsValue.text = playerManager.GetLegsMod().gameObject.name;
        healthValue.text = playerManager.GetHealth().ToString();
    }

    public void SetHealth (int health) { healthValue.text = health.ToString(); }

    //----------------------------------------------------------- OnClick Methods -------------------------------------------------

    public void BtnPause ()
    {
        gameManager.paused = true;
        Debug.Log("HUD -> Pause");
        Time.timeScale = 0;
        playerHUD.alpha = 0;
        playerHUD.interactable = false;
        pauseMenu.alpha = 1;
        pauseMenu.interactable = true;

        armLDropdown.value = getDropdownIndexOfMod(playerManager.GetArmOneMod().gameObject.name);
        armRDropdown.value = getDropdownIndexOfMod(playerManager.GetArmTwoMod().gameObject.name);
        legsDropdown.value = getDropdownIndexOfMod(playerManager.GetLegsMod().gameObject.name);
    }

    public void BtnResume()
    {
        gameManager.paused = false;
        Debug.Log("HUD -> Resume");
        Time.timeScale = 1;
        playerHUD.alpha = 1;
        playerHUD.interactable = true;
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
    }

    public void DropdownBodyMod(TMP_Dropdown dropdown, int whichOne)
    {
        if (dropdown.value == 0) playerManager.SetMod(whichOne, playerManager.bm_Drill);
        if (dropdown.value == 1) playerManager.SetMod(whichOne, playerManager.bm_Gun);
        if (dropdown.value == 2) playerManager.SetMod(whichOne, playerManager.bm_StrongArm);
    }

    private int getDropdownIndexOfMod(string name)
    {
        if (name == "StrongArm") return 2;
        else if (name == "Drill") return 0;
        else if (name == "Gun") return 1;
        return 0;
    }
}
