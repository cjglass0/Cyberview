using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private bool collected;
    [TextArea]
    public string message;
    public AvatarShown avatar;
    public bool displayOnlyOnce;
    private bool displayed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected && (!displayOnlyOnce || !displayed))
        {
            GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>().showDialogue(avatar, message);
            collected = true;
            displayed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>().hideDialogue();
            collected = false;
        }
    }
}
