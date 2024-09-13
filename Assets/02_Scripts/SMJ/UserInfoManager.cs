using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UserInfoManager : MonoBehaviour
{
    public Button confirmButton;
    private TMP_InputField usernameInput;
    private TMP_InputField passwordInput;
    private readonly string LoginUrl = "http://125.132.216.190:12502/api/auth/login";
    private readonly string UserInfoUrl = "http://125.132.216.190:12502/api/user/";

    private bool isLoginSuccessful = false;
    private bool isWaitingForResponse = false;
    private float loginCheckInterval = 1f;
    private float loginTimeout = 10f;

    public delegate void LoginStatusChanged(bool status);
    public event LoginStatusChanged OnLoginStatusChanged;

    void Start()
    {
        usernameInput = LoginUIManager.instance.usernameInput;
        passwordInput = LoginUIManager.instance.passwordInput;
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    void OnConfirmButtonClick()
    {
        if (!isWaitingForResponse)
        {
            string username = usernameInput.text;
            string password = passwordInput.text;
            StartCoroutine(LoginAndWaitForResponse(username, password));
        }
        else
        {
            Debug.Log("이미 로그인 시도 중입니다. 잠시 기다려주세요.");
        }
    }

    IEnumerator LoginAndWaitForResponse(string username, string password)
    {
        isWaitingForResponse = true;
        SetLoginStatus(false);

        LoginAndGetUserInfo(username, password);

        float elapsedTime = 0f;
        while (!isLoginSuccessful && elapsedTime < loginTimeout)
        {
            yield return new WaitForSeconds(loginCheckInterval);
            elapsedTime += loginCheckInterval;
        }

        if (isLoginSuccessful)
        {
            Debug.Log("로그인 성공!");
            SceneManager.LoadScene("avatarScene");
        }
        else
        {
            Debug.Log("로그인 실패 또는 시간 초과");
            SetLoginStatus(false);
        }

        isWaitingForResponse = false;
    }

    void LoginAndGetUserInfo(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        StartCoroutine(HttpClient.Instance.Post(LoginUrl, form,
            onSuccess: (result) =>
            {
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(result);
                string token = response.token;
                int id = response.id;
                PlayerPrefs.SetString("UserToken", token);
                PlayerPrefs.SetInt("UserId", id);
                PlayerPrefs.Save();
                Debug.Log("로그인 성공. 토큰과 ID가 저장되었습니다.");
                SetLoginStatus(true);
            },
            onError: (error) =>
            {
                Debug.LogError("로그인 실패: " + error);
                SetLoginStatus(false);
            }
        ));
    }

    void GetUserInfo(string token, int id)
    {
        string url = UserInfoUrl + id;
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "Authorization", "Bearer " + token }
        };
        StartCoroutine(HttpClient.Instance.Get(url,
            onSuccess: (result) =>
            {
                Debug.Log("사용자 정보 조회 성공. 서버 응답: " + result);
            },
            onError: (error) =>
            {
                Debug.LogError("사용자 정보 조회 실패: " + error);
            },
            headers: headers
        ));
    }

    // 외부에서 로그인 상태를 확인할 수 있는 메서드
    public bool IsLoggedIn()
    {
        return isLoginSuccessful;
    }

    // 로그인 상태를 설정하고 이벤트를 발생시키는 메서드
    private void SetLoginStatus(bool status)
    {
        isLoginSuccessful = status;
        OnLoginStatusChanged?.Invoke(status);
    }

    // 로그아웃 메서드
    public void Logout()
    {
        PlayerPrefs.DeleteKey("UserToken");
        PlayerPrefs.DeleteKey("UserId");
        SetLoginStatus(false);
    }
}

[System.Serializable]
public class LoginResponse
{
    public string token;
    public int id;
}