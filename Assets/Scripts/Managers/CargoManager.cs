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
    int cargoIndex = 0;
    int remainCargoIndex;
    int currentGenerateCargo = 0;

    private void Awake()
    {
        cargoZone = GameObject.Find("CargoZone");
    }

    public void GenerateCargo(int cargosQuantity)
    {
        // spawnRate��ŭ ���� ����
        for (int i = 0; i < cargos.Length; i++)
        {
            if (cargos[cargoIndex].GetComponent<CargoInfo>().spawnRate == 0)
            {
                remainCargoIndex = cargoIndex;
            }
            for (int j = 0; j < Mathf.Floor((cargosQuantity * cargos[cargoIndex].GetComponent<CargoInfo>().spawnRate)); j++) 
            {
                Instantiate(cargos[cargoIndex], cargoZone.transform);
                currentGenerateCargo++;    
            }
            cargoIndex++;
        }
        Debug.Log(cargosQuantity);
        Debug.Log(currentGenerateCargo);
        Debug.Log(remainCargoIndex);
        // ������ ������ spawnRaterk 0�� ������Ʈ�� ä��
        for (int i = 0; i < cargosQuantity - currentGenerateCargo; i++) 
        {
            Instantiate(cargos[remainCargoIndex], cargoZone.transform);
        }
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
