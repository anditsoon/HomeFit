//using SimpleJSON;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using System.Linq;
//using System;
//using System.Runtime.InteropServices.WindowsRuntime;


//public class Y_ShowTrackingList : MonoBehaviour
//{

//    public UDPPoseHandler conn;
//    //public List<Transform> bones;
//    //public Y_MeasureModelSize measureModelSize;
//    //public Camera mainCamera;
//    //public Vector3 locationOffset;
//    //public Vector3 scaleFactor;
//    //private Vector3 initialModelScale;
//    private Vector3 currentScaleFactor = Vector3.one;
//    //private Vector3 initialModelScale;
//    public float targetHeight;
//    public float targetLegLength;
//   // public float scaleSmoothing = 0.1f; 
//    //public float minScale = 0.1f; 
//    //public float maxScale = 10f; 

//    // Rigging
//    public GameObject rigLeftArmTarget;
//    public GameObject rigLeftArmHint;
//    public GameObject rigRightArmTarget;
//    public GameObject rigRightArmHint;
//    public GameObject rigLeftLegTarget;
//    public GameObject rigLeftLegHint;
//    public GameObject rigRightLegTarget;
//    public GameObject rigRightLegHint;
//    public GameObject rigHeadAim1;
//    public GameObject rigHeadAim2;
//    public GameObject rigRightHandTarget;
//    public GameObject rigRightHandHint;
//    public GameObject rigLeftHandTarget;
//    public GameObject rigLeftHandHint;
//    public GameObject rigSpineTarget;
//    public GameObject rigSpineHint;

//    // Rigging - Left Hand
//    public GameObject rigLThumbTarget;
//    //public GameObject rigLThumbHint;
//    public GameObject rigLIndexTarget;
//    //public GameObject rigLIndexHint;
//    public GameObject rigLMiddleTarget;
//    //public GameObject rigLMiddleHint;
//    public GameObject rigLRingTarget;
//    //public GameObject rigLRingHint;
//    public GameObject rigLPinkyTarget;
//    //public GameObject rigLPinkyHint;

//    //public GameObject rigRootTarget;
//    //public GameObject rigRootHint;

//    public GameObject rigLFootTarget;
//    //public GameObject rigRFootTarget;

//    float targetScaleBody;
//    float targetScaleLeg;

//    Vector3 standardPoint;
//    Vector3 startPoint;
//    Vector3 startDir;

//    public Transform startTrans;
//    public Transform startDummyTrans;


//    // Start is called before the first frame update
//    void Start()
//    {

//        conn = GameObject.Find("UDPConnector").GetComponent<UDPPoseHandler>();
//        //measureModelSize = GetComponent<Y_MeasureModelSize>();
//        //initialModelScale = transform.localScale;
//        //locationOffset = GameObject.Find("GameObject").transform.position;
//        //mainCamera = Camera.main;
//        targetHeight = 0.78f;
//        targetLegLength = 0.78f;

//        rigLeftArmTarget = GameObject.Find("Rig_LeftArm_target");
//        rigLeftArmHint = GameObject.Find("Rig_LeftArm_hint");
//        rigRightArmTarget = GameObject.Find("Rig_RightArm_target");
//        rigRightArmHint = GameObject.Find("Rig_RightArm_hint");
//        rigLeftLegTarget = GameObject.Find("Rig_LeftLeg_target");
//        rigLeftLegHint = GameObject.Find("Rig_LeftLeg_hint");
//        rigRightLegTarget = GameObject.Find("Rig_RightLeg_target");
//        rigRightLegHint = GameObject.Find("Rig_RightLeg_hint");
//        rigHeadAim1 = GameObject.Find("Aim");
//        rigHeadAim2 = GameObject.Find("Aim2");
//        rigRightHandTarget = GameObject.Find("RigRightHand_target");
//        rigLeftHandTarget = GameObject.Find("RigLeftHand_target");
//        rigRightHandHint = GameObject.Find("RigRightHand_hint");
//        rigLeftHandHint = GameObject.Find("RigLeftHand_hint");
//        rigSpineTarget = GameObject.Find("Rig_Spine_target");
//        rigSpineHint = GameObject.Find("Rig_Spine_hint");

//        // Left Hand
//        rigLThumbTarget = GameObject.Find("Rig_LThumb_target");
//        //rigLThumbHint = GameObject.Find("Rig_LThumb_hint");
//        rigLIndexTarget = GameObject.Find("Rig_LIndex_target");
//        //rigLIndexHint = GameObject.Find("Rig_LIndex_hint");
//        rigLMiddleTarget = GameObject.Find("Rig_LMiddle_target");
//        //rigLMiddleHint = GameObject.Find("Rig_LMiddle_hint");
//        rigLRingTarget = GameObject.Find("Rig_LRing_target");
//        //rigLRingHint = GameObject.Find("Rig_LRing_hint");
//        rigLPinkyTarget = GameObject.Find("Rig_LPinky_target");
//        //rigLPinkyHint = GameObject.Find("Rig_LPinky_hint");
//        //rigRootTarget = GameObject.Find("RigRoot_target");
//        //rigRootHint = GameObject.Find("RigRoot_hint");

//        rigLFootTarget = GameObject.Find("Rig_LeftFoot_target");
//        //rigRFootTarget = GameObject.Find("Rig_RightFoot_target");

//    }




//    void Update()
//    {
//        if (conn.latestPoseList.landmarkList.Count > 0)
//        {
//            UpdateScaleFactor();
//            UpdateScaleFactorLeg();
//            UpdateRigPosition();
//        }

//        if(Input.GetKeyDown(KeyCode.Q))
//        {
//            standardPoint = getStandardPoint();
//        }

//    }


//    void UpdateScaleFactor()
//    {
        
//        float fullHeight = GetFullHeight(11, 12, 23, 24);
//        if (fullHeight > 0)
//        {
//            targetScaleBody = targetHeight / fullHeight;
//            //print("full height body : " + fullHeight);
//            //print("targetScaleBody : " + targetScaleBody);
//        }
//    }

//    void UpdateScaleFactorLeg()
//    {
//        float fullHeight = GetFullHeight(23, 24, 27, 28);
//        if (fullHeight > 0)
//        {
//            targetScaleLeg = targetLegLength / fullHeight;
//           // print("full height Leg : " + fullHeight);
//            //print("targetScaleLeg : " + targetScaleBody);
//        }
//    }


//    public float GetFullHeight(int a, int b, int c, int d)
//    {
//        if (conn.latestPoseList.landmarkList.Count == 0)
//            return 0f;

//        //float topY = (conn.latestPoseList.landmarkList[a].y + conn.latestPoseList.landmarkList[b].y) * 0.5f;
//        //float bottomY = (conn.latestPoseList.landmarkList[c].y + conn.latestPoseList.landmarkList[d].y) * 0.5f;

//        Vector3 top = (getV3FromLandmark(a) + getV3FromLandmark(b)) * 0.5f;
//        Vector3 bottom = (getV3FromLandmark(c) + getV3FromLandmark(d)) * 0.5f;

//        return Vector3.Distance(top, bottom);
//    }

//    Vector3 getV3FromLandmark(int i)
//    {
//        Vector3 localPos = new Vector3(
//                    conn.latestPoseList.landmarkList[i].x,
//                    conn.latestPoseList.landmarkList[i].y,
//                    conn.latestPoseList.landmarkList[i].z);
//        localPos.z *= 0.3f;
//        return localPos;
//    }

//    public GameObject[] cubes;

//    void UpdateRigPosition()
//    {
//        rigHeadAim1.transform.position = Vector3.Lerp(rigHeadAim1.transform.position, UpdateRigPart(9), 0.1f);
//        rigHeadAim2.transform.position = Vector3.Lerp(rigHeadAim2.transform.position, UpdateRigPart(10), 0.1f);
//        rigLeftArmHint.transform.position = Vector3.Lerp(rigLeftArmHint.transform.position, UpdateRigPart(13), 0.1f);
//        rigRightArmHint.transform.position = Vector3.Lerp(rigRightArmHint.transform.position, UpdateRigPart(14), 0.1f);
//        rigLeftArmTarget.transform.position = Vector3.Lerp(rigLeftArmTarget.transform.position, UpdateRigPart(15),0.1f);
//        rigRightArmTarget.transform.position = Vector3.Lerp(rigRightArmTarget.transform.position, UpdateRigPart(16), 0.1f);
//        rigLeftLegHint.transform.position = Vector3.Lerp(rigLeftLegHint.transform.position, UpdateRigPart(25), 0.1f);
//        rigRightLegHint.transform.position = Vector3.Lerp(rigRightLegHint.transform.position, UpdateRigPart(26), 0.1f);
//        cubes[0].transform.position = rigRightLegHint.transform.position;
//        cubes[1].transform.position = rigLeftLegHint.transform.position;
//        print(rigRightLegHint.transform.position);
//        print(rigLeftLegHint.transform.position);
//        rigLeftLegTarget.transform.position = Vector3.Lerp(rigLeftLegTarget.transform.position, UpdateRigPart(27), 0.1f);
//        rigRightLegTarget.transform.position = Vector3.Lerp(rigRightLegTarget.transform.position, UpdateRigPart(28), 0.1f);

//        rigSpineTarget.transform.position = Vector3.Lerp(rigSpineTarget.transform.position, (UpdateRigPart(11) + UpdateRigPart(12)) * 0.5f, 0.1f); 
//        rigSpineHint.transform.position = Vector3.Lerp(rigSpineHint.transform.position, (UpdateRigPart(24) + UpdateRigPart(23)) * 0.5f, 0.1f);

//        //startTrans.position = ((transform.position - (transform.up * 0.2f)) + startDir);
//        //startTrans.y = Mathf.Clamp(((transform.position - (transform.up * 0.2f)) + startDir).y, 0.3f, 5f);
//        Vector3 newPosition = transform.position - (transform.up * 0.2f) + startDir;  // 새로운 위치 계산
//        newPosition.y = Mathf.Clamp(newPosition.y, 3f, 3.8f);  // y 값 클램프 적용
//        //print(newPosition);
//        startTrans.position = newPosition;

//        //rigRootTarget.transform.position = (UpdateRigPart(24) + UpdateRigPart(23)) * 0.5f;
//        //rigRootHint.transform.position = (UpdateRigPart(24) + UpdateRigPart(23)) * 0.5f;

//        // Left Hand
//        rigLThumbTarget.transform.position = Vector3.Lerp(rigLThumbTarget.transform.position, UpdateFingerRig(21), 0.1f);
//        rigLIndexTarget.transform.position = Vector3.Lerp(rigLIndexTarget.transform.position, UpdateFingerRig(19), 0.1f);
//        rigLMiddleTarget.transform.position = Vector3.Lerp(rigLMiddleTarget.transform.position, UpdateFingerRig(19), 0.1f);
//        rigLRingTarget.transform.position = Vector3.Lerp(rigLRingTarget.transform.position, UpdateFingerRig(19), 0.1f);
//        rigLPinkyTarget.transform.position = Vector3.Lerp(rigLPinkyTarget.transform.position, UpdateFingerRig(17), 0.1f);

//        rigLFootTarget.transform.position = Vector3.Lerp(rigLFootTarget.transform.position, UpdateRigPart(31), 0.1f);
//    }

//    Vector3 startSP;

//    Vector3 getStandardPoint()
//    {
//        Vector3 vector23rd = new Vector3(
//            conn.latestPoseList.landmarkList[23].x,
//            conn.latestPoseList.landmarkList[23].y,
//            conn.latestPoseList.landmarkList[23].z
//            );

//        Vector3 vector24th = new Vector3(
//            conn.latestPoseList.landmarkList[24].x,
//            conn.latestPoseList.landmarkList[24].y,
//            conn.latestPoseList.landmarkList[24].z
//            );

//        startSP = (vector23rd + vector24th) * 0.5f;


//        //currentScaleFactor = Vector3.one * targetScaleBody;

//        //currentScaleFactor.z = currentScaleFactor.z * 0.3f;

//        return startSP;
//    }


//    // Distance 로 
//    Vector3 UpdateRigPart(int i)
//    {

//        Vector3 vector23rd = new Vector3(
//                   conn.latestPoseList.landmarkList[23].x,
//                   conn.latestPoseList.landmarkList[23].y,
//                   conn.latestPoseList.landmarkList[23].z
//                   );

//        Vector3 vector24th = new Vector3(
//            conn.latestPoseList.landmarkList[24].x,
//            conn.latestPoseList.landmarkList[24].y,
//            conn.latestPoseList.landmarkList[24].z
//            );

//        standardPoint = (vector23rd + vector24th) / 2;

//        //Vector3 localPos = new Vector3(
//        //            conn.latestPoseList.landmarkList[i].x,
//        //            conn.latestPoseList.landmarkList[i].y,
//        //            conn.latestPoseList.landmarkList[i].z);
//        Vector3 localPos = getV3FromLandmark(i);

//        localPos = (localPos - standardPoint);

//        localPos = new Vector3(localPos.x, -localPos.y, localPos.z);

//        if (i == 25 || i == 26) // if(localPos.y >)
//        {
//            if(localPos.y < 3.15)
//            {
//                currentScaleFactor = Vector3.one;
//            }
//            else
//            {
//                currentScaleFactor = Vector3.one * targetScaleLeg;
//            }
//        }
//        else
//        {
//            currentScaleFactor = Vector3.one * targetScaleBody;
//        }
        
//        //currentScaleFactor.z = currentScaleFactor.z * 0.3f;

//        if(i == 23)
//        {
//            startDir = standardPoint - startSP;

//            startDir.y = -startDir.y;

//            startDir = Vector3.Scale(startDir, currentScaleFactor);

//            startDir = transform.TransformDirection(startDir);
//        }

        

//        return transform.TransformPoint(Vector3.Scale(localPos, currentScaleFactor));

//    }

//    Vector3 UpdateFingerRig(int i)
//    {
//        Vector3 fingerVector = UpdateRigPart(i) - UpdateRigPart(15);
//        return fingerVector *= 2;
//    }






//}
