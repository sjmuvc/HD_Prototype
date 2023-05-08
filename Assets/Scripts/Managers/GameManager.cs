using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    
    public float newObjectHeight;
    public float virtualPlaneHeight;
    GameObject virtualPlane;
    public MeshRenderer virtualPlaneMeshRenderer;
    public GameObject objectZone;
    public Material redMaterial;
    public Material greenMaterial;
    public Material lineMaterial;
    public GameObject dragObject;
    //public Text dragText;
    //float maxCargoCount = 100;
    //float totalWeight;

    //List<Vector3> originTransformForSimulation = new List<Vector3>();

    void Awake()
    {
        cam = Camera.main;
        objectZone = GameObject.Find("ObjectZone");
        virtualPlane = GameObject.Find("ULD_VirtualPlane");
        virtualPlaneMeshRenderer = virtualPlane.GetComponent<MeshRenderer>();
        virtualPlaneHeight = virtualPlane.transform.position.y; 
    }

    public void SaveOriginTransform()
    {

    }
}
