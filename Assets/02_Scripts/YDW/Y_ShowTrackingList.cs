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
    public float targetHeight;
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
    public GameObject rigHeadAim1;
    public GameObject rigHeadAim2;
    public GameObject rigRightHandTarget;
    public GameObject rigRightHandHint;
    public GameObject rigLeftHandTarget;
    public GameObject rigLeftHandHint;
    //public GameObject rigSpineTarget;
    //public GameObject rigSpineHint;
    //public GameObject rigRootTarget;
    //public GameObject rigRootHint;


    // Start is called before the first frame update
    void Start()
    {

        conn = GameObject.Find("UDPConnector").GetComponent<WebSocketPoseHandler>();
        //measureModelSize = GetComponent<Y_MeasureModelSize>();
        initialModelScale = transform.localScale;
        //locationOffset = GameObject.Find("GameObject").transform.position;
        mainCamera = Camera.main;
        targetHeight = 0.78f;

        rigLeftArmTarget = GameObject.Find("Rig_LeftArm_target");
        rigLeftArmHint = GameObject.Find("Rig_LeftArm_hint");
        rigRightArmTarget = GameObject.Find("Rig_RightArm_target");
        rigRightArmHint = GameObject.Find("Rig_RightArm_hint");
        rigLeftLegTarget = GameObject.Find("Rig_LeftLeg_target");
        rigLeftLegHint = GameObject.Find("Rig_LeftLeg_hint");
        rigRightLegTarget = GameObject.Find("Rig_RightLeg_target");
        rigRightLegHint = GameObject.Find("Rig_RightLeg_hint");
        rigHeadAim1 = GameObject.Find("Aim");
        rigHeadAim2 = GameObject.Find("Aim2");
        rigRightHandTarget = GameObject.Find("RigRightHand_target");
        rigLeftHandTarget = GameObject.Find("RigLeftHand_target");
        rigRightHandHint = GameObject.Find("RigRightHand_hint");
        rigLeftHandHint = GameObject.Find("RigLeftHand_hint");
        //rigSpineTarget = GameObject.Find("Rig_Spine_target");
        //rigSpineHint = GameObject.Find("Rig_Spine_hint");
        //rigRootTarget = GameObject.Find("RigRoot_target");
        //rigRootHint = GameObject.Find("RigRoot_hint");
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
            currentScaleFactor = Vector3.one * targetScale;
        }
    }

    public float GetFullHeight()
    {
        if (conn.latestPoseList.landmarkList.Count == 0)
            return 0f;

        //float topY = conn.latestPoseList.landmarkList.Max(I => I.y);
        //float bottomY = conn.latestPoseList.landmarkList.Min(l => l.y);


        float topY = (conn.latestPoseList.landmarkList[11].y + conn.latestPoseList.landmarkList[12].y) * 0.5f;
        float bottomY = (conn.latestPoseList.landmarkList[23].y + conn.latestPoseList.landmarkList[24].y) * 0.5f;

        return Mathf.Abs(bottomY - topY);
    }

    void UpdateRigPosition()
    {
        rigHeadAim1.transform.position = UpdateRigPart(9);
        rigHeadAim2.transform.position = UpdateRigPart(10);
        rigLeftArmHint.transform.position = UpdateRigPart(13);
        rigRightArmHint.transform.position = UpdateRigPart(14);
        rigLeftArmTarget.transform.position = UpdateRigPart(15);
        rigLeftHandHint.transform.position = UpdateRigPart(15);
        rigRightArmTarget.transform.position = UpdateRigPart(16);
        rigRightHandHint.transform.position = UpdateRigPart(16);
        rigLeftHandTarget.transform.position = UpdateRigPart(21);
        rigRightHandTarget.transform.position = UpdateRigPart(22);
        rigLeftLegHint.transform.position = UpdateRigPart(25);
        rigRightLegHint.transform.position = UpdateRigPart(26);
        rigLeftLegTarget.transform.position = UpdateRigPart(27);
        rigRightLegTarget.transform.position = UpdateRigPart(28);
        //rigSpineTarget.transform.position =(UpdateRigPart(12) + UpdateRigPart(11))  * 0.5f;
        //rigSpineHint.transform.position = (UpdateRigPart(24) + UpdateRigPart(23))* 0.5f;
        //rigRootTarget.transform.position = (UpdateRigPart(24) + UpdateRigPart(23)) * 0.5f;
        //rigRootHint.transform.position = (UpdateRigPart(24) + UpdateRigPart(23)) * 0.5f;
    }

    Vector3 UpdateRigPart(int i)
    {

        Vector3 vector23rd = new Vector3(
            conn.latestPoseList.landmarkList[23].x,
            conn.latestPoseList.landmarkList[23].y,
            conn.latestPoseList.landmarkList[23].z
            );

        Vector3 vector24th = new Vector3(
            conn.latestPoseList.landmarkList[24].x,
            conn.latestPoseList.landmarkList[24].y,
            conn.latestPoseList.landmarkList[24].z
            );

        Vector3 vectorMiddle = (vector23rd +vector24th) / 2;


        Vector3 localPos = new Vector3(
                    conn.latestPoseList.landmarkList[i].x,
                    conn.latestPoseList.landmarkList[i].y,
                    conn.latestPoseList.landmarkList[i].z);

        localPos = localPos - vectorMiddle;

        localPos = new Vector3(localPos.x, -localPos.y, localPos.z);

        return transform.TransformPoint(Vector3.Scale(localPos, currentScaleFactor));


        //Vector3 worldPos = 

        //Vector3 scaledPos = Vector3.Scale(worldPos, currentScaleFactor);

        //return scaledPos;

    }






}
