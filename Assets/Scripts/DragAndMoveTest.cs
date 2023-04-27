using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndMoveTest : MonoBehaviour
{
    RaycastHit hitLayerMask; //히트가 발생한 위치, Ray가 충돌한 물체, Ray의 원점에서 얼마나 떨어져있는지 등의 정보 반환
    DragAndMoveTest self;
    Vector3 distance = Vector3.zero;

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ray의 시작점을 마우스포인트로 잡음
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green); // 시작(origin) 지점부터 direction(방향 + 길이)까지 선을 그림 
        
        int layerMaskPlane = 1 << LayerMask.NameToLayer("Virtual Plane"); // Layer가 Virtual Plane인 것만 검출
        int layerMaskFreight = 1 << LayerMask.NameToLayer("Freight");
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMaskPlane)) // layerMask에 닿은 RaycastHit 반환
        {
            float currentX = this.transform.position.x;
            float currentY = this.transform.position.y;
            float currentZ = this.transform.position.z;

            this.transform.position = new Vector3(hitLayerMask.point.x, currentY, hitLayerMask.point.z); // 객체의 위치를 RaycastHit의 point값 위치로 이동

        }
        else if(Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMaskFreight))
        {
            this.transform.position = new Vector3(hitLayerMask.point.x, hitLayerMask.point.y, hitLayerMask.point.z);
        }
    }

    private void OnMouseUp()
    {
        Debug.Log("뗐다.");
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
