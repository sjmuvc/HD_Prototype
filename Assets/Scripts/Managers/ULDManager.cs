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
    }

    public void ChangeULD(int selectedULDNum)
    {
        ResetULD();
        Destroy(currentULD.gameObject);
        currentULD = Instantiate(ulds[selectedULDNum]); // »õ uld »ý¼º
        currentULD.Initialize();
    }
}
