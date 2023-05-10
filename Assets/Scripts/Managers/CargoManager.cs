using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    public Cargo[] cargos;

    public GameObject cargoZone;
    public GameObject cargoZonePlane;
    public GameObject dragObject;
    float cargoZoneLength;
    float currentCargoZoneLength;
    GameObject lastCargoZoneObject;

    public List<GameObject> cargoZoneObjects = new List<GameObject>();  
    public List<GameObject> uldObjects = new List<GameObject>();
    public int uldObjectsNum;
    int cargoIndex;
    int remainCargoIndex;
    int currentGenerateCargo;

    private void Awake()
    {
        cargoZone = GameObject.Find("CargoZone");
        cargoZonePlane = GameObject.Find("CargoZonePlane");
        cargoZoneLength = cargoZonePlane.GetComponent<MeshCollider>().bounds.size.x;
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
        currentCargoZoneLength = 0;

        // spawnRate만큼 갯수 생성
        for (int i = 0; i < cargos.Length; i++)
        {
            if (cargos[cargoIndex].GetComponent<CargoInfo>().spawnRate == 0)
            {
                remainCargoIndex = cargoIndex;
            }
            for (int j = 0; j < Mathf.Floor((cargosQuantity * cargos[cargoIndex].GetComponent<CargoInfo>().spawnRate)); j++) 
            {
                Cargo generatedCargo = Instantiate(cargos[cargoIndex], cargoZone.transform);
                lastCargoZoneObject = generatedCargo.gameObject;
                cargoZoneObjects.Add(generatedCargo.gameObject);
                currentGenerateCargo++;
                generatedCargo.GetComponent<Cargo>().GenerateSetting();
                GeneratePositioning(generatedCargo.gameObject);
            }
            cargoIndex++;
        }

        // 부족한 갯수는 spawnRate가 0인 오브젝트로 채움
        for (int i = 0; i < cargosQuantity - currentGenerateCargo; i++) 
        {
            Cargo generatedCargo =  Instantiate(cargos[remainCargoIndex], cargoZone.transform);
            cargoZoneObjects.Add(generatedCargo.gameObject);
            generatedCargo.GetComponent<Cargo>().GenerateSetting();
            GeneratePositioning(generatedCargo.gameObject);
        }
    }

    void GeneratePositioning(GameObject generatedCargo) // 나중에 Cargo로 변경하는게 좋을듯함
    {
        //generatedCargo.GetComponent<Cargo>().Objectpivot.transform.localPosition = Vector3.zero;
        if (currentCargoZoneLength + generatedCargo.GetComponent<MeshCollider>().bounds.size.x < cargoZoneLength)
        {
            generatedCargo.GetComponent<Cargo>().Objectpivot.transform.localPosition = new Vector3(currentCargoZoneLength + generatedCargo.GetComponent<MeshCollider>().bounds.size.x / 2, generatedCargo.GetComponent<MeshCollider>().bounds.size.y / 2, 0);
            currentCargoZoneLength += generatedCargo.GetComponent<MeshCollider>().bounds.size.x;
        }
        else
        {
            currentCargoZoneLength = lastCargoZoneObject.GetComponent<MeshCollider>().bounds.size.x;
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
