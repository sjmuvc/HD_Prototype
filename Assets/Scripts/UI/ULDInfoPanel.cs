using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ULDInfoPanel : MonoBehaviour
{
    public float loadedCapacity;
    public float loadedWeight;

    public Text loadedCapacity_txt;
    public Text loadedWeight_txt;

    public void AddCargo(CargoInfo cargoInfo)
    {
        loadedCapacity += cargoInfo.volume_water / Cacher.uldManager.currentULD.volume * 100;
        loadedWeight += cargoInfo.weight;
        loadedCapacity_txt.text = ("적재율: ") + Math.Round(loadedCapacity, 2).ToString() + ("%");
        loadedWeight_txt.text = ("총 중량: ") + loadedWeight.ToString() + ("t");
    }

    public void SubCargo(CargoInfo cargoInfo)
    {
        loadedCapacity -= cargoInfo.volume_water / Cacher.uldManager.currentULD.volume * 100;
        loadedWeight -= cargoInfo.weight;
        loadedCapacity_txt.text = ("적재율: ") + Math.Round(loadedCapacity, 2).ToString() + ("%");
        loadedWeight_txt.text = ("총 중량: ") + loadedWeight.ToString() + ("t");
    }

    public void Reset()
    {
        loadedCapacity = 0;
        loadedWeight = 0;
        loadedCapacity_txt.text = ("적재율: ") + loadedCapacity.ToString() + ("%");
        loadedWeight_txt.text = ("총 중량: ") + loadedWeight.ToString() + ("t");
    }
}
