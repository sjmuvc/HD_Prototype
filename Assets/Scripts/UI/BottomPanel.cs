using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    public Button btn_SCB1;
    public Button btn_SCB2;

    private void Awake()
    {
        btn_SCB1.onClick.AddListener(OnClickButton_SCB1);
        btn_SCB2.onClick.AddListener(OnClickButton_SCB2);
    }

    void OnClickButton_SCB1()
    {
        Cacher.uldManager.ChangeULD();
    }

    void OnClickButton_SCB2()
    {
        Cacher.uldManager.ChangeULD();
    }

    void OnClickButton_SpawnCargo()
    {

    }
}
