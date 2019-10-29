using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI armLValue, armRValue, legsValue, healthValue, creditValue, tmpMsg;
    public TMP_Dropdown armLDropdown, armRDropdown, legsDropdown;
    public PlayerManager playerManager;
    public GameManager gameManager;
    public CanvasGroup playerHUD, pauseMenu, playerHitCG, bmMenu;
    public GameObject blackout;
    private float originalPlayerHitCGalpha;
    private bool bmMenuLoaded;


    public void Start()
    {
        originalPlayerHitCGalpha = playerHitCG.alpha;
        playerHitCG.alpha = 0;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (!bmMenuLoaded) {
                LoadBodyModMenu();
            } else{
                BtnExitBMMenu();
            }
        }
    }

    //----------------------------------------------------------- OnClick Methods -------------------------------------------------

    public void BtnPause ()
    {
        gameManager.paused = true;
        Debug.Log("HUD -> Pause");
        Time.timeScale = 0;
        playerHUD.alpha = 0;
        playerHUD.interactable = false;
        playerHUD.gameObject.SetActive(false);
        pauseMenu.alpha = 1;
        pauseMenu.interactable = true;
        pauseMenu.gameObject.SetActive(true);
    }

    public void BtnResume()
    {
        gameManager.paused = false;
        Debug.Log("HUD -> Resume");
        Time.timeScale = 1;
        playerHUD.alpha = 1;
        playerHUD.interactable = true;
        playerHUD.gameObject.SetActive(true);
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
        pauseMenu.gameObject.SetActive(false);
    }

    public void BtnExitBMMenu()
    {
        gameManager.paused = false;
        Debug.Log("HUD -> Exit BM MEnu");
        Time.timeScale = 1;
        playerHUD.alpha = 1;
        playerHUD.interactable = true;
        playerHUD.gameObject.SetActive(true);
        bmMenu.alpha = 0;
        bmMenu.interactable = false;
        bmMenu.gameObject.SetActive(false);

        //Apply selected body mods
        playerManager.SetMod(0, GetDropdownBodyMod(armLDropdown));
        playerManager.SetMod(1, GetDropdownBodyMod(armRDropdown));
        playerManager.SetMod(2, GetDropdownBodyMod(legsDropdown));

        UpdateBodyModsDisplay();
        bmMenuLoaded = false;
    }

    public void BtnRestartLevel()
    {
        gameManager.ReloadLevel();
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

            //set the currently active dropdown (set to "none" if none is active)
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

    private AbstractBodyMod GetDropdownBodyMod(TMP_Dropdown dropdown)
    {
        //check which option is selected in the dropdown menu and return the corresponding reference within the player
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
        creditValue.text = playerManager.GetCredit().ToString();
    }

    public void LoadBodyModMenu()
    {
        if (!bmMenuLoaded)
        {
            gameManager.paused = true;
            Debug.Log("HUD -> Body Mod Menu");
            Time.timeScale = 0;
            playerHUD.alpha = 0;
            playerHUD.interactable = false;
            playerHUD.gameObject.SetActive(false);
            bmMenu.alpha = 1;
            bmMenu.interactable = true;
            bmMenu.gameObject.SetActive(true);

            UpdateBodyModsDropdownOptions(armLDropdown);
            UpdateBodyModsDropdownOptions(armRDropdown);
            UpdateBodyModsDropdownOptions(legsDropdown);

            bmMenuLoaded = true;
        }
    }

    public void SetHealth(int health) { healthValue.text = health.ToString(); }

    public void SetCredit(int credit) { creditValue.text = credit.ToString(); }

    public void ShowTmpMsg (string msg)
    {
        tmpMsg.text = msg;
        tmpMsg.gameObject.SetActive(true);
        StartCoroutine(TmpMsgDelay());
    }

    IEnumerator TmpMsgDelay()
    {
        yield return new WaitForSeconds(5);
        tmpMsg.gameObject.SetActive(false);
    }

    public void PlayerHitFX()
    {
        StartCoroutine(FadeCanvasGroup(playerHitCG, originalPlayerHitCGalpha, 0, 1.5f));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }
    }

    public void BlackOutFX(int delay)
    {
        StartCoroutine(BlackoutDelay(delay));
    }

    IEnumerator BlackoutDelay(int delay)
    {
        blackout.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        blackout.gameObject.SetActive(false);
    }

}
