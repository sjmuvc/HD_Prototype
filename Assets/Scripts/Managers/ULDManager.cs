using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULDManager : MonoBehaviour
{
    public GameObject uld;
    public GameObject uld_Plane;
    public float uldHeight;

    GameObject virtualPlane;
    public float virtualPlaneHeight;
    public MeshRenderer virtualPlaneMeshRenderer;

    public List<GameObject> stackObjects = new List<GameObject>();
    public int stackNum;

    private void Awake()
    {
        uld = GameObject.Find("ULD");
        uld_Plane = GameObject.Find("ULD_Plane");
        uldHeight = uld_Plane.transform.position.y;

        virtualPlane = GameObject.Find("ULD_VirtualPlane");
        virtualPlaneMeshRenderer = virtualPlane.GetComponent<MeshRenderer>();
        virtualPlaneHeight = virtualPlane.transform.position.y;
    }

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
