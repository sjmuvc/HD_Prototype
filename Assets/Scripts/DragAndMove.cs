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

        // �θ� ������Ʈ ������ �ùٸ� �Ǻ� ����
        pivot = GetComponent<MeshCollider>().bounds.center;
        Objectpivot = new GameObject();
        Objectpivot.transform.position = pivot;
        this.transform.parent = Objectpivot.transform;
        settingPivotPosition = this.transform.localPosition;
        settingPivotRotation = this.transform.localEulerAngles;

        // �Ǻ� ��ġ�� ���� ���� ������Ʈ �����ϰ� false
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
        // Mesh Collider ����
        this.gameObject.AddComponent<MeshCollider>();
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
        Vector3 worldMousePos = gameManager.cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraToObjectDistance)); // ī�޶�κ��� �Ÿ���
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

        int layerMask = 1 << LayerMask.NameToLayer("Virtual Plane"); // Layer�� Virtual Plane�� �͸� ����
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask)) // layerMask�� ���� RaycastHit ��ȯ
        {
            isOnVirtualPlane = true;
            Objectpivot.transform.position = new Vector3(hitLayerMask.point.x, gameManager.virtualPlaneHeight + (objectHeight / 2) - 0.00001f, hitLayerMask.point.z); // ��ü�� ��ġ�� RaycastHit�� point�� ��ġ�� �̵�

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

        // �ùķ��̼� ����
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
        lineRenderer.SetPosition(1, new Vector3(thisPos.x, currentStackHeight + objectHeight / 2, thisPos.z)); // ������ ��ġ stackHeight
        lineRenderer.enabled = active;

        virtualObject.SetActive(active);
        virtualObject.transform.position = new Vector3(thisPos.x, currentStackHeight + objectHeight / 2, thisPos.z);

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
            isSimulationOn = true;
            Simulation(isSimulationOn);

            simulationTime += Time.deltaTime;
            if (simulationTime > replayTimeToSimulation)
            {
                SettingVirtualObjectTransform(thisPos);
            }
        }
        else if (currentPos != lastPos) // �����̸� �ùķ��̼� ����
        {
            isSimulationOn = false;
            Simulation(isSimulationOn);

            time = 0;
            SettingVirtualObjectTransform(thisPos);
        }
        #endregion

        // �������� �� ���ٸ� �� ����
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

