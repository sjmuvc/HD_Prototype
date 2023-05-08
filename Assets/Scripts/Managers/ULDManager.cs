using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULDManager : MonoBehaviour
{
    public ULD[] ulds;
    public ULD currentULD;

    public GameObject uld;
    public GameObject uld_Plane;
    public float uldHeight;

    GameObject virtualPlane;
    public float virtualPlaneHeight;
    public MeshRenderer virtualPlaneMeshRenderer;

    private void Awake() 
    {
        Initialize();
        currentULD = GameObject.FindGameObjectWithTag("ULD").GetComponent<ULD>();
    }

    void Initialize()
    {
        uld = GameObject.FindGameObjectWithTag("ULD");
        uld_Plane = GameObject.Find("ULD_Plane");
        uldHeight = uld_Plane.transform.position.y;

        virtualPlane = GameObject.Find("ULD_VirtualPlane");
        virtualPlaneMeshRenderer = virtualPlane.GetComponent<MeshRenderer>();
        virtualPlaneHeight = virtualPlane.transform.position.y;
    }

    public void ResetULD()
    {
        
    }

    public void ChangeULD(int selectedULDNum)
    {
        ResetULD();
        Destroy(currentULD.gameObject);
        currentULD = Instantiate(ulds[selectedULDNum]); // »õ uld »ý¼º
        Initialize();
    }
}
