using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Y_WebcamStream : MonoBehaviour
{
    public RawImage targetRawImage; // Inspector에서 Quad의 Renderer를 할당할 수 있게 설정
    //public LineRenderer lineRenderer;  // 선을 그릴 LineRenderer

    private WebCamTexture webcamTexture;
    private Texture2D texture2D;
    public string serverUrl = "http://localhost:8764"; // Python FastAPI 서버 URL

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
            byte[] imageBytes = texture2D.EncodeToPNG();

            // 서버로 이미지 전송
            StartCoroutine(UploadFrame(imageBytes));
        }
    }

    IEnumerator UploadFrame(byte[] imageBytes)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("webcam_frame", imageBytes, "frame.png", "image/png");

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // 서버에서 신체 랜드마크 데이터를 받아 처리
                string json = www.downloadHandler.text;
                Debug.Log("Received JSON: " + json);

                // JSON 데이터를 파싱하여 신체 랜드마크를 시각화
                // ProcessLandmarkData(json);
            }
        }
    }

    //// 신체 랜드마크 데이터를 시각화하는 부분 (선으로 연결)
    //void ProcessLandmarkData(string json)
    //{
    //    // JSON 데이터를 객체로 변환
    //    BodyLandmarks landmarks = JsonUtility.FromJson<BodyLandmarks>(json);

    //    // 신체 랜드마크 사이를 선으로 연결
    //    DrawBodyConnections(landmarks);
    //}

    //// 신체 관절을 선으로 연결하는 함수
    //void DrawBodyConnections(BodyLandmarks landmarks)
    //{
    //    List<Vector3> points = new List<Vector3>();

    //    // 각 랜드마크의 좌표를 리스트에 추가
    //    foreach (var landmark in landmarks.body_landmarks)
    //    {
    //        Vector3 position = new Vector3(landmark.x, landmark.y, landmark.z);
    //        points.Add(position);
    //    }

    //    // LineRenderer로 관절 간의 선을 그립니다.
    //    lineRenderer.positionCount = points.Count;
    //    lineRenderer.SetPositions(points.ToArray());
    //}
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
