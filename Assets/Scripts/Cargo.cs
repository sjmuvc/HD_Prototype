using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cargo : MonoBehaviour, IPointerClickHandler
{
    GameManager gameManager;
    RaycastHit hitLayerMask;
    Vector3 thisPos;
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
    Vector3 startPosition;

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
    bool isEnableStack;
    int layerName;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.gameObject.tag = "StackObject";
        layerName = LayerMask.NameToLayer("Cargo");
        this.gameObject.layer = layerName;

        AddComponenet();

        // 부모 오브젝트 생성후 올바른 피봇 지정
        pivot = GetComponent<MeshCollider>().bounds.center;
        Objectpivot = new GameObject();
        Objectpivot.transform.position = pivot;
        startPosition = Objectpivot.transform.position;
        this.transform.parent = Objectpivot.transform;
        settingPivotPosition = this.transform.localPosition;
        settingPivotRotation = this.transform.localEulerAngles;
        Objectpivot.name = this.gameObject.name;

        // 피봇 위치를 맞춘 가상 오브젝트 생성하고 false
        virtualObject = Instantiate(Objectpivot, Objectpivot.transform);
        Destroy(virtualObject.GetComponentInChildren<Cargo>());
        Destroy(virtualObject.GetComponentInChildren<LineRenderer>());
        virtualObject.transform.GetChild(0).gameObject.GetComponent<MeshCollider>().convex = true;
        virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
        virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = true;
        virtualObjectOriginMat = gameManager.greenMaterial;
        virtualObject.transform.GetChild(0).gameObject.tag = "Untagged";
        virtualObject.transform.GetChild(0).gameObject.AddComponent<VirtualObjectTrigger>();
        virtualObject.transform.GetChild(0).gameObject.GetComponent<VirtualObjectTrigger>().cargoManager = this.GetComponent<Cargo>();
        virtualObject.SetActive(false);

        GotoObjectZone();
    }

    void AddComponenet()
    {
        // Mesh Collider 생성
        if (this.gameObject.GetComponent<MeshCollider>() == null)
        {
            this.gameObject.AddComponent<MeshCollider>();
        }
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
        Vector3 worldMousePos = Cacher.uiManager.mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraToObjectDistance)); // 카메라로부터 거리값
        Objectpivot.transform.position = worldMousePos;
        Cacher.cargoManager.AllFreeze(true);
        RayPositioning(worldMousePos);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Objectpivot.transform.localEulerAngles = new Vector3(Objectpivot.transform.localEulerAngles.x, Objectpivot.transform.localEulerAngles.y + rotateValue, Objectpivot.transform.localEulerAngles.z);
        }
    }

    void RayPositioning(Vector3 worldMousePos)
    {
        Ray ray = Cacher.uiManager.mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * mouseRayDistance, Color.red);

        int layerMask = 1 << LayerMask.NameToLayer("Virtual Plane"); // Layer가 Virtual Plane인 것만 검출
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask)) // layerMask에 닿은 RaycastHit 반환
        {
            isOnVirtualPlane = true;
            Objectpivot.transform.position = new Vector3(hitLayerMask.point.x, Cacher.uldManager.currentULD.virtualPlaneHeight + (objectHeight / 2) - 0.00001f, hitLayerMask.point.z); // 객체의 위치를 RaycastHit의 point값 위치로 이동

            DetectStackHeight();
            DrawVirtualObject(isOnVirtualPlane);
            Cacher.uldManager.currentULD.virtualPlaneMeshRenderer.enabled = false;
        }
        else
        {
            isOnVirtualPlane = false;
            DrawVirtualObject(isOnVirtualPlane);
            Cacher.uldManager.currentULD.virtualPlaneMeshRenderer.enabled = true;
        }
    }
    private void OnMouseUp()
    {
        Cacher.cargoManager.dragObject = null;
        lineRenderer.enabled = false;
        virtualObject.SetActive(false);
        Cacher.uldManager.currentULD.virtualPlaneMeshRenderer.enabled = true;

        if (isOnVirtualPlane && isEnableStack)
        {
            Objectpivot.transform.position = new Vector3(Objectpivot.transform.position.x, currentStackHeight + objectHeight / 2, Objectpivot.transform.position.z);
            rigidBody.isKinematic = false;
            Cacher.cargoManager.uldObjects.Add(this.gameObject);
            Cacher.cargoManager.uldObjectsNum++;
        }
        else
        {
            GotoObjectZone();
        }
        Simulation(false);
        Cacher.cargoManager.AllFreeze(false);
    }

    private void OnMouseDown()
    {
        Objectpivot.transform.parent = Cacher.uldManager.currentULD.uld.transform.Find("Objects").gameObject.transform;
        Cacher.cargoManager.dragObject = Objectpivot;
        SettingObjectTransform();
        rigidBody.isKinematic = true;
    }

    void DrawVirtualObject(bool active)
    {
        thisPos = Objectpivot.GetComponent<Transform>().position;
        lineRenderer.SetPosition(0, thisPos);
        lineRenderer.SetPosition(1, new Vector3(thisPos.x, currentStackHeight + objectHeight / 2, thisPos.z)); // 내려갈 위치 stackHeight
        lineRenderer.enabled = active;

        if (active)
        {
            virtualObject.SetActive(active);
            virtualObject.transform.position = new Vector3(thisPos.x, currentStackHeight + objectHeight / 2, thisPos.z);
        }
        else
        {
            virtualObject.SetActive(active);
        }

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
            Simulation(true);

            simulationTime += Time.deltaTime;
            if (simulationTime > replayTimeToSimulation)
            {
                SettingVirtualObjectTransform(thisPos);
            }
        }
        else if (currentPos != lastPos) // 움직이면 시뮬레이션 종료
        {
            Simulation(false);

            time = 0;
            SettingVirtualObjectTransform(thisPos);
        }
        #endregion

        // 벽 안쪽이어야만 내려놓을 수 있음
        if (isInsideTheWall == true && Cacher.uldManager.currentULD.virtualPlaneHeight > currentStackHeight + objectHeight)
        {
            EnableStack(true);
        }
        else
        {
            EnableStack(false);
        }
    }

    void DetectStackHeight()
    {
        RaycastHit[] sweepTestHitAll;

        sweepTestHitAll = rigidBody.SweepTestAll(-Objectpivot.transform.up, Cacher.uldManager.currentULD.virtualPlaneHeight + 5, QueryTriggerInteraction.Ignore);
        if (sweepTestHitAll.Length == 0)
        {
            return;
        }

        RaycastHit sweepTestHitSelected = sweepTestHitAll[0];

        foreach (RaycastHit sweepTestHit in sweepTestHitAll)
        {
            if (sweepTestHit.collider.tag == "StackObject")
            {
                if (sweepTestHitSelected.distance > sweepTestHit.distance || sweepTestHitSelected.collider.tag != "StackObject")
                {
                    sweepTestHitSelected = sweepTestHit;
                }
                float rayHeight = Cacher.uldManager.currentULD.virtualPlaneHeight - (sweepTestHitSelected.distance);
                currentStackHeight = rayHeight;
            }
        }
    }
    void EnableStack(bool enable)
    {
        if (!isSimulationOn)
        {
            if (enable)
            {
                virtualObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = virtualObjectOriginMat;
                isEnableStack = true;
            }
            else
            {
                virtualObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = gameManager.redMaterial;
                isEnableStack = false;
            }
        }
    }

    public void GotoObjectZone()
    {
        SettingObjectTransform();
        Objectpivot.transform.position = startPosition;
        Objectpivot.transform.parent = Cacher.cargoManager.cargoZone.transform.Find("Objects").gameObject.transform;
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
            virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = false;
            isSimulationOn = active;
        }
        else
        {
            virtualObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = true;
            isSimulationOn = active;
        }
    }

    public void GenerateCargo(List<string> tmp)
    {

    }

}

