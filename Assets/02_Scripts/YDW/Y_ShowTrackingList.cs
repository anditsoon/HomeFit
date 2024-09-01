using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;


public class Y_ShowTrackingList : MonoBehaviour
{

    public WebSocketPoseHandler conn;
    //public List<Transform> bones;
    //public Y_MeasureModelSize measureModelSize;
    public Camera mainCamera;
    public Vector3 locationOffset;
    //public Vector3 scaleFactor;
    //private Vector3 initialModelScale;
    private Vector3 currentScaleFactor = Vector3.one;
    private Vector3 initialModelScale;
    public float targetHeight = 1.94f;
    public float scaleSmoothing = 0.1f; 
    public float minScale = 0.1f; 
    public float maxScale = 10f; 

    // Rigging
    public GameObject rigLeftArmTarget;
    public GameObject rigLeftArmHint;
    public GameObject rigRightArmTarget;
    public GameObject rigRightArmHint;
    public GameObject rigLeftLegTarget;
    public GameObject rigLeftLegHint;
    public GameObject rigRightLegTarget;
    public GameObject rigRightLegHint;
    public GameObject rigHead;
  

    // Start is called before the first frame update
    void Start()
    {

        conn = GameObject.Find("UDPConnector").GetComponent<WebSocketPoseHandler>();
        //measureModelSize = GetComponent<Y_MeasureModelSize>();
        initialModelScale = transform.localScale;
        locationOffset = transform.position;
        mainCamera = Camera.main;

        rigLeftArmTarget = GameObject.Find("Rig_LeftArm_target");
        rigLeftArmHint = GameObject.Find("Rig_LeftArm_hint");
        rigRightArmTarget = GameObject.Find("Rig_RightArm_target");
        rigRightArmHint = GameObject.Find("Rig_RightArm_hint");
        rigLeftLegTarget = GameObject.Find("Rig_LeftLeg_target");
        rigLeftLegHint = GameObject.Find("Rig_LeftLeg_hint");
        rigRightLegTarget = GameObject.Find("Rig_RightLeg_target");
        rigRightLegHint = GameObject.Find("Rig_RightLeg_hint");
        rigHead = GameObject.Find("Rig_Head");

    }

    

    // Update is called once per frame
    void Update()
    {

        //scaleFactor = measureModelSize.modelSize;
        // 만일, 트래킹된 데이터가 있다면
        if (conn.latestPoseList.landmarkList.Count > 0) //// < ?
        {
            UpdateScaleFactor();
            UpdateRigPosition();

        }

    }

    
    void UpdateScaleFactor()
    {
        float fullHeight = GetFullHeight();
        if (fullHeight > 0)
        {
            float targetScale = targetHeight / fullHeight;
            targetScale = Mathf.Clamp(targetScale, minScale, maxScale);

            // 부드러운 스케일 변화
            currentScaleFactor = Vector3.Lerp(currentScaleFactor, new Vector3(targetScale, targetScale, targetScale), scaleSmoothing);

            //Vector3 newScale = Vector3.Scale(initialModelScale, currentScaleFactor);
            //transform.localScale = newScale;

            //Debug.Log($"Full Height: {fullHeight}, Target Scale: {targetScale}, Current Scale: {currentScaleFactor}, New Model Scale: {newScale}");
        }
        //transform.localScale = Vector3.Scale(initialModelScale, scaleFactor);
    }

    public float GetFullHeight()
    {
        if (conn.latestPoseList.landmarkList.Count == 0)
            return 0f;

        float topY = conn.latestPoseList.landmarkList.Min(I => I.y);
        float bottomY = conn.latestPoseList.landmarkList.Max(l => l.y);
        return Mathf.Abs(bottomY - topY);
    }

    void UpdateRigPosition()
    {
        rigLeftArmTarget.transform.position = UpdateRigPart(15);
        rigLeftArmHint.transform.position = UpdateRigPart(13);
        rigRightArmTarget.transform.position = UpdateRigPart(16);
        rigRightArmHint.transform.position = UpdateRigPart(14);
        rigLeftLegTarget.transform.position = UpdateRigPart(27);
        rigLeftLegHint.transform.position = UpdateRigPart(25);
        rigRightLegTarget.transform.position = UpdateRigPart(28);
        rigRightLegHint.transform.position = UpdateRigPart(26);
        rigHead.transform.position = UpdateRigPart(0);
    }

    Vector3 UpdateRigPart(int i)
    {

        //float avatarHeight = 1.94f;
        //float dataHeight = Mathf.Abs(conn.latestPoseList.landmarkList[0].y - conn.latestPoseList.landmarkList[27].y);
        //float scaleY = avatarHeight / dataHeight;
        //scaleFactor = new Vector3(scaleY, scaleY, scaleY);

        Vector3 localPos = new Vector3(
                    conn.latestPoseList.landmarkList[i].x,
                    conn.latestPoseList.landmarkList[i].y,
                    conn.latestPoseList.landmarkList[i].z);

        //Vector3 scaledPos = Vector3.Scale(normalizedPos, scaleFactor);

        //Vector3 cameraSpacePos = mainCamera.transform.TransformPoint(scaledPos);

        localPos = new Vector3(localPos.x, -localPos.y, -localPos.z);

        Vector3 scaledPos = Vector3.Scale(localPos, currentScaleFactor);
        Vector3 worldPos = transform.TransformPoint(scaledPos) + locationOffset;

        //Debug.LogError($"Index {i} while Landmark list count: {conn.latestPoseList.landmarkList.Count}");

        return worldPos;

        //return transform.TransformPoint(Vector3.Scale(localPos, scaleFactor)) + locationOffset;

        //return localPos;

    }

    

    


}
