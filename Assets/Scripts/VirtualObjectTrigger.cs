using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VirtualObjectTrigger : MonoBehaviour
{
    public DragAndMove dragAndMove;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            dragAndMove.isEnableStack = false;
            Debug.Log("´êÀ½");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            dragAndMove.isEnableStack = true;
            Debug.Log("¶³¾îÁü");
        }
    }
}
