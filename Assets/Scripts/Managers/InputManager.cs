using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log(Cacher.cargoManager.cargoZoneObjects.Count);
            Cacher.cargoManager.cargoZoneObjects.Add(Cacher.cargoManager.uldObjects[Cacher.cargoManager.uldObjects.Count - 1]);
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
}
