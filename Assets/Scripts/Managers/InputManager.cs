using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject dragObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (Cacher.cargoManager.stackNum - 1 >= 0)
            {
                Cacher.cargoManager.stackObjects[Cacher.cargoManager.stackNum - 1].GetComponent<Cargo>().GotoObjectZone();
                Cacher.cargoManager.stackObjects.RemoveAt(Cacher.cargoManager.stackNum - 1);
                Cacher.cargoManager.stackNum--;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Cacher.uldManager.uld.transform.localEulerAngles = new Vector3(Cacher.uldManager.uld.transform.localEulerAngles.x, Cacher.uldManager.uld.transform.localEulerAngles.y + 10, Cacher.uldManager.uld.transform.localEulerAngles.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Cacher.uldManager.uld.transform.localEulerAngles = new Vector3(Cacher.uldManager.uld.transform.localEulerAngles.x, Cacher.uldManager.uld.transform.localEulerAngles.y - 10, Cacher.uldManager.uld.transform.localEulerAngles.z);
        }
    }
}
