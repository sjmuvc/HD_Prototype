using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndMove : MonoBehaviour, IPointerClickHandler
{
    GameManager gameManager;
    RaycastHit hitLayerMask;
    RaycastHit hitStackHeight;
    Vector3 thisPos, uldPos;
    GameObject virtualObject;
    float objectHeight;
    Vector3 pivot;
    public GameObject Objectpivot;
    Vector3 settingPivotPosition;
    Vector3 settingPivotRotation;
    Rigidbody rigidBody;
    LineRenderer lineRenderer;
    Material virtualObjectOriginMat;
    MeshCollider meshCollider;

    float cameraToObjectDistance = 20;
    float mouseRayDistance = 1000;
    bool isOnVirtualPlane = false;
    float rotateValue = 10;
    float currentStackHeight;
    public bool isInsideTheWall = true;
    float lineWidth = .05f;
    float time;
    float simulationTime;
    Vector3 currentPos, lastPos;
    float delayTimeToSimulation = 0.5f;
    float replayTimeToSimulation = 2.5f;
    bool isSimulationOn;


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.gameObject.tag = "StackObject";

        AddComponenet();

        // 부모 오브젝트 생성후 올바른 피봇 지정
        pivot = GetComponent<MeshCollider>().bounds.center;
        Objectpivot = new GameObject();
        Objectpivot.transform.position = pivot;
        this.transform.parent = Objectpivot.transform;
        settingPivotPosition = this.transform.localPosition;
        settingPivotRotation = this.transform.localEulerAngles;

        // 피봇 위치를 맞춘 가상 오브젝트 생성하고 false
        virtualObject = Instantiate(Objectpivot, Objectpivot.transform);
        Destroy(virtualObject.GetComponentInChildren<DragAndMove>());
        Destroy(virtualObject.GetComponentInChildren<LineRenderer>());
        virtualObject.transform.GetChild(0).gameObject.GetComponent<MeshCollider>().convex = false;
        virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = true;
        virtualObjectOriginMat = gameManager.greenMaterial;
        virtualObject.transform.GetChild(0).gameObject.tag = "Untagged";
        virtualObject.transform.GetChild(0).gameObject.AddComponent<VirtualObjectTrigger>();
        virtualObject.transform.GetChild(0).gameObject.GetComponent<VirtualObjectTrigger>().dragAndMove = this.GetComponent<DragAndMove>();
        virtualObject.SetActive(false);
    }

    void AddComponenet()
    {
        // Mesh Collider 생성
        this.gameObject.AddComponent<MeshCollider>();
        meshCollider = this.GetComponent<MeshCollider>();
        meshCollider.convex = true;
        objectHeight = meshCollider.bounds.size.y; // extents로 할 경우 x,y,z축과 상관없는 오브젝트의 높이, 하지만 높이가 안맞음

        // rigidBody 생성
        this.gameObject.AddComponent<Rigidbody>();
        rigidBody = this.GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        rigidBody.isKinematic = true;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // 라인 렌더러 생성
        this.gameObject.AddComponent<LineRenderer>();
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.material = gameManager.lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = gameManager.cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraToObjectDistance)); // 카메라로부터 거리값
        Objectpivot.transform.position = worldMousePos;

        RayPositioning();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Objectpivot.transform.localEulerAngles = new Vector3(Objectpivot.transform.localEulerAngles.x, Objectpivot.transform.localEulerAngles.y + rotateValue, Objectpivot.transform.localEulerAngles.z);
        }
    }

    void RayPositioning()
    {
        Ray ray = gameManager.cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * mouseRayDistance, Color.red);

        int layerMask = 1 << LayerMask.NameToLayer("Virtual Plane"); // Layer가 Virtual Plane인 것만 검출
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask)) // layerMask에 닿은 RaycastHit 반환
        {
            isOnVirtualPlane = true;
            Objectpivot.transform.position = new Vector3(hitLayerMask.point.x, gameManager.virtualPlaneHeight + (objectHeight / 2) - 0.00001f, hitLayerMask.point.z); // 객체의 위치를 RaycastHit의 point값 위치로 이동

            DrawVirtualObject(isOnVirtualPlane);
            DetectStackHeight();
            gameManager.virtualPlaneMeshRenderer.enabled = false;
        }
        else
        {
            isOnVirtualPlane = false;
            DrawVirtualObject(isOnVirtualPlane);
            gameManager.virtualPlaneMeshRenderer.enabled = true;
        }
    }
    private void OnMouseUp()
    {
        lineRenderer.enabled = false;
        virtualObject.SetActive(false);
        gameManager.virtualPlaneMeshRenderer.enabled = true;

        if (isOnVirtualPlane && isInsideTheWall && gameManager.virtualPlaneHeight > currentStackHeight + objectHeight)
        {
            Objectpivot.transform.position = new Vector3(Objectpivot.transform.position.x, currentStackHeight + objectHeight / 2, Objectpivot.transform.position.z);
            rigidBody.isKinematic = false;
            Objectpivot.transform.parent = gameManager.uld.transform.Find("Objects").gameObject.transform;
            gameManager.stackObjects.Add(this.gameObject);
            gameManager.stackNum++;
        }
        else
        {
            GotoObjectZone();
        }

        // 시뮬레이션 종료
        isSimulationOn = false;
        Simulation(isSimulationOn);
        gameManager.AllFreeze(isSimulationOn);
    }

    private void OnMouseDown()
    {
        SettingObjectTransform();
        rigidBody.isKinematic = true;
    }

    void DrawVirtualObject(bool active)
    {
        thisPos = Objectpivot.GetComponent<Transform>().position;
        lineRenderer.SetPosition(0, thisPos);
        lineRenderer.SetPosition(1, new Vector3(thisPos.x, currentStackHeight + objectHeight / 2, thisPos.z)); // 내려갈 위치 stackHeight
        lineRenderer.enabled = active;

        virtualObject.SetActive(active);
        virtualObject.transform.position = new Vector3(thisPos.x, currentStackHeight + objectHeight / 2, thisPos.z);

        #region 시뮬레이션 기능
        currentPos = Objectpivot.transform.position; // currentPos 계속 업데이트
        StartCoroutine(LastPosSetting(currentPos));  // lastPos는 딜레이를 두고 업데이트

        time += Time.deltaTime;

        IEnumerator LastPosSetting(Vector3 currentPos)
        {
            yield return new WaitForSeconds(0.1f);
            lastPos = currentPos;
        }

        if (time > delayTimeToSimulation && currentPos == lastPos) // 1초 이상 움직임 없으면 시뮬레이션 시작
        {
            isSimulationOn = true;
            Simulation(isSimulationOn);

            simulationTime += Time.deltaTime;
            if (simulationTime > replayTimeToSimulation)
            {
                SettingVirtualObjectTransform(thisPos);
            }
        }
        else if (currentPos != lastPos) // 움직이면 시뮬레이션 종료
        {
            isSimulationOn = false;
            Simulation(isSimulationOn);

            time = 0;
            SettingVirtualObjectTransform(thisPos);
        }
        #endregion

        // 내려놓을 수 없다면 색 변경
        if (isInsideTheWall == false || gameManager.virtualPlaneHeight < currentStackHeight + objectHeight)
        {
            virtualObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = gameManager.redMaterial;
        }
        else
        {
            virtualObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = virtualObjectOriginMat;
        }
    }

    void DetectStackHeight()
    {
        RaycastHit sweepTestHit;
        
        if (rigidBody.SweepTest(-Objectpivot.transform.up, out sweepTestHit, gameManager.virtualPlaneHeight + 50))
        {
            if (sweepTestHit.collider.tag == "StackObject")
            {
                float rayHeight = gameManager.virtualPlaneHeight - (sweepTestHit.distance);
                currentStackHeight = rayHeight;
                Debug.Log(currentStackHeight);
            } 
        }
    }

    public void GotoObjectZone()
    {
        SettingObjectTransform();
        Objectpivot.transform.position = new Vector3(gameManager.objectZone.transform.position.x, gameManager.objectZone.transform.position.y + 5, gameManager.objectZone.transform.position.z);
        Objectpivot.transform.parent = gameManager.objectZone.transform.Find("Objects").gameObject.transform;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Debug.Log(name + "Game Object Clicked!");
    }

    void SettingObjectTransform()
    {
        this.gameObject.transform.localPosition = settingPivotPosition;
        this.gameObject.transform.localEulerAngles = settingPivotRotation;
    }

    void SettingVirtualObjectTransform(Vector3 thisPos)
    {
        virtualObject.transform.position = new Vector3(thisPos.x, currentStackHeight + objectHeight / 2, thisPos.z);
        virtualObject.transform.GetChild(0).gameObject.transform.localPosition = settingPivotPosition;
        virtualObject.transform.GetChild(0).gameObject.transform.localEulerAngles = settingPivotRotation;
        simulationTime = 0;
    }

    void Simulation(bool active)
    {
        if (active)
        {
            if (virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>() == null && virtualObject.transform.GetChild(0).gameObject.GetComponent<MeshCollider>() == null)
            {
                virtualObject.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
                virtualObject.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
            }
            virtualObject.transform.GetChild(0).gameObject.GetComponent<MeshCollider>().convex = true;
            virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
            virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameManager.AllFreeze(active);
        }
        else
        {
            if (virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>() != null || virtualObject.transform.GetChild(0).gameObject.GetComponent<MeshCollider>() != null)
            {
                virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            gameManager.AllFreeze(active);
        }
    }




}

