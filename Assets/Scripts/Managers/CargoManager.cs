using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    public List<GameObject> stackObjects = new List<GameObject>();
    public int stackNum;

    public void AllFreeze(bool freeze)
    {
        for (int i = 0; i < stackNum; i++)
        {
            if (freeze)
            {
                stackObjects[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                stackObjects[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }
}
