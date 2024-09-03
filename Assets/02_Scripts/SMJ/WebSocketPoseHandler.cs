using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

[System.Serializable]
public struct PoseData
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public struct PoseList
{
    public List<PoseData> landmarkList;
}

public enum PoseName
{
    nose,
    left_eye_inner,
    left_eye,
    left_eye_outer,
    right_eye_inner,
    right_eye,
    right_eye_outer,
    left_ear,
    right_ear,
    mouth_left,
    mouth_right,
    left_shoulder,
    right_shoulder,
    left_elbow,
    right_elbow,
    left_wrist,
    right_wrist,
    left_pinky,
    right_pinky,
    left_index,
    right_index,
    left_thumb,
    right_thumb,
    left_hip,
    right_hip,
    left_knee,
    right_knee,
    left_ankle,
    right_ankle,
    left_heel,
    right_heel,
    left_foot_index,
    right_foot_index
}

public class WebSocketPoseHandler : MonoBehaviour
{
    [SerializeField] private string serverUrl = "ws://localhost:8764";
    private ClientWebSocket webSocket;
    private bool isRunning = false;

    public PoseList latestPoseList;

    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        webSocket = new ClientWebSocket();
        Uri serverUri = new Uri(serverUrl);

        try
        {
            webSocket.ConnectAsync(serverUri, CancellationToken.None).Wait();
            Debug.Log("Connected to server");
            isRunning = true;
            StartCoroutine(ReceiveLoop());
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error connecting to server: {ex.Message}");
        }
    }

    private IEnumerator ReceiveLoop()
    {
        var buffer = new byte[1024 * 4];
        while (isRunning && webSocket.State == WebSocketState.Open)
        {
            var segment = new ArraySegment<byte>(buffer);
            WebSocketReceiveResult result = null;

            try
            {
                result = webSocket.ReceiveAsync(segment, CancellationToken.None).Result;

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    ProcessMessage(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    isRunning = false;
                    webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).Wait();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error receiving message: {ex.Message}");
                isRunning = false;
            }

            yield return null;
        }
    }

    private void ProcessMessage(string message)
    {
        try
        {
            Debug.Log($"Received message: {message}");
            if (message.StartsWith("[") && message.EndsWith("]"))
            {
                message = "{\"landmarkList\":" + message + "}";
                latestPoseList = JsonUtility.FromJson<PoseList>(message);
                Debug.Log($"Processed PoseList with {latestPoseList.landmarkList.Count} landmarks");
            }
            else
            {
                Debug.LogWarning($"Unrecognized message format: {message}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to process message: {e.Message}");
        }
    }

    private void OnApplicationQuit()
    {
        isRunning = false;
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application quitting", CancellationToken.None).Wait();
        }
    }
}