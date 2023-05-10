using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudPanel : MonoBehaviour
{
    public float volume;
    public float weight;
    public string priority;
    public Text volume_txt;
    public Text weight_txt;
    public Text priority_txt;
    public GameObject cloudImage;
    float cloudHeight = 3.5f;


    public void ShowData(CargoInfo cargoInfo, bool active)
    {
        if(Cacher.cargoManager.dragObject == null)
        {
            if (active)
            {
                volume = cargoInfo.volume_water;
                weight = cargoInfo.weight;
                priority = cargoInfo.priority;
                volume_txt.text = ("Wavolumeter: ") + volume.ToString() + ("m©ø");
                weight_txt.text = ("Weight: ") + weight.ToString() + ("kg");
                priority_txt.text = ("Priority: ") + priority.ToString();
                cloudImage.transform.position = Cacher.uiManager.mainCamera.WorldToScreenPoint(cargoInfo.GetComponent<Cargo>().Objectpivot.transform.position + new Vector3(0, cloudHeight, 0));
            }
            cloudImage.SetActive(active);
        }
        else
        {
            cloudImage.SetActive(false);
        }
    } 
}
