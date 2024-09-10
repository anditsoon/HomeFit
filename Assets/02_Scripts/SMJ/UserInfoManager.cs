using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System;

public class UserInfoManager : MonoBehaviour
{
    //창 속성 정의
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    //현재 활성화된 창의 IntPtr을 가져오는 함수
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    public Button confirmButton;
    private const int id = 8;
    //TODO : 고정 IP 나오면 변경
    private readonly string TokenUrl = "http://192.168.0.183:8080/api/auth/kakao/login";
    private readonly string UserInfoUrl = $"http://192.168.0.183:8080/api/user/{id}";

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    void OnConfirmButtonClick()
    {
        //StartCoroutine(GetTokenAndSendUserInfo());
        GetTokenAndSendUserInfo();
    }

    /*IEnumerator GetTokenAndSendUserInfo()
    {
        // 1. GET 요청으로 토큰 받아오기
        using (UnityWebRequest tokenRequest = UnityWebRequest.Get(TokenUrl))
        {
            yield return tokenRequest.SendWebRequest();

            if (tokenRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("토큰 받기 실패: " + tokenRequest.error);
                yield break;
            }
            else
            {
                string token = tokenRequest.downloadHandler.text;
                Debug.Log("받은 토큰: " + token);
                // 클라 상에서 playerprefab으로 저장
                PlayerPrefs.SetString("jwtToken", token);
                // 2. 받은 토큰과 사용자 정보를 함께 POST로 전송
                yield return StartCoroutine(SendUserInfo(token));
            }
        }
    }*/
    public void GetTokenAndSendUserInfo()
    {
        StartCoroutine(HttpClient.Instance.Get(TokenUrl,
            onSuccess: (result) =>
            {
                string token = result;
                // 클라 상에서 playerprefab으로 저장
                PlayerPrefs.SetString("jwtToken", token);
                // 2. 받은 토큰과 사용자 정보를 함께 POST로 전송
                SendUserInfo(token);
            },
            onError: (error) =>
            {
                Debug.LogError("GET 실패: " + error);
            }
         ));
    }

    public void OnFullWindow()
    {
        //현재 활성화된 창을 최대화
        ShowWindow(GetActiveWindow(), 3);
    }

    /*IEnumerator SendUserInfo(string token)
    {
        print(token);
        WWWForm form = new WWWForm();
        form.AddField("jwtToken", token);
        form.AddField("nickname", AvatarInfo.instance.NickName);
        form.AddField("height", AvatarInfo.instance.Height.ToString());
        form.AddField("weight", AvatarInfo.instance.Weight.ToString());
        form.AddField("birthday", AvatarInfo.instance.Birthday.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(UserInfoUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("사용자 정보 전송 실패: " + www.error);
            }
            else
            {
                string result = www.downloadHandler.text;
                Debug.Log("사용자 정보 전송 성공. 서버 응답: " + result);
                // 씬이동
                OnFullWindow();
                SceneManager.LoadScene("avatarScene");
            }
        }
    }*/

    IEnumerator SendUserInfo(string token)
    {
        print(token);
        WWWForm form = new WWWForm();
        form.AddField("jwtToken", token);
        form.AddField("nickname", AvatarInfo.instance.NickName);
        form.AddField("height", AvatarInfo.instance.Height.ToString());
        form.AddField("weight", AvatarInfo.instance.Weight.ToString());
        form.AddField("birthday", AvatarInfo.instance.Birthday.ToString());

        yield return StartCoroutine(HttpClient.Instance.Post(UserInfoUrl, form,
            onSuccess: (result) =>
            {
                Debug.Log("사용자 정보 전송 성공. 서버 응답: " + result);
                // 씬이동
                OnFullWindow();
                SceneManager.LoadScene("avatarScene");
            },
            onError: (error) =>
            {
                Debug.LogError("사용자 정보 전송 실패: " + error);
            }
        ));
    }
}