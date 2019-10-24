using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI armLValue, armRValue, legsValue, healthValue;
    public PlayerManager playerManager;


    public void InitializeHUD ()
    {
        armLValue.text = playerManager.GetArmOneMod().gameObject.name;
        armRValue.text = playerManager.GetArmTwoMod().gameObject.name;
        legsValue.text = playerManager.GetLegsMod().gameObject.name;
        healthValue.text = playerManager.GetHealth().ToString();
    }

    public void SetHealth (int health) { healthValue.text = health.ToString(); }
}
