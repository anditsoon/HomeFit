using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using WebSocketSharp;
using SimpleJSON;

public class WebSocketHandler : MonoBehaviour
{
    public string websocketUrl = "ws://your-server-url.com/pose-data";

    [System.Serializable]
    public class BodyPartMapping
    {
        public string partName;
        public Transform partTransform;
    }

    [SerializeField]
    private List<BodyPartMapping> bodyPartMappings = new List<BodyPartMapping>();

    private Dictionary<string, Transform> bodyParts = new Dictionary<string, Transform>();
    private WebSocket ws;
    private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();

    void Start()
    {
        InitializeBodyParts();
        ConnectWebSocket();
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

    void ConnectWebSocket()
    {
        ws = new WebSocket(websocketUrl);

        ws.OnMessage += (sender, e) =>
        {
            messageQueue.Enqueue(e.Data);
        };

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connection opened");
        };

        ws.OnError += (sender, e) =>
        {
            Debug.LogError("WebSocket error: " + e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket connection closed");
        };

        ws.Connect();
    }

    void Update()
    {
        while (messageQueue.TryDequeue(out string jsonData))
        {
            UpdatePoseFromJson(jsonData);
        }
    }

    void UpdatePoseFromJson(string jsonData)
    {
        JSONNode data = JSON.Parse(jsonData);

        foreach (var bodyPart in bodyParts)
        {
            if (data[bodyPart.Key] != null)
            {
                Vector3 position = new Vector3(
                    data[bodyPart.Key]["x"].AsFloat,
                    data[bodyPart.Key]["y"].AsFloat,
                    data[bodyPart.Key]["z"].AsFloat
                );
                bodyPart.Value.localPosition = position;
            }
        }
    }

    void OnDestroy()
    {
        if (ws != null && ws.ReadyState == WebSocketState.Open)
        {
            ws.Close();
        }
    }
}