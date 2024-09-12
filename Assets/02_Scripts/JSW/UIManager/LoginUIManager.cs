using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class LoginUIManager : MonoBehaviour
{
    public GameObject playerNameInput;
    public GameObject playerBirthInput;
    public GameObject playerWeightInput;
    public GameObject playerHeightInput;
    public Button makeAvaterButton;

    public GameObject webView;
    public GameObject userInfoManager;

    private WebViewTest web;
    private UserInfoManager user;

    private string playerName = null;

    public static LoginUIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn(transform.GetChild(0).gameObject));
        web = webView.GetComponent<WebViewTest>();
        user = userInfoManager.GetComponent<UserInfoManager>();
    }

    public void Number1Button()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(FadeIn(transform.GetChild(1).gameObject));
    }
    public void Number2Button()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(FadeIn(transform.GetChild(2).gameObject));
    }
    public void Number3Button()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        StartCoroutine(FadeIn(transform.GetChild(3).gameObject));
    }
    public void Number4ButtonGL()
    {
        // 구글 로그인
        web.OnLoginButtonClick();
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(3).gameObject.SetActive(false);
        StartCoroutine(FadeIn(transform.GetChild(4).gameObject));
    }
    public void Number5ButtoL()
    {
        AvatarInfo.instance.NickName = playerNameInput.GetComponent<TMP_InputField>().text;
        AvatarInfo.instance.Birthday = playerBirthInput.GetComponent<TMP_InputField>().text;
        AvatarInfo.instance.Height = float.Parse(playerHeightInput.GetComponent<TMP_InputField>().text);
        AvatarInfo.instance.Weight = float.Parse(playerWeightInput.GetComponent<TMP_InputField>().text);
        transform.GetChild(4).gameObject.SetActive(false);
        StartCoroutine(FadeIn(transform.GetChild(5).gameObject));
    }

    public void Number6Button()
    {

        transform.GetChild(5).gameObject.SetActive(false);
        StartCoroutine(FadeIn(transform.GetChild(6).gameObject));
    }
    public void Number7Button()
    {
        transform.GetChild(6).gameObject.SetActive(false);
        StartCoroutine(FadeIn(transform.GetChild(7).gameObject));
    }
    public void Number8Button()
    {
        SceneManager.LoadScene("AvatarScene");
        //next Scene
        //print("다음 씬 꾸미기 씬");
        //민제_씬 이동 코드는 UserInfoManager의 SendUserInfo 메서드의 전송 성공시점으로 이동
    }
    IEnumerator FadeIn(GameObject Scene2)
    {
        Scene2.SetActive(true);
        float endTime = 20f;
        float startTime = 0f;
        while (startTime <= endTime)
        {
            Scene2.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(Scene2.GetComponent<CanvasGroup>().alpha, 1, startTime/endTime);
            startTime += Time.deltaTime;
            yield return null;
        }
    }
}
