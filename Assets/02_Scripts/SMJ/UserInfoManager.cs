using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UserInfoManager : MonoBehaviour
{


    public Button confirmButton;
    private const int id = 8;
    //TODO : 고정 IP 나오면 변경
    private readonly string TokenUrl = "http://192.168.0.76:8081/api/auth/kakao/login";
    private readonly string UserInfoUrl = $"http://192.168.0.76:8081/api/user/{id}";

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    void OnConfirmButtonClick()
    {
        //StartCoroutine(GetTokenAndSendUserInfo());
        GetTokenAndSendUserInfo();
    }

    public void GetTokenAndSendUserInfo()
    {
        StartCoroutine(HttpClient.Instance.Get(TokenUrl,
            onSuccess: (result) =>
            {
                string token = result;
                // 클라 상에서 playerprefab으로 저장
                
                // 2. 받은 토큰과 사용자 정보를 함께 POST로 전송
                SendUserInfo(token);
            },
            onError: (error) =>
            {
                Debug.LogError("GET 실패: " + error);
            }
         ));
    }

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
                // 씬이동
                SceneManager.LoadScene("avatarScene");
            },
            onError: (error) =>
            {
                Debug.LogError("사용자 정보 전송 실패: " + error);
            }
        ));
    }
}