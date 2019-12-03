using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButtonOrb : MonoBehaviour
{
    public FinalBossCutscene finalBossCutscene;
    public MonoBehaviour target;
    private bool pressed = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Orb" && !pressed)
        {
            pressed = true;
            finalBossCutscene.OrbPress();
            //Debug.Log("MyButtonOrb -> Pressed");

            if (target is ActivatedBySwitchInterface a)
            {
                a.switchTurnedOn();
                //Debug.Log("MyButtonOrb -> turnOn");
            }
        }
    }


  
}
