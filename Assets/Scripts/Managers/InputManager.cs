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
            if (Cacher.uldManager.stackNum - 1 >= 0)
            {
                Cacher.uldManager.stackObjects[Cacher.uldManager.stackNum - 1].GetComponent<Cargo>().GotoObjectZone();
                Cacher.uldManager.stackObjects.RemoveAt(Cacher.uldManager.stackNum - 1);
                Cacher.uldManager.stackNum--;
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
