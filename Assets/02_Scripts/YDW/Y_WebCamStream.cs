using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.Sockets;

public class Y_WebcamStream : MonoBehaviour
{
    public RawImage targetRawImage;
    private WebCamTexture webcamTexture;
    private Texture2D texture2D;

    private UdpClient udpClient;
    public string serverIp = "localhost";
    public int serverPort = 8764;

    //public string serverUrl = "http://localhost:8764"; // Python UDP 서버 URL

    void Start()
    {
        try
        {
            // 웹캠을 시작합니다.
            webcamTexture = new WebCamTexture();

            // 할당된 Renderer의 메인 텍스처에 웹캠 스트림을 적용
            targetRawImage.texture = webcamTexture;
            webcamTexture.Play();

            // 웹캠이 제대로 실행되지 않으면 오류 메시지 표시
            if (!webcamTexture.isPlaying)
            {
                Debug.LogError("Webcam is not playing. Attempting to start again.");
                webcamTexture.Play();
            }

            // 캡처된 텍스처를 2D 텍스처로 저장
            texture2D = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB24, false);

            udpClient = new UdpClient();
            udpClient.Connect(serverIp, serverPort);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Webcam initialization error: " + e.Message);
        }

        // 주기적으로 웹캠 프레임을 서버로 전송
        StartCoroutine(SendWebcamFrames());
    }

    IEnumerator SendWebcamFrames()
    {
        while (true)
        {
            // 웹캠이 실행 중인지 확인, 실행 중이 아니면 재시작
            if (!webcamTexture.isPlaying)
            {
                Debug.LogError("Webcam stopped playing. Attempting to start again.");
                webcamTexture.Play();
            }

            // 0.1초에 한 번씩 웹캠 데이터를 서버로 전송
            yield return new WaitForSeconds(0.1f);

            // 현재 웹캠 프레임을 Texture2D로 저장
            texture2D.SetPixels(webcamTexture.GetPixels());
            texture2D.Apply();

            // 이미지 데이터를 PNG로 인코딩
            byte[] imageBytes = texture2D.EncodeToJPG();

            // 서버로 이미지 전송
            SendFrameUdp(imageBytes);
        }
    }

    void SendFrameUdp(byte[] imageBytes)
    {
        try
        {
            udpClient.Send(imageBytes, imageBytes.Length);
            Debug.Log("Frame sent to server.");
        }
        catch (SocketException e)
        {
            Debug.LogError("Socket error: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }


}

// 신체 랜드마크 데이터 클래스 정의
[System.Serializable]
public class Landmark
{
    public int landmark_index;
    public float x;
    public float y;
    public float z;
    public float visibility;
}

[System.Serializable]
public class BodyLandmarks
{
    public List<Landmark> body_landmarks;
}
