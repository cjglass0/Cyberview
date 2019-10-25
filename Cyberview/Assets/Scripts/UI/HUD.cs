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

        UpdateBodyModsDropdownOptions(armLDropdown);
        UpdateBodyModsDropdownOptions(armRDropdown);
        UpdateBodyModsDropdownOptions(legsDropdown);
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

        ApplyBodyMods();
        UpdateBodyModsDisplay();
    }

    //----------------------------------------------------------- Dropdown Logic -------------------------------------------------

    private void UpdateBodyModsDropdownOptions(TMP_Dropdown dropdown)
    {
        List<string> availableUpperBodyModsStrings = new List<string>();
        List<string> availableLowerBodyModsStrings = new List<string>();
        List<string> bmStrings = new List<string>();
        List<AbstractBodyMod> availableBodyMods = playerManager.GetUnlockedBodyMods();

        //check which body mods of which type are unlocked
        foreach (AbstractBodyMod abm in availableBodyMods)
        {
            if (abm.bodyModType == BodyModType.UPPERBODY) availableUpperBodyModsStrings.Add(abm.name);
            if (abm.bodyModType == BodyModType.LOWERBODY) availableLowerBodyModsStrings.Add(abm.name);
        }

        //list only applicable types (upper vs lower body)
        if (dropdown.gameObject.name== "ArmLDropdown" || dropdown.gameObject.name == "ArmRDropdown")
        {
            bmStrings = availableUpperBodyModsStrings;
        } else
        {
            bmStrings = availableLowerBodyModsStrings;
        }

        //clear/remove all option item
        dropdown.options.Clear();
        dropdown.options.Add(new TMP_Dropdown.OptionData() { text = "none" });

        int selectedItem = 0;
        int counter = 1;

        //fill the dropdown menu OptionData with all applicable Body Mod Names
        foreach (string c in bmStrings)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = c });

            //set the currently active dropdown
            if (dropdown == armLDropdown) {
                if (playerManager.GetArmOneMod() != null) {
                    if (playerManager.GetArmOneMod().name == c) selectedItem = counter;
                } else
                {
                    selectedItem = 0;
                }
            }
            else if (dropdown == armRDropdown)
            {
                if (playerManager.GetArmTwoMod() != null)
                {
                    if (playerManager.GetArmTwoMod().name == c) selectedItem = counter;
                }
                else
                {
                    selectedItem = 0;
                }
            }
            else if (dropdown == legsDropdown)
            {
                if (playerManager.GetLegsMod() != null)
                {
                    if (playerManager.GetLegsMod().name == c) selectedItem = counter;
                }
                else
                {
                    selectedItem = 0;
                }
            }
            counter++;
        }

        //refresh Visuals by assigning a temporary new value
        dropdown.value = 1;
        dropdown.value = 0;
        //set active body mod as selected on the Dropdown
        dropdown.value = selectedItem;
    }

    private void ApplyBodyMods()
    {
        //L = 0, R = 1, Legs = 2
        playerManager.SetMod(0, GetDropdownBodyMod(armLDropdown));
        playerManager.SetMod(1, GetDropdownBodyMod(armRDropdown));
        playerManager.SetMod(2, GetDropdownBodyMod(legsDropdown));
    }

    private AbstractBodyMod GetDropdownBodyMod(TMP_Dropdown dropdown)
    {
        //check which option is selected int he dropdown menu
        AbstractBodyMod equippedBodyMod = null;
        if (dropdown.options[dropdown.value].text == "Drill") equippedBodyMod = playerManager.bm_Drill;
        if (dropdown.options[dropdown.value].text == "Gun") equippedBodyMod = playerManager.bm_Gun;
        if (dropdown.options[dropdown.value].text == "StrongArm") equippedBodyMod = playerManager.bm_StrongArm;
        if (dropdown.options[dropdown.value].text == "RegularLegs") equippedBodyMod = playerManager.bm_Legs;
        return equippedBodyMod;
    }

    //Update In-Game HUD Body Mod Display
    private void UpdateBodyModsDisplay()
    {
        if (playerManager.GetArmOneMod() != null)
        {
            armLValue.text = playerManager.GetArmOneMod().gameObject.name;
        } else { armLValue.text = "-"; }
        if (playerManager.GetArmTwoMod() != null)
        {
            armRValue.text = playerManager.GetArmTwoMod().gameObject.name;
        } else { armRValue.text = "-"; }
        if (playerManager.GetLegsMod() != null)
        {
            legsValue.text = playerManager.GetLegsMod().gameObject.name;
        } else { legsValue.text = "-"; }
    }

    //----------------------------------------------------------- Public Interface -------------------------------------------------

    public void InitializeHUD()
    {
        UpdateBodyModsDisplay();
        healthValue.text = playerManager.GetHealth().ToString();
    }

    public void SetHealth(int health) { healthValue.text = health.ToString(); }

}
