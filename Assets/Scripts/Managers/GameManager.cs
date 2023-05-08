using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject objectZone;
    public Material redMaterial;
    public Material greenMaterial;
    public Material lineMaterial;
    
    //public Text dragText;
    //List<Vector3> originTransformForSimulation = new List<Vector3>();

    void Awake()
    {
        objectZone = GameObject.Find("ObjectZone");
    }

    public void SaveOriginTransform()
    {

    }
}
