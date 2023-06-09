using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULDManager : MonoBehaviour
{
    public ULD[] ulds;
    public ULD currentULD;

    private void Awake() 
    {
        currentULD = GameObject.FindGameObjectWithTag("ULD").GetComponent<ULD>();
        currentULD.Initialize();
    }

    public void ResetULD()
    {
        Cacher.cargoManager.GotoObjectZoneAll();
        Cacher.uiManager.GetComponent<ULDInfoPanel>().Reset();
    }

    public void ChangeULD(int selectedULDNum)
    {
        Destroy(currentULD.gameObject);
        currentULD = Instantiate(ulds[selectedULDNum]); // �� uld ����
        currentULD.Initialize();
        ResetULD();
    }
}
