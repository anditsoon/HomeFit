using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ProfileGetManager : MonoBehaviour
{
    private const string SERVER_URL = "https://125.132.216.190:12502/api/exerciseLogs/user/";

    [System.Serializable]
    private class ProfileData
    {
        public string date;
        public double totalCaloriesBurned;
        public int totalExerciseCount;
    }

    private void Start()
    {
        // SSL 인증서 검증 우회 설정
        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
    }

    public void SendDataToServer(string checkDate)
    {
        print(checkDate);
        string jwtToken = PlayerPrefs.GetString("jwtToken");
        string userId = PlayerPrefs.GetString("userId");
        StartCoroutine(GetProfileCoroutine(jwtToken, userId, checkDate));
    }

    IEnumerator GetProfileCoroutine(string jwtToken, string _userId, string _checkDate)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(SERVER_URL + _userId + "/date?date=" + _checkDate))
        {
            www.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            www.certificateHandler = new BypassCertificate();
            print("전송시작");
            yield return www.SendWebRequest();
            print("전송끝");
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"프로필 정보 가져오기 실패: {www.error}");
            }
            else
            {
                string responseBody = www.downloadHandler.text;
                Debug.Log($"프로필 정보 응답: {responseBody}");

                try
                {
                    ProfileData profile = JsonUtility.FromJson<ProfileData>(responseBody);
                    AvatarInfo.instance.totalCaloriesBurned = profile.totalCaloriesBurned;
                    AvatarInfo.instance.totalExerciseCount = profile.totalExerciseCount;
                    SceneManager.LoadScene("ProfileScene");
                }
                catch (Exception e)
                {
                    Debug.LogError($"JSON 파싱 오류: {e.Message}");
                }
            }
        }
    }
}
