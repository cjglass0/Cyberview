using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum AvatarShown
{
    PROGAGONIST,
    HELPERBOT,
    MINEMAN
}


//This class is always present, as it is attached to the Player's HUD (_HUD -> Player HUD -> Dialogue Handler)

public class DialogueHandler : MonoBehaviour
{
    public Texture protagonistAvatar, helperBotAvatar, mineManAvatar;
    public GameObject panel;
    public TextMeshProUGUI text;
    public RawImage avatarRenderer;

    public void Start()
    {

    }

    public void showDialogue(AvatarShown avatarShown, string message)
    {
        panel.SetActive(true);
        text.text = message;
        switch (avatarShown)
        {
            case AvatarShown.PROGAGONIST:
                avatarRenderer.texture = protagonistAvatar;
                break;
            case AvatarShown.HELPERBOT:
                avatarRenderer.texture = helperBotAvatar;
                break;
            case AvatarShown.MINEMAN:
                avatarRenderer.texture = mineManAvatar;
                break;
        }
    }

    public void hideDialogue()
    {
        panel.SetActive(false);
    }

    public string GetCurrentMessage()
    {
        return text.text;
    }
}
