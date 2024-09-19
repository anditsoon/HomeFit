using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using TMPro;

public class UserInfoManager : MonoBehaviour
{
    public Button loginButton;
    public Button registerButton;
    public Button updateInfoButton;
    private TMP_InputField usernameInput;
    private TMP_InputField passwordInput;
    private TMP_InputField nicknameInput;
    private TMP_InputField birthdayInput;
    private TMP_InputField heightInput;
    private TMP_InputField weightInput;
    private readonly string LoginUrl = "https://125.132.216.190:12502/api/login";
    private readonly string RegisterUrl = "https://125.132.216.190:12502/api/register";
    private readonly string UserInfoUrl = "https://125.132.216.190:12502/api/user/";

    private bool isLoginSuccessful = false;
    private bool isWaitingForResponse = false;
    private float responseCheckInterval = 1f;
    private float responseTimeout = 10f;

    public delegate void StatusChanged(bool status);
    public event StatusChanged OnLoginStatusChanged;
    public event StatusChanged OnRegisterStatusChanged;
    public event StatusChanged OnUpdateInfoStatusChanged;

    void Start()
    {
        usernameInput = LoginUIManager.instance.usernameInput;
        passwordInput = LoginUIManager.instance.passwordInput;
        nicknameInput = LoginUIManager.instance.playerNameInput.GetComponent<TMP_InputField>();
        birthdayInput = LoginUIManager.instance.playerBirthInput.GetComponent<TMP_InputField>();
        heightInput = LoginUIManager.instance.playerHeightInput.GetComponent<TMP_InputField>();
        weightInput = LoginUIManager.instance.playerWeightInput.GetComponent<TMP_InputField>();

        loginButton.onClick.AddListener(OnLoginButtonClick);
        registerButton.onClick.AddListener(OnRegisterButtonClick);
        updateInfoButton.onClick.AddListener(OnUpdateInfoButtonClick);

        ServicePointManager.ServerCertificateValidationCallback = TrustAllCertificates;
    }

    private bool TrustAllCertificates(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
    {
        return true;
    }

    void OnLoginButtonClick()
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

    void OnRegisterButtonClick()
    {
        if (!isWaitingForResponse)
        {
            string username = usernameInput.text;
            string password = passwordInput.text;
            StartCoroutine(RegisterAndWaitForResponse(username, password));
        }
        else
        {
            Debug.Log("이미 회원가입 시도 중입니다. 잠시 기다려주세요.");
        }
    }

    void OnUpdateInfoButtonClick()
    {
        if (!isWaitingForResponse)
        {
            string nickname = nicknameInput.text;
            string birthday = birthdayInput.text;
            string height = heightInput.text;
            string weight = weightInput.text;
            StartCoroutine(UpdateUserInfoAndWaitForResponse(nickname, birthday, height, weight));
        }
        else
        {
            Debug.Log("이미 정보 수정 시도 중입니다. 잠시 기다려주세요.");
        }
    }

    IEnumerator LoginAndWaitForResponse(string username, string password)
    {
        isWaitingForResponse = true;
        SetLoginStatus(false);

        LoginAndGetUserInfo(username, password);

        yield return StartCoroutine(WaitForResponse(() => isLoginSuccessful));

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

    IEnumerator RegisterAndWaitForResponse(string username, string password)
    {
        isWaitingForResponse = true;
        bool isRegistrationSuccessful = false;

        RegisterUser(username, password);

        yield return StartCoroutine(WaitForResponse(() => isRegistrationSuccessful));

        if (isRegistrationSuccessful)
        {
            Debug.Log("회원가입 성공!");
            SetRegisterStatus(true);
        }
        else
        {
            Debug.Log("회원가입 실패 또는 시간 초과");
            SetRegisterStatus(false);
        }

        isWaitingForResponse = false;
    }

    IEnumerator UpdateUserInfoAndWaitForResponse(string nickname, string birthday, string height, string weight)
    {
        isWaitingForResponse = true;
        bool isUpdateSuccessful = false;

        UpdateUserInfo(nickname, birthday, height, weight);

        yield return StartCoroutine(WaitForResponse(() => isUpdateSuccessful));

        if (isUpdateSuccessful)
        {
            Debug.Log("사용자 정보 수정 성공!");
            SetUpdateInfoStatus(true);
        }
        else
        {
            Debug.Log("사용자 정보 수정 실패 또는 시간 초과");
            SetUpdateInfoStatus(false);
        }

        isWaitingForResponse = false;
    }

    IEnumerator WaitForResponse(Func<bool> condition)
    {
        float elapsedTime = 0f;
        while (!condition() && elapsedTime < responseTimeout)
        {
            yield return new WaitForSeconds(responseCheckInterval);
            elapsedTime += responseCheckInterval;
        }
    }

    void LoginAndGetUserInfo(string username, string password)
    {
        string jsonData = JsonUtility.ToJson(new LoginData { userName = username, password = password });
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = new UnityWebRequest(LoginUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.certificateHandler = new BypassCertificate();

        StartCoroutine(SendRequest(www,
            onSuccess: (result) =>
            {
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(result);//무슨 문제?
                string token = response.token;
                //int id = response.id;
                PlayerPrefs.SetString("token", token);
                //PlayerPrefs.SetInt("id", id);
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

    void RegisterUser(string username, string password)
    {
        string jsonData = JsonUtility.ToJson(new RegisterData { userName = username, password = password, role = "USER" });
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = new UnityWebRequest(RegisterUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.certificateHandler = new BypassCertificate();

        StartCoroutine(SendRequest(www,
            onSuccess: (result) =>
            {
                Debug.Log("회원가입 성공. 서버 응답: " + result);
                SetRegisterStatus(true);
            },
            onError: (error) =>
            {
                Debug.LogError("회원가입 실패: " + error);
                SetRegisterStatus(false);
            }
        ));
    }

    void UpdateUserInfo(string nickname, string birthday, string height, string weight)
    {
        string token = PlayerPrefs.GetString("token");
        int id = PlayerPrefs.GetInt("id");
        string url = UserInfoUrl + id;

        string jsonData = JsonUtility.ToJson(new UpdateUserData
        {
            nickName = nickname,
            birthday = birthday,
            height = float.Parse(height),
            weight = float.Parse(weight)
        });
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = new UnityWebRequest(url, "PUT");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + token);
        www.certificateHandler = new BypassCertificate();

        StartCoroutine(SendRequest(www,
            onSuccess: (result) =>
            {
                Debug.Log("사용자 정보 수정 성공. 서버 응답: " + result);
                SetUpdateInfoStatus(true);
            },
            onError: (error) =>
            {
                Debug.LogError("사용자 정보 수정 실패: " + error);
                SetUpdateInfoStatus(false);
            }
        ));
    }

    IEnumerator SendRequest(UnityWebRequest www, Action<string> onSuccess, Action<string> onError)
    {
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            onError(www.error);
        }
        else
        {
            onSuccess(www.downloadHandler.text);
        }
    }

    public bool IsLoggedIn()
    {
        return isLoginSuccessful;
    }

    private void SetLoginStatus(bool status)
    {
        isLoginSuccessful = status;
        OnLoginStatusChanged?.Invoke(status);
    }

    private void SetRegisterStatus(bool status)
    {
        OnRegisterStatusChanged?.Invoke(status);
    }

    private void SetUpdateInfoStatus(bool status)
    {
        OnUpdateInfoStatusChanged?.Invoke(status);
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("token");
        PlayerPrefs.DeleteKey("id");
        SetLoginStatus(false);
    }
}

[System.Serializable]
public class LoginData
{
    public string userName;
    public string password;
}

[System.Serializable]
public class RegisterData
{
    public string userName;
    public string password;
    public string role;
}

[System.Serializable]
public class UpdateUserData
{
    public string nickName;
    public string birthday;
    public float height;
    public float weight;
}

[Serializable]
public class LoginResponse
{
    public string token;
    public int id;
}

public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}