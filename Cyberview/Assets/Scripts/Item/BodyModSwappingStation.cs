using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModSwappingStation : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            GameObject.Find("_HUD").GetComponent<HUD>().ShowTmpMsg("Press TAB to use Body Modding Station");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && Input.GetKeyDown(KeyCode.Tab))
        {
            HUD hud = GameObject.Find("_HUD").GetComponent<HUD>();
            hud.LoadBodyModMenu();
        }
    }
}
