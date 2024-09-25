using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using static UnityEngine.Rendering.DebugUI.Table;

public class DataSender : MonoBehaviour
{
    private const string SERVER_URL = "https://125.132.216.190:12502/api/exerciseLogs";

    [System.Serializable]
    private class LocalTime
    {
        public int hour;
        public int minute;
        public int second;
        public int nano;
    }

    [System.Serializable]
    private class ExerciseData
    {
        public string date;
        public double caloriesBurned;
        public int exerciseCount;
        public string startTime;
        public string endTime;
        public long userId;
        public long exerciseId;
    }

    private void Start()
    {
        // SSL 인증서 검증 우회 설정
        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
    }

    public void SendDataToServer()
    {
        string timeStartString = PlayerPrefs.GetString("startTime", "000000");
        string timeEndString = PlayerPrefs.GetString("endTime", "000000");

        ExerciseData data = new ExerciseData
        {
            date = PlayerPrefs.GetString("date", DateTime.Now.ToString("yyyy-MM-dd")),
            caloriesBurned = (double) PlayerPrefs.GetFloat("caloriesBurned", 0f),
            exerciseCount = PlayerPrefs.GetInt("exerciseCount"),
            startTime = timeStartString,
            endTime = timeEndString,
            userId = long.Parse(PlayerPrefs.GetString("userId")),
            exerciseId = long.Parse(PlayerPrefs.GetString("exerciseId"))
        };

        StartCoroutine(PostData(data));
    }

    private IEnumerator PostData(ExerciseData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        print(PlayerPrefs.GetInt("exerciseCount") + "결과");
        using (UnityWebRequest www = new UnityWebRequest(SERVER_URL, "POST"))
        {
            string jwtToken = PlayerPrefs.GetString("jwtToken");
            www.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.certificateHandler = new BypassCertificate();

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data sent successfully");
            }
            else
            {
                Debug.LogError("Error sending data: " + www.error);
                Debug.LogError("Response Code: " + www.responseCode);
                Debug.LogError("Response Body: " + www.downloadHandler.text);
            }
        }
    }
}