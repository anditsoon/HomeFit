using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Y_ShowTrackingList : MonoBehaviour
{

    public WebSocketPoseHandler conn;
    public List<Transform> bones;
    public Y_MeasureModelSize measureModelSize;
    public Vector3 locationOffset;
    Vector3 scaleFactor;
  

    // Start is called before the first frame update
    void Start()
    {
        conn = GameObject.Find("UDPConnector").GetComponent<WebSocketPoseHandler>();
        measureModelSize = GetComponent<Y_MeasureModelSize>();
        locationOffset = transform.position;

        bones = new List<Transform>();

        Transform root = GameObject.Find("root").transform;
        if (root == null)
        {
            Debug.LogError("Root bone not found!");
            return;
        }

        // Initialize the list with 33 null entries
        for (int i = 0; i < 33; i++)
        {
            bones.Add(null);
        }

        // Assign bones according to the enum order
        bones[0] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-spine.004/DEF-spine.006"); // nose (approximation)
        // 1-10 => null 값으로 유지

        bones[11] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.L/DEF-upper_arm.L"); // left_shoulder
        bones[12] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.R/DEF-upper_arm.R"); // right_shoulder
        bones[13] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.L/DEF-upper_arm.L/DEF-forearm.L"); // left_elbow
        bones[14] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.R/DEF-upper_arm.R/DEF-forearm.R"); // right_elbow
        bones[15] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.L/DEF-upper_arm.L/DEF-forearm.L/DEF-hand.L"); // left_wrist
        bones[16] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.R/DEF-upper_arm.R/DEF-forearm.R/DEF-hand.R"); // right_wrist

        bones[17] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.L/DEF-upper_arm.L/DEF-forearm.L/DEF-hand.L/DEF-f_pinky.01.L"); // left_pinky
        bones[18] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.R/DEF-upper_arm.R/DEF-forearm.R/DEF-hand.R/DEF-f_pinky.01.R"); // right_pinky
        bones[19] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.L/DEF-upper_arm.L/DEF-forearm.L/DEF-hand.L/DEF-f_index.01.L"); // left_index
        bones[20] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.R/DEF-upper_arm.R/DEF-forearm.R/DEF-hand.R/DEF-f_index.01.R"); // right_index
        bones[21] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.L/DEF-upper_arm.L/DEF-forearm.L/DEF-hand.L/DEF-thumb.01.L"); // left_thumb
        bones[22] = FindBone(root, "DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.R/DEF-upper_arm.R/DEF-forearm.R/DEF-hand.R/DEF-thumb.01.R"); // right_thumb

        bones[23] = FindBone(root, "DEF-spine/DEF-thigh.L"); // left_hip
        bones[24] = FindBone(root, "DEF-spine/DEF-thigh.R"); // right_hip
        bones[25] = FindBone(root, "DEF-spine/DEF-thigh.L/DEF-shin.L"); // left_knee
        bones[26] = FindBone(root, "DEF-spine/DEF-thigh.R/DEF-shin.R"); // right_knee
        bones[27] = FindBone(root, "DEF-spine/DEF-thigh.L/DEF-shin.L/DEF-foot.L"); // left_ankle
        bones[28] = FindBone(root, "DEF-spine/DEF-thigh.R/DEF-shin.R/DEF-foot.R"); // right_ankle

        // 29-30 => null 값으로 유지

        bones[31] = FindBone(root, "DEF-spine/DEF-thigh.L/DEF-shin.L/DEF-foot.L/DEF-toe.L"); // left_foot_index
        bones[32] = FindBone(root, "DEF-spine/DEF-thigh.R/DEF-shin.R/DEF-foot.R/DEF-toe.R"); // right_foot_index

        
    }

    Transform FindBone(Transform parent, string bonePath)
    {
        Transform bone = parent.Find(bonePath);
        if (bone == null)
        {
            Debug.LogWarning($"Bone not found: {bonePath}");
        }
        return bone;
    }

    // Update is called once per frame
    void Update()
    {

        scaleFactor = measureModelSize.modelSize;
        // 만일, 트래킹된 데이터가 있다면
        if (conn.latestPoseList.landmarkList.Count > 0) //// < ?
        {
            // 트래킹된 벡터 값을 모든 자식 오브젝트의 로컬 위치 값으로 전달한다
            for (int i = 0; i < conn.latestPoseList.landmarkList.Count; i++)
            {
                if(i == 0 || (i >= 11 && i <= 28) || (i >= 31 && i <=32))
                {
                    bones[i].localPosition = new Vector3 (
                        conn.latestPoseList.landmarkList[i].x * scaleFactor.x, 
                        conn.latestPoseList.landmarkList[i].y * scaleFactor.y, 
                        conn.latestPoseList.landmarkList[i].z * scaleFactor.z) 
                        + locationOffset;

                }
            }
        }
    }

    
}
