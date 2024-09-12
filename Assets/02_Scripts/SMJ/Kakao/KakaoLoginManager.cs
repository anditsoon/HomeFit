using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class KakaoLoginManager : MonoBehaviour
{
    private const string LOGIN_URL_ENDPOINT = "https://192.168.0.76:8081/api/auth/kakao/login";
    private const string JWT_TOKEN_KEY = "token";

    [Serializable]
    private class LoginUrlResponse
    {
        public int status;
        public string message;
        public string data;
    }

    public void StartKakaoLogin()
    {
        StartCoroutine(GetKakaoLoginUrl());
    }

    private IEnumerator GetKakaoLoginUrl()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(LOGIN_URL_ENDPOINT))
        {
            www.certificateHandler = new AcceptAllCertificatesSignedWithAnyCA();
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error getting Kakao login URL: " + www.error);
            }
            else
            {
                try
                {
                    string jsonResponse = www.downloadHandler.text;
                    LoginUrlResponse response = JsonUtility.FromJson<LoginUrlResponse>(jsonResponse);

                    if (response.status == 200 && response.message == "loginUrl")
                    {
                        string kakaoLoginUrl = response.data;
                        string url = "https://192.168.0.76:8081/oauth2/authorization/kakao";
                        Application.OpenURL(url);
                        Debug.Log("Opened Kakao login URL: " + url);
                    }
                    else
                    {
                        Debug.LogError("Unexpected response format or status");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error parsing JSON response: " + e.Message);
                }
            }
        }
    }

    private void SaveJwtToken(string token)
    {
        PlayerPrefs.SetString(JWT_TOKEN_KEY, token);
        PlayerPrefs.Save();
    }

    public string GetJwtToken()
    {
        return PlayerPrefs.GetString(JWT_TOKEN_KEY, string.Empty);
    }

    public IEnumerator CallAuthenticatedApi(string apiUrl)
    {
        string token = GetJwtToken();
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("No JWT token found. Please login first.");
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            www.SetRequestHeader("Authorization", "Bearer " + token);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error calling API: " + www.error);
            }
            else
            {
                Debug.Log("API response: " + www.downloadHandler.text);
            }
        }
    }

    private const int id = 2;
    private readonly string UserInfoUrl = $"http://192.168.0.76:8081/api/user/{id}";

    IEnumerator SendUserInfo(string token)
    {
        print(token);
        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("nickname", AvatarInfo.instance.NickName);
        form.AddField("height", AvatarInfo.instance.Height.ToString());
        form.AddField("weight", AvatarInfo.instance.Weight.ToString());
        form.AddField("birthday", AvatarInfo.instance.Birthday.ToString());

        yield return StartCoroutine(HttpClient.Instance.Post(UserInfoUrl, form,
            onSuccess: (result) =>
            {
                Debug.Log("사용자 정보 전송 성공. 서버 응답: " + result);
                SceneManager.LoadScene("avatarScene");
            },
            onError: (error) =>
            {
                Debug.LogError("사용자 정보 전송 실패: " + error);
            }
        ));
    }

    public void SendMessage()
    {
        StartCoroutine(SendUserInfo(GetJwtToken()));
    }
}

public class AcceptAllCertificatesSignedWithAnyCA : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true; // 모든 인증서 수락
    }
}