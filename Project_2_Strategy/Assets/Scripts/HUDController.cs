using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class HUDController : MonoBehaviour
{
    private Player_EnergyController energyController;
    public TMP_Text energyLevel;

    private void Start() {
        energyController = FindObjectOfType<Player_EnergyController>().GetComponent<Player_EnergyController>();
    }

    private void Update() {
        energyLevel.text = energyController.CheckEnergy().ToString();
    }
}
