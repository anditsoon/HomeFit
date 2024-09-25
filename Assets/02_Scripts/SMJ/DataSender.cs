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
        public LocalTime startTime;
        public LocalTime endTime;
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
        string timeStartString = PlayerPrefs.GetString("startTime", "00:00:00");
        string timeEndString = PlayerPrefs.GetString("endTime", "00:00:00");

        string[] timeParts = timeStartString.Split(':');
        string[] timePartsEnd = timeEndString.Split(':');

        int _hour = 0, _minute = 0, _second = 0;

        if (timeParts.Length == 3)
        {
            int.TryParse(timeParts[0], out _hour);
            int.TryParse(timeParts[1], out _minute);
            int.TryParse(timeParts[2], out _second);
        }
        LocalTime localStart = new LocalTime
        {
            hour = _hour,
            minute = _minute,
            second = _second,
            nano = 0
        };
        if (timePartsEnd.Length == 3)
        {
            int.TryParse(timePartsEnd[0], out _hour);
            int.TryParse(timePartsEnd[1], out _minute);
            int.TryParse(timePartsEnd[2], out _second);
        }
        LocalTime localEnd = new LocalTime
        {
            hour = _hour,
            minute = _minute,
            second = _second,
            nano = 0
        };

        ExerciseData data = new ExerciseData
        {
            date = PlayerPrefs.GetString("date", DateTime.Now.ToString("yyyy-MM-dd")),
            caloriesBurned = (double) PlayerPrefs.GetFloat("caloriesBurned", 0f),
            exerciseCount = PlayerPrefs.GetInt("exerciseCount"),
            startTime = localStart,
            endTime = localEnd,
            userId = PlayerPrefs.GetInt("userId"),
            exerciseId = PlayerPrefs.GetInt("exerciseId")
        };

        StartCoroutine(PostData(data));
    }

    private IEnumerator PostData(ExerciseData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest(SERVER_URL, "POST"))
        {
            string jwtToken = PlayerPrefs.GetString("jwtToken");
            www.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.certificateHandler = new BypassCertificate();

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data sent successfully");
            }
            else
            {
                Debug.LogError("Error sending data: " + www.error);
            }
        }
    }
}