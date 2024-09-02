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
    public GameObject rigHeadAim1;
    public GameObject rigHeadAim2;
    public GameObject rigLeftThumb;
    public GameObject rigLeftIndex;
    public GameObject rigLeftPinky;
    public GameObject rigRightThumb;
    public GameObject rigRightIndex;
    public GameObject rigRightPinky;
    //public GameObject rigBody11;
    //public GameObject rigBody12;
    //public GameObject rigBody23;
    //public GameObject rigBody24;




    // Start is called before the first frame update
    void Start()
    {

        conn = GameObject.Find("UDPConnector").GetComponent<WebSocketPoseHandler>();
        //measureModelSize = GetComponent<Y_MeasureModelSize>();
        initialModelScale = transform.localScale;
        //locationOffset = GameObject.Find("GameObject").transform.position;
        mainCamera = Camera.main;

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
        rigLeftThumb = GameObject.Find("Rig_LHandThumb");
        rigLeftIndex = GameObject.Find("Rig_LHandIndex");
        rigLeftPinky = GameObject.Find("Rig_LHandPinky");
        rigRightThumb = GameObject.Find("Rig_RHandThumb");
        rigRightIndex = GameObject.Find("Rig_RHandIndex");
        rigRightPinky = GameObject.Find("Rig_RHandPinky");
        //rigBody11 = GameObject.Find("Rig_Body11");
        //rigBody12 = GameObject.Find("Rig_Body12");
        //rigBody23 = GameObject.Find("Rig_Body23");
        //rigBody24 = GameObject.Find("Rig_Body24");


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
        rigHeadAim1.transform.position = UpdateRigPart(9);
        rigHeadAim2.transform.position = UpdateRigPart(10);
        rigLeftThumb.transform.position = UpdateRigPart(22);
        rigLeftIndex.transform.position = UpdateRigPart(20);
        rigLeftPinky.transform.position = UpdateRigPart(18);
        rigRightThumb.transform.position = UpdateRigPart(21);
        rigRightIndex.transform.position = UpdateRigPart(19);
        rigRightPinky.transform.position = UpdateRigPart(17);
        //rigBody11.transform.position = UpdateRigPart(11);
        //rigBody12.transform.position = UpdateRigPart(12);
        //rigBody23.transform.position = UpdateRigPart(23);
        //rigBody24.transform.position = UpdateRigPart(24);


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

        Vector3 vectorMiddle = (vector23rd - vector24th) / 2 + vector24th;


        Vector3 localPos = new Vector3(
                    conn.latestPoseList.landmarkList[i].x,
                    conn.latestPoseList.landmarkList[i].y,
                    conn.latestPoseList.landmarkList[i].z);

        localPos = localPos - vectorMiddle;

        localPos = new Vector3(localPos.x, -localPos.y, localPos.z);
        localPos.z = 0;

        return transform.TransformPoint(Vector3.Scale(localPos, currentScaleFactor));


        //Vector3 worldPos = 

        //Vector3 scaledPos = Vector3.Scale(worldPos, currentScaleFactor);

        //return scaledPos;

    }






}
