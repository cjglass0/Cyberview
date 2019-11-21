using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    // Handle all HUD stuff

    public TextMeshProUGUI armLValue, armRValue, legsValue, healthValue, creditValue, tmpMsg;
    public TMP_Dropdown armLDropdown, armRDropdown, legsDropdown;
    public PlayerManager playerManager;
    public GameManager gameManager;
    public CanvasGroup playerHUD, pauseMenu, playerHitCG, bmMenu;
    public RawImage batteryBar;
    public GameObject blackout;
    private float originalPlayerHitCGalpha, origBatterySizeX;
    private AudioSource clickSound;
    private bool bmMenuLoaded, bmFrameDelay;

    private void Awake()
    {
        originalPlayerHitCGalpha = playerHitCG.alpha;
        origBatterySizeX = batteryBar.rectTransform.sizeDelta.x;
        playerHitCG.alpha = 0;
        clickSound = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1) { BtnPause(); } else { BtnResume(); }
        }
        if (Input.GetKeyDown(KeyCode.Tab) && bmMenuLoaded && !bmFrameDelay)
        {
            BtnExitBMMenu();
        }
        if (Input.GetKeyDown(KeyCode.Tab) && bmMenuLoaded && bmFrameDelay) bmFrameDelay = false;
    }

    //----------------------------------------------------------- OnClick Methods -------------------------------------------------

    public void BtnPause ()
    {
        clickSound.Play();
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
        clickSound.Play();
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

    public void BtnClearStorage()
    {
        clickSound.Play();
        Debug.Log("HUD -> DEBUG: Cleared Storage");
        PlayerPrefs.DeleteAll();
    }

    public void BtnExitBMMenu()
    {
        clickSound.Play();
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
        clickSound.Play();
        gameManager.paused = false;
        Debug.Log("HUD -> Restart");
        Time.timeScale = 1;
        playerHUD.alpha = 1;
        playerHUD.interactable = true;
        playerHUD.gameObject.SetActive(true);
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
        pauseMenu.gameObject.SetActive(false);

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
        if (dropdown.options[dropdown.value].text == "Strong Arm") equippedBodyMod = playerManager.bm_StrongArm;
        if (dropdown.options[dropdown.value].text == "Regular Legs") equippedBodyMod = playerManager.bm_Legs;
        if (dropdown.options[dropdown.value].text == "Grapple Hook") equippedBodyMod = playerManager.bm_Grapple;
        return equippedBodyMod;
    }

    //Update In-Game HUD Body Mod Display
    public void UpdateBodyModsDisplay()
    {
        if (playerManager.GetArmOneMod() != null)
        {
            armLValue.text = playerManager.GetArmOneMod().GetComponent<AbstractBodyMod>().name;
        } else { armLValue.text = "-"; }
        if (playerManager.GetArmTwoMod() != null)
        {
            armRValue.text = playerManager.GetArmTwoMod().GetComponent<AbstractBodyMod>().name;
        } else { armRValue.text = "-"; }
        if (playerManager.GetLegsMod() != null)
        {
            legsValue.text = playerManager.GetLegsMod().GetComponent<AbstractBodyMod>().name;
        } else { legsValue.text = "-"; }
    }

    //----------------------------------------------------------- Public Interface -------------------------------------------------

    public void InitializeHUD()
    {
        UpdateBodyModsDisplay();
        SetHealth(playerManager.GetHealth());
        creditValue.text = playerManager.GetCredit().ToString();
    }

    public void LoadBodyModMenu()
    {
        if (!bmMenuLoaded) {
            clickSound.Play();
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
            bmFrameDelay = true;
        }   
    }

    public void SetHealth(int health) { 
        healthValue.text = health.ToString() + "%";
        batteryBar.rectTransform.sizeDelta = new Vector2(mapNumber(health, 0, playerManager.origHealth, 0, origBatterySizeX), 51.85f);
        batteryBar.color = new Color(mapNumber(health, 0, playerManager.origHealth, 0.8f, 0.2f), mapNumber(health, 0, playerManager.origHealth, 0.2f, 0.8f), 0.2f);
    }

    public void SetCredit(int credit) { creditValue.text = credit.ToString(); }

    public void ShowTmpMsg (string msg)
    {
        tmpMsg.text = msg;
        tmpMsg.gameObject.SetActive(true);
        StartCoroutine(TmpMsgDelay());
    }

    public void HideTmpMsg()
    {
        tmpMsg.gameObject.SetActive(false);
    }

    IEnumerator TmpMsgDelay()
    {
        yield return new WaitForSeconds(3);
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

    float mapNumber(float pX, float pA, float pB, float pM, float pN)
    {
        return (pX - pA) / (pB - pA) * (pN - pM) + pM;
    }
}
