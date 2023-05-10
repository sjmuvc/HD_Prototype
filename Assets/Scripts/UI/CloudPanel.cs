using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CloudPanel : MonoBehaviour
{
    public TextField volume;
    public TextField weight;
    public Image cloudImage;

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
                //Debug.Log("Cloud UI ¶ç¿öÁÖ±â");
            }
        }
    }
    void ShowData(string volume, string weight)
    {

    }
}
