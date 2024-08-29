using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class MediaPipeRequester : MonoBehaviour
{
    public string serverUrl = "http://your-server-url.com/pose-data";
    public float updateInterval = 0.1f;

    [System.Serializable]
    public class BodyPartMapping
    {
        public string partName;
        public Transform partTransform;
    }

    [SerializeField]
    private List<BodyPartMapping> bodyPartMappings = new List<BodyPartMapping>();

    private Dictionary<string, Transform> bodyParts = new Dictionary<string, Transform>();

    void Start()
    {
        InitializeBodyParts();
        StartCoroutine(UpdatePoseRoutine());
    }

    void InitializeBodyParts()
    {
        bodyParts.Clear();
        foreach (var mapping in bodyPartMappings)
        {
            if (mapping.partTransform != null)
            {
                bodyParts[mapping.partName] = mapping.partTransform;
            }
            else
            {
                Debug.LogWarning($"Body part '{mapping.partName}' has no assigned transform.");
            }
        }
    }

    IEnumerator UpdatePoseRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(GetPoseData());
            yield return new WaitForSeconds(updateInterval);
        }
    }

    IEnumerator GetPoseData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(serverUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = webRequest.downloadHandler.text;
                UpdatePoseFromJson(jsonResult);
            }
            else
            {
                Debug.Log("Error: " + webRequest.error);
            }
        }
    }

    void UpdatePoseFromJson(string jsonData)
    {
        // JSON 파싱 (SimpleJSON 등의 라이브러리 사용)
        var N = JSON.Parse(jsonData);

        foreach (var bodyPart in bodyParts)
        {
            if (N[bodyPart.Key] != null)
            {
                Vector3 position = new Vector3(
                    N[bodyPart.Key]["x"].AsFloat,
                    N[bodyPart.Key]["y"].AsFloat,
                    N[bodyPart.Key]["z"].AsFloat
                );
                bodyPart.Value.localPosition = position;
            }
        }
    }
}
