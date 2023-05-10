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
    float cargoZoneLength_X;
    float cargoZoneLength_Z;
    float currentCargoZoneLength_X;
    float currentCargoZoneLength_Z;
    float longestAxis_Z;
    float axisSpacing_Z;

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
        cargoZoneLength_X = cargoZonePlane.GetComponent<MeshCollider>().bounds.size.x;
        cargoZoneLength_Z = cargoZonePlane.GetComponent<MeshCollider>().bounds.size.z;
    }

    public void GenerateCargo(int cargosQuantity)
    {
        for (int i = 0; i < cargoZoneObjects.Count; i++)
        {
            Destroy(cargoZoneObjects[i].GetComponent<Cargo>().Objectpivot);
        }
        cargoZoneObjects.Clear();

        cargoIndex = 0;
        remainCargoIndex = 0;
        currentGenerateCargo = 0;
        currentCargoZoneLength_X = 0;
        currentCargoZoneLength_Z = 0;
        longestAxis_Z = 0;
        axisSpacing_Z = 0;

        // spawnRate��ŭ ���� ����
        for (int i = 0; i < cargos.Length; i++)
        {
            if (cargos[cargoIndex].GetComponent<CargoInfo>().spawnRate == 0)
            {
                remainCargoIndex = cargoIndex;
            }
            for (int j = 0; j < Mathf.Floor((cargosQuantity * cargos[cargoIndex].GetComponent<CargoInfo>().spawnRate)); j++) 
            {
                Cargo generatedCargo = Instantiate(cargos[cargoIndex], cargoZone.transform);
                cargoZoneObjects.Add(generatedCargo.gameObject);
                currentGenerateCargo++;
                generatedCargo.GetComponent<Cargo>().GenerateSetting();
                CargoZonePositioning(generatedCargo.gameObject);
                generatedCargo.startPosition = generatedCargo.Objectpivot.transform.localPosition;
                generatedCargo.startLocalEulerAngles = generatedCargo.Objectpivot.transform.localEulerAngles;
            }
            cargoIndex++;
        }

        // ������ ������ spawnRate�� 0�� ������Ʈ�� ä��
        for (int i = 0; i < cargosQuantity - currentGenerateCargo; i++) 
        {
            Cargo generatedCargo =  Instantiate(cargos[remainCargoIndex], cargoZone.transform);
            cargoZoneObjects.Add(generatedCargo.gameObject);
            generatedCargo.GetComponent<Cargo>().GenerateSetting();
            CargoZonePositioning(generatedCargo.gameObject);
            generatedCargo.startPosition = generatedCargo.Objectpivot.transform.localPosition;
            generatedCargo.startLocalEulerAngles = generatedCargo.Objectpivot.transform.localEulerAngles;
        }
    }

    public void CargoZonePositioning(GameObject addedCargo)
    {
        addedCargo.GetComponent<Cargo>().Objectpivot.transform.localPosition = Vector3.zero;
        if (currentCargoZoneLength_X + addedCargo.GetComponent<MeshCollider>().bounds.size.x > cargoZoneLength_X)
        {
            // CargoZone�� ������Ʈ�� ���� �� Z���� ���� Z�� �������� ����
            for (int i = 0; i < cargoZoneObjects.Count; i++)
            {
                if (longestAxis_Z < cargoZoneObjects[i].gameObject.GetComponent<MeshCollider>().bounds.size.z)
                {
                    longestAxis_Z = cargoZoneObjects[i].gameObject.GetComponent<MeshCollider>().bounds.size.z;
                }
            }
            axisSpacing_Z += longestAxis_Z;
            currentCargoZoneLength_X = 0;
        }
        addedCargo.GetComponent<Cargo>().Objectpivot.transform.localPosition = new Vector3(currentCargoZoneLength_X + addedCargo.GetComponent<MeshCollider>().bounds.size.x / 2, addedCargo.GetComponent<MeshCollider>().bounds.size.y / 2, -axisSpacing_Z);
        currentCargoZoneLength_X += addedCargo.GetComponent<MeshCollider>().bounds.size.x;
    }

    public void RemoveAtuldObjects()
    {
        if (uldObjectsNum - 1 >= 0)
        {
            uldObjects[uldObjectsNum - 1].GetComponent<Cargo>().GotoCargoZone();
            uldObjects.RemoveAt(uldObjectsNum - 1);
            uldObjectsNum--;
        }
    }

    public void GotoObjectZoneAll()
    {
        for(int i = 0; i < uldObjectsNum; i++)
        {
            uldObjects[i].GetComponent<Cargo>().GotoCargoZone();
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
