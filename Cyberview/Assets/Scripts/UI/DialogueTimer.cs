using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTimer : MonoBehaviour
{
    private bool collected;
    [TextArea]
    public string message;
    public int showDelay, hideDelay;
    public AvatarShown avatar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected)
        {
            
            collected = true;
            StartCoroutine(MyDelay());
        }
    }

    IEnumerator MyDelay()
    {
        yield return new WaitForSeconds(showDelay);
        GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>().showDialogue(avatar, message);
        yield return new WaitForSeconds(hideDelay);
        GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>().hideDialogue();
    }
}
