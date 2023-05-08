using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    TopPanel topPanel;
    ULDInfoPanel ULDInfoPanel;
    BottomPanel bottomPanel;
    CloundPanel cloundPanel;
    public Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
}
