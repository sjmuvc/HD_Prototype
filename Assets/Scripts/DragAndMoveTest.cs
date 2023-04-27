using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndMoveTest : MonoBehaviour
{
    RaycastHit hitLayerMask; //��Ʈ�� �߻��� ��ġ, Ray�� �浹�� ��ü, Ray�� �������� �󸶳� �������ִ��� ���� ���� ��ȯ
    DragAndMoveTest self;
    Vector3 distance = Vector3.zero;

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ray�� �������� ���콺����Ʈ�� ����
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green); // ����(origin) �������� direction(���� + ����)���� ���� �׸� 
        
        int layerMaskPlane = 1 << LayerMask.NameToLayer("Virtual Plane"); // Layer�� Virtual Plane�� �͸� ����
        int layerMaskFreight = 1 << LayerMask.NameToLayer("Freight");
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMaskPlane)) // layerMask�� ���� RaycastHit ��ȯ
        {
            float currentX = this.transform.position.x;
            float currentY = this.transform.position.y;
            float currentZ = this.transform.position.z;

            this.transform.position = new Vector3(hitLayerMask.point.x, currentY, hitLayerMask.point.z); // ��ü�� ��ġ�� RaycastHit�� point�� ��ġ�� �̵�

        }
        else if(Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMaskFreight))
        {
            this.transform.position = new Vector3(hitLayerMask.point.x, hitLayerMask.point.y, hitLayerMask.point.z);
        }
    }

    private void OnMouseUp()
    {
        Debug.Log("�ô�.");
        //self.AddComponent<Rigidbody>();
    }
}

/*
public class DragAndMove : MonoBehaviour
{
    GameObject objectHitPostion;
    RaycastHit hitRay, hitLayerMask;

    private void OnMouseUp()
    {
        this.transform.parent = null;
        Destroy(objectHitPostion);
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitRay))
        {
            objectHitPostion = new GameObject("HitPosition");
            objectHitPostion.transform.position = hitRay.point;
            this.transform.SetParent(objectHitPostion.transform);
        }
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
        {
            float H = Camera.main.transform.position.y;
            float h = objectHitPostion.transform.position.y;

            Vector3 newPos
                = (hitLayerMask.point * (H - h) + Camera.main.transform.position * h) / H;

            objectHitPostion.transform.position = newPos;
        }
    }
}
*/
