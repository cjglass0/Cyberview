using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModSwappingStation : MonoBehaviour
{
    //(for some reason there's a chance the code will execute twice, that's why there's the bool)
    bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected)
        {
            HUD hud = GameObject.Find("_HUD").GetComponent<HUD>();
            hud.LoadBodyModMenu();
            collected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && collected)
        {
            collected = false;
        }
    }
}
