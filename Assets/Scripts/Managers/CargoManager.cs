using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    public Cargo[] cargos;

    public GameObject cargoZone;
    public GameObject dragObject;

    public List<GameObject> uldObjects = new List<GameObject>();
    public int uldObjectsNum;

    private void Awake()
    {
        cargoZone = GameObject.Find("CargoZone");
    }

    public void GenerateCargo(int cargosQuantity)
    {
        // ȭ�� �������ְ� CargoInfo �߰�
        Instantiate(cargos[0], cargoZone.transform);
        Debug.Log(cargosQuantity);
    }

    public void RemoveAtuldObjects()
    {
        if (uldObjectsNum - 1 >= 0)
        {
            uldObjects[uldObjectsNum - 1].GetComponent<Cargo>().GotoObjectZone();
            uldObjects.RemoveAt(uldObjectsNum - 1);
            uldObjectsNum--;
        }
    }

    public void GotoObjectZoneAll()
    {
        for(int i = 0; i < uldObjectsNum; i++)
        {
            uldObjects[i].GetComponent<Cargo>().GotoObjectZone();
        }
        uldObjects.Clear();
        uldObjectsNum = 0;
    }

    public void AllFreeze(bool freeze)
    {
        for (int i = 0; i < uldObjectsNum; i++)
        {
            if (freeze)
            {
                uldObjects[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                uldObjects[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }
}
