using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL_0_MineCollapse : AbstractLvlItem
{
    //Hardcoded sequence to handle the mine collapse stuff.

    public GameObject theDudeThatRunsAway, rubble, bouldersToRemove;
    private bool collected;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MineAlreadyCollapsed")){
            theDudeThatRunsAway.SetActive(false);
            bouldersToRemove.SetActive(false);
            rubble.SetActive(true);
            collected = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected)
        {
            collected = true;
            StartCoroutine(MySequence());
        }
    }

    IEnumerator MySequence()
    {
        HUD hud = GameObject.Find("_HUD").GetComponent<HUD>();
        GameObject.Find("DoorToMainArea").GetComponent<Door>().isPermanentlyLocked = true;

        yield return new WaitForSeconds(1);
        GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>().showDialogue(AvatarShown.MINEMAN, "Oh no! The mine is collapsing!! I better get out of here");
        theDudeThatRunsAway.SetActive(false);

        yield return new WaitForSeconds(2);
        GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>().hideDialogue();

        yield return new WaitForSeconds(1);
        hud.BlackOutFX(5);
        bouldersToRemove.SetActive(false);
        rubble.SetActive(true);
        GameObject.Find("_Player").GetComponent<PlayerManager>().health = 15;
        PlayerPrefs.SetInt("PlayerHealth", 15);
        hud.SetHealth(15);
        hud.ShowTmpMsg("ERROR. ERROR. ERROR.");
        hud.PlayerHitFX();

        yield return new WaitForSeconds(1);
        hud.GetComponent<HUD>().PlayerHitFX();

        yield return new WaitForSeconds(4.1f);
        GameObject.Find("DoorToMainArea").GetComponent<Door>().isPermanentlyLocked = false;
        hud.GetComponent<HUD>().ShowTmpMsg("Error: Critical System Damage. Go to Repair Shop immediately.");

        PlayerPrefs.SetInt("MineAlreadyCollapsed", 1);
    }
}
