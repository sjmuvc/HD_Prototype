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
            dragAndMove.isInsideTheWall = false;
            //Debug.Log("Stay");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            dragAndMove.isInsideTheWall = true;
            //Debug.Log("Exit");
        }
    }
}
