﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModSwappingStation : MonoBehaviour
{
    private bool chargeUsed = false;

    [System.NonSerialized]
    public string objectID;
    private void Awake()
    {
        objectID = gameObject.scene.name + ", x=" + gameObject.transform.position.x + ", y=" + gameObject.transform.position.y;
        if (PlayerPrefs.HasKey(objectID)) chargeUsed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            GameObject.Find("_HUD").GetComponent<HUD>().ShowTmpMsg("Press TAB to use Body Modding Station");
            //collision.gameObject.GetComponent<PlayerManager>().Recharge(100);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && Input.GetKeyDown(KeyCode.Tab))
        {
            HUD hud = GameObject.Find("_HUD").GetComponent<HUD>();
            hud.LoadBodyModMenu(chargeUsed, this);
        }
    }

    public void ChargeUsed()
    {
        chargeUsed = true;
        PlayerPrefs.SetInt(objectID, 1);
    }

    public void LevelReset()
    {
        chargeUsed = false;
        if (PlayerPrefs.HasKey(objectID)) PlayerPrefs.DeleteKey(objectID);
    }
}
