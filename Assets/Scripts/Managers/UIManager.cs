using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Camera mainCamera;

    TopPanel topPanel;
    ULDInfoPanel ULDInfoPanel;
    BottomPanel bottomPanel;
    CloundPanel cloundPanel;
    

    private void Awake()
    {
        mainCamera = Camera.main;


    }
}
