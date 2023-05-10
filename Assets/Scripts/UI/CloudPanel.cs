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

    /*
    RaycastHit hitLayerMask;
    float mouseRayDistance = 1000;
    private void Update()
    {
        if (Cacher.cargoManager.dragObject == null)
        {
            Ray ray = Cacher.uiManager.mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * mouseRayDistance, Color.red);

            int layerMask = 1 << LayerMask.NameToLayer("Cargo");
            if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
            {
                //ShowData(RaycastHit.co)
            }
        }
    }
    */
    public void ShowData(CargoInfo cargoInfo)
    {
        volume = cargoInfo.volume_water;
        weight = cargoInfo.weight;
        volume_txt.text = ("Wavolumeter: ") + volume.ToString() + ("m©ø");
        weight_txt.text = ("Weight: ") + weight.ToString() + ("kg");
        cloudImage.transform.position = Cacher.uiManager.mainCamera.WorldToScreenPoint(cargoInfo.gameObject.transform.position);    
        cloudImage.SetActive(true);
    }

    public void Close()
    {
        cloudImage.SetActive(false);
    }
}
