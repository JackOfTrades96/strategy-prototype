using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EnergyController : MonoBehaviour
{
    [SerializeField] private int energyLevel = 100;
    [SerializeField] private int maxEnergy = 100;

    public int CheckEnergy() {
        return energyLevel;
    }

    public void UpdateMaxEnergy(int changeAmt) {
        maxEnergy += changeAmt;
    }

    public void UpdateEnergy(int changeAmt) {
        energyLevel += changeAmt;
    }

    public void ResetEnergy() {
        energyLevel = maxEnergy;
    }
}
