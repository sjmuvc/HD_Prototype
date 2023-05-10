using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudPanel : MonoBehaviour
{
    public Text volume;
    public Text weight;
    public GameObject cloudImage;

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
            }
        }
    }
    void ShowData(CargoInfo)
    {
        volume =
        weight
    }
}
