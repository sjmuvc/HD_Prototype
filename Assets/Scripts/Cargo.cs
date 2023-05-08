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


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.gameObject.tag = "StackObject";

        AddComponenet();

        // �θ� ������Ʈ ������ �ùٸ� �Ǻ� ����
        pivot = GetComponent<MeshCollider>().bounds.center;
        Objectpivot = new GameObject();
        Objectpivot.transform.position = pivot;
        startPosition = Objectpivot.transform.position;
        this.transform.parent = Objectpivot.transform;
        settingPivotPosition = this.transform.localPosition;
        settingPivotRotation = this.transform.localEulerAngles;
        Objectpivot.name = this.gameObject.name;

        // �Ǻ� ��ġ�� ���� ���� ������Ʈ �����ϰ� false
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
        // Mesh Collider ����
        if (this.gameObject.GetComponent<MeshCollider>() == null)
        {
            this.gameObject.AddComponent<MeshCollider>();
        }
        meshCollider = this.GetComponent<MeshCollider>();
        meshCollider.convex = true;
        objectHeight = meshCollider.bounds.size.y; // extents�� �� ��� x,y,z��� ������� ������Ʈ�� ����, ������ ���̰� �ȸ���

        // rigidBody ����
        this.gameObject.AddComponent<Rigidbody>();
        rigidBody = this.GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        rigidBody.isKinematic = true;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // ���� ������ ����
        this.gameObject.AddComponent<LineRenderer>();
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.material = gameManager.lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = Cacher.uiManager.mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraToObjectDistance)); // ī�޶�κ��� �Ÿ���
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

        int layerMask = 1 << LayerMask.NameToLayer("Virtual Plane"); // Layer�� Virtual Plane�� �͸� ����
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask)) // layerMask�� ���� RaycastHit ��ȯ
        {
            isOnVirtualPlane = true;
            Objectpivot.transform.position = new Vector3(hitLayerMask.point.x, Cacher.uldManager.virtualPlaneHeight + (objectHeight / 2) - 0.00001f, hitLayerMask.point.z); // ��ü�� ��ġ�� RaycastHit�� point�� ��ġ�� �̵�

            DetectStackHeight();
            DrawVirtualObject(isOnVirtualPlane);
            Cacher.uldManager.virtualPlaneMeshRenderer.enabled = false;
        }
        else
        {
            isOnVirtualPlane = false;
            DrawVirtualObject(isOnVirtualPlane);
            Cacher.uldManager.virtualPlaneMeshRenderer.enabled = true;
        }
    }
    private void OnMouseUp()
    {
        Cacher.inputManager.dragObject = null;
        lineRenderer.enabled = false;
        virtualObject.SetActive(false);
        Cacher.uldManager.virtualPlaneMeshRenderer.enabled = true;

        if (isOnVirtualPlane && isEnableStack)
        {
            Objectpivot.transform.position = new Vector3(Objectpivot.transform.position.x, currentStackHeight + objectHeight / 2, Objectpivot.transform.position.z);
            rigidBody.isKinematic = false;
            Cacher.cargoManager.stackObjects.Add(this.gameObject);
            Cacher.cargoManager.stackNum++;
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
        Objectpivot.transform.parent = Cacher.uldManager.uld.transform.Find("Objects").gameObject.transform;
        Cacher.inputManager.dragObject = Objectpivot;
        SettingObjectTransform();
        rigidBody.isKinematic = true;
    }

    void DrawVirtualObject(bool active)
    {
        thisPos = Objectpivot.GetComponent<Transform>().position;
        lineRenderer.SetPosition(0, thisPos);
        lineRenderer.SetPosition(1, new Vector3(thisPos.x, currentStackHeight + objectHeight / 2, thisPos.z)); // ������ ��ġ stackHeight
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

        #region �ùķ��̼� ���
        currentPos = Objectpivot.transform.position; // currentPos ��� ������Ʈ
        StartCoroutine(LastPosSetting(currentPos));  // lastPos�� �����̸� �ΰ� ������Ʈ

        time += Time.deltaTime;

        IEnumerator LastPosSetting(Vector3 currentPos)
        {
            yield return new WaitForSeconds(0.1f);
            lastPos = currentPos;
        }

        if (time > delayTimeToSimulation && currentPos == lastPos) // 1�� �̻� ������ ������ �ùķ��̼� ����
        {
            Simulation(true);

            simulationTime += Time.deltaTime;
            if (simulationTime > replayTimeToSimulation)
            {
                SettingVirtualObjectTransform(thisPos);
            }
        }
        else if (currentPos != lastPos) // �����̸� �ùķ��̼� ����
        {
            Simulation(false);

            time = 0;
            SettingVirtualObjectTransform(thisPos);
        }
        #endregion

        // �� �����̾�߸� �������� �� ����
        if (isInsideTheWall == true && Cacher.uldManager.virtualPlaneHeight > currentStackHeight + objectHeight)
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

        sweepTestHitAll = rigidBody.SweepTestAll(-Objectpivot.transform.up, Cacher.uldManager.virtualPlaneHeight + 5, QueryTriggerInteraction.Ignore);
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
                float rayHeight = Cacher.uldManager.virtualPlaneHeight - (sweepTestHitSelected.distance);
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
