using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
    [SerializeField] private string serverUrl = "ws://localhost:8080";
    private ClientWebSocket webSocket;
    private CancellationTokenSource cts;

    public PoseList latestPoseList;

    private async void Start()
    {
        cts = new CancellationTokenSource();
        await ConnectToServerAsync();
    }

    private async Task ConnectToServerAsync()
    {
        webSocket = new ClientWebSocket();
        Uri serverUri = new Uri(serverUrl);

        try
        {
            await webSocket.ConnectAsync(serverUri, cts.Token);
            Debug.Log("Connected to server");
            _ = ReceiveLoop();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error connecting to server: {ex.Message}");
        }
    }

    private async Task ReceiveLoop()
    {
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    ProcessMessage(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cts.Token);
                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error receiving message: {ex.Message}");
                break;
            }
        }
    }

    private void ProcessMessage(string message)
    {
        try
        {
            // 디버그를 위해 받은 메시지 출력
            Debug.Log($"Received message: {message}");

            // 메시지가 단일 PoseData인 경우
            if (message.StartsWith("{") && message.EndsWith("}"))
            {
                PoseData poseData = JsonUtility.FromJson<PoseData>(message);
                if (latestPoseList.landmarkList == null)
                {
                    latestPoseList.landmarkList = new List<PoseData>();
                }
                latestPoseList.landmarkList.Clear();
                latestPoseList.landmarkList.Add(poseData);
                Debug.Log($"Processed single PoseData: x={poseData.x}, y={poseData.y}, z={poseData.z}");
            }
            // 메시지가 PoseList인 경우
            else if (message.StartsWith("[") && message.EndsWith("]"))
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

    public PoseData GetPoseData(PoseName poseName)
    {
        if (latestPoseList.landmarkList != null)
        {
            int index = (int)poseName;
            if (index < latestPoseList.landmarkList.Count)
            {
                return latestPoseList.landmarkList[index];
            }
        }
        return new PoseData();
    }

    private async void OnApplicationQuit()
    {
        cts.Cancel();
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application quitting", CancellationToken.None);
        }
    }
}