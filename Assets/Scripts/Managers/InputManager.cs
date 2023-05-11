using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float rotateValue = 90;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (Cacher.cargoManager.uldObjects.Count > 0)
            {
                Cacher.cargoManager.cargoZoneObjects.Add(Cacher.cargoManager.uldObjects[Cacher.cargoManager.uldObjects.Count - 1]);
                Cacher.uiManager.GetComponent<ULDInfoPanel>().SubCargo(Cacher.cargoManager.uldObjects[Cacher.cargoManager.uldObjects.Count - 1].GetComponent<CargoInfo>());
            }
            Cacher.cargoManager.RemoveAtuldObjects();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Cacher.uldManager.currentULD.uld.transform.localEulerAngles = new Vector3(Cacher.uldManager.currentULD.uld.transform.localEulerAngles.x, Cacher.uldManager.currentULD.uld.transform.localEulerAngles.y + 10, Cacher.uldManager.currentULD.uld.transform.localEulerAngles.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Cacher.uldManager.currentULD.uld.transform.localEulerAngles = new Vector3(Cacher.uldManager.currentULD.uld.transform.localEulerAngles.x, Cacher.uldManager.currentULD.uld.transform.localEulerAngles.y - 10, Cacher.uldManager.currentULD.uld.transform.localEulerAngles.z);
        }
    }

    public void InPutRotate(GameObject objectPivot)
    {
        Vector3 tmpVector = new Vector3();

        tmpVector = objectPivot.transform.localEulerAngles;

        if (Input.GetKeyDown(KeyCode.R))
        {
            tmpVector.y += rotateValue;

            //objectPivot.transform.localEulerAngles = new Vector3(objectPivot.transform.localEulerAngles.x, objectPivot.transform.localEulerAngles.y + rotateValue, objectPivot.transform.localEulerAngles.z);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            tmpVector.x += rotateValue;
            //objectPivot.transform.localEulerAngles = new Vector3(objectPivot.transform.localEulerAngles.x + rotateValue, objectPivot.transform.localEulerAngles.y, objectPivot.transform.localEulerAngles.z);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            tmpVector.z += rotateValue;
            //objectPivot.transform.localEulerAngles = new Vector3(objectPivot.transform.localEulerAngles.x, objectPivot.transform.localEulerAngles.y, objectPivot.transform.localEulerAngles.z + rotateValue);
        }

        objectPivot.transform.localEulerAngles = tmpVector;

    }
}
