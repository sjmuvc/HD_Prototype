using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ULDInfoPanel : MonoBehaviour
{
    public float loadedCapacity;
    public float loadedWeight;

    public Text loadedCapacity_txt;
    public Text loadedWeight_txt;

    public void AddCargo(CargoInfo cargoInfo)
    {
        loadedWeight += cargoInfo.weight;
        loadedWeight_txt.text = ("ÃÑ Áß·®: ") + loadedWeight.ToString() + ("t");
    }

    public void SubCargo(CargoInfo cargoInfo)
    {
        loadedWeight -= cargoInfo.weight;
        loadedWeight_txt.text = ("ÃÑ Áß·®: ") + loadedWeight.ToString() + ("t");
    }

    public void Reset()
    {
        loadedCapacity = 0;
        loadedWeight = 0;


        loadedWeight_txt.text = ("ÃÑ Áß·®: ") + loadedWeight.ToString() + ("t");
    }
}
