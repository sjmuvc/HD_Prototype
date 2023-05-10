using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    public Cargo[] cargos;

    public GameObject cargoZone;
    public GameObject dragObject;

    public List<GameObject> cargoZoneObjects = new List<GameObject>();  
    public List<GameObject> uldObjects = new List<GameObject>();
    public int uldObjectsNum;
    int cargoIndex;
    int remainCargoIndex;
    int currentGenerateCargo;

    private void Awake()
    {
        cargoZone = GameObject.Find("CargoZone");
    }

    public void GenerateCargo(int cargosQuantity)
    {
        for (int i = 0; i < cargoZoneObjects.Count; i++)
        {
            Destroy(cargoZoneObjects[i]);
        }
        cargoZoneObjects.Clear();

        cargoIndex = 0;
        remainCargoIndex = 0;
        currentGenerateCargo = 0;

        // spawnRate만큼 갯수 생성
        for (int i = 0; i < cargos.Length; i++)
        {
            if (cargos[cargoIndex].GetComponent<CargoInfo>().spawnRate == 0)
            {
                remainCargoIndex = cargoIndex;
            }
            for (int j = 0; j < Mathf.Floor((cargosQuantity * cargos[cargoIndex].GetComponent<CargoInfo>().spawnRate)); j++) 
            {
                Cargo newCargo = Instantiate(cargos[cargoIndex], cargoZone.transform);
                cargoZoneObjects.Add(newCargo.gameObject);
                currentGenerateCargo++;    
            }
            cargoIndex++;
        }

        // 부족한 갯수는 spawnRaterk 0인 오브젝트로 채움
        for (int i = 0; i < cargosQuantity - currentGenerateCargo; i++) 
        {
            Cargo newCargo =  Instantiate(cargos[remainCargoIndex], cargoZone.transform);
            cargoZoneObjects.Add(newCargo.gameObject);
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
