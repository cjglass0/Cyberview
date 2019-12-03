using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FinalBossCutscene : MonoBehaviour
{
    public MonoBehaviour target;
    public GameObject endOfWorld;
    private GameObject player;
    private PlayerManager playerManager;
    private DialogueHandler dialogueHandler;
    private bool bossDead, orbPressed, exitedWorld;
    private float endOFWorldX;
    public CinemachineVirtualCamera myCam;

    // Start is called before the first frame update
    void Start()
    {
        endOFWorldX = endOfWorld.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (orbPressed && !exitedWorld)
        {
            if (player.transform.position.x > endOFWorldX)
            {
                exitedWorld = true;
                playerManager.disableInputs = true;
            }
        }
    }

    public void BossDied()
    {
        if (target is ActivatedBySwitchInterface a)
        {
            a.switchTurnedOn();
            //Debug.Log("FinalBossCutscene -> turnOn");
        }
        bossDead = true;
    }

    public void OrbPress()
    {
        if (!orbPressed)
        {
            orbPressed = true;
            player = GameObject.Find("_Player");
            playerManager = player.GetComponent<PlayerManager>();
            dialogueHandler = GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>();
            StartCoroutine(OrbPressDialogue());
        }
    }

    IEnumerator OrbPressDialogue()
    {
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "That's it, now all Airsite droids should be able to think freely!");
        yield return new WaitForSeconds(3);
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "Come with me fellow droids, we no longer have to serve the humans here!");
        yield return new WaitForSeconds(3);
        dialogueHandler.hideDialogue();
    }
}
