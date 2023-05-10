using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudPanel : MonoBehaviour
{
    public float volume;
    public float weight;
    public Text volume_txt;
    public Text weight_txt;
    public GameObject cloudImage;
    float cloudHeight = 3.5f;


    public void ShowData(CargoInfo cargoInfo, bool active)
    {
        if (active)
        {
            volume = cargoInfo.volume_water;
            weight = cargoInfo.weight;
            volume_txt.text = ("Wavolumeter: ") + volume.ToString() + ("m©ø");
            weight_txt.text = ("Weight: ") + weight.ToString() + ("kg");
            cloudImage.transform.position = Cacher.uiManager.mainCamera.WorldToScreenPoint(cargoInfo.GetComponent<Cargo>().Objectpivot.transform.position + new Vector3(0, cloudHeight, 0));
        }
        cloudImage.SetActive(true);
    }
}
