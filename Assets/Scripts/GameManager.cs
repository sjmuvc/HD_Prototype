using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    public float uldHeight;
    public float newObjectHeight;
    public float virtualPlaneHeight;
    GameObject virtualPlane;
    public MeshRenderer virtualPlaneMeshRenderer;
    public GameObject uld;
    public GameObject uld_Plane;
    public GameObject objectZone;
    float totalWeight;
    public Material redMaterial;
    public Material greenMaterial;
    public Material lineMaterial;
    public GameObject dragObject;
    Vector3 rotatePivot;
    public Text dragText;

    public List<GameObject> stackObjects = new List<GameObject>();
    public int stackNum;
    List<Vector3> originTransformForSimulation = new List<Vector3>();

    void Awake()
    {
        cam = Camera.main;
        objectZone = GameObject.Find("ObjectZone");
        virtualPlane = GameObject.Find("ULD_VirtualPlane");
        virtualPlaneMeshRenderer = virtualPlane.GetComponent<MeshRenderer>();
        virtualPlaneHeight = virtualPlane.transform.position.y;
        uld = GameObject.Find("ULD");
        uld_Plane = GameObject.Find("ULD_Plane");
        uldHeight = uld_Plane.transform.position.y;  
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (stackNum - 1 >= 0) 
            {
                stackObjects[stackNum - 1].GetComponent<DragAndMove>().GotoObjectZone();
                stackObjects.RemoveAt(stackNum - 1);
                stackNum--;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            uld.transform.localEulerAngles = new Vector3(uld.transform.localEulerAngles.x, uld.transform.localEulerAngles.y + 10, uld.transform.localEulerAngles.z);
            if (dragObject != null)
            {
                dragObject.transform.localEulerAngles = new Vector3(dragObject.transform.localEulerAngles.x, dragObject.transform.localEulerAngles.y + 10, dragObject.transform.localEulerAngles.z);
            }   
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            uld.transform.localEulerAngles = new Vector3(uld.transform.localEulerAngles.x, uld.transform.localEulerAngles.y - 10, uld.transform.localEulerAngles.z);
            if (dragObject != null)
            {
                dragObject.transform.localEulerAngles = new Vector3(dragObject.transform.localEulerAngles.x, dragObject.transform.localEulerAngles.y - 10, dragObject.transform.localEulerAngles.z);
            }
        }
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

    public void SaveOriginTransform()
    {

    }
}
