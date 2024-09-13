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
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    public GameObject user;

    private UserInfoManager userManager;

    private string playerName = null;

    public static LoginUIManager instance;

    public bool isLogin = false;

    private Coroutine curCor;

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
        userManager = user.GetComponent<UserInfoManager>();
        curCor = StartCoroutine(FadeIn(transform.GetChild(0).gameObject));
        userManager.OnLoginStatusChanged += HandleLoginStatusChanged;
    }
    void OnDestroy()
    {
        // 이벤트 구독 해제
        userManager.OnLoginStatusChanged -= HandleLoginStatusChanged;
    }

    void HandleLoginStatusChanged(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            isLogin = true;
        }
        else
        {
            isLogin = false;
        }
    }

    public void Number1Button()
    {
        ChangePanel(0);
    }
    public void Number2Button()
    {
        ChangePanel(1);
    }
    public void Number3Button()
    {
        ChangePanel(2);
    }
    public void Number4ButtonGL()
    {
        ChangePanel(3);
    }

    public void Login()
    {
        StartCoroutine(RoopCheck());
    }

    IEnumerator RoopCheck()
    {
        while (true)
        {
            if (isLogin == true)
            {
                ChangePanel(4);
                break;
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    public void Number5ButtoL()
    {
        AvatarInfo.instance.NickName = playerNameInput.GetComponent<TMP_InputField>().text;
        AvatarInfo.instance.Birthday = playerBirthInput.GetComponent<TMP_InputField>().text;
        AvatarInfo.instance.Height = float.Parse(playerHeightInput.GetComponent<TMP_InputField>().text);
        AvatarInfo.instance.Weight = float.Parse(playerWeightInput.GetComponent<TMP_InputField>().text);
        ChangePanel(5);
    }

    public void Number6Button()
    {
        ChangePanel(6);
    }
    public void Number7Button()
    {
        ChangePanel(7);
    }
    public void Number8Button()
    {
        //SceneManager.LoadScene("AvatarScene");
        //next Scene
        //print("다음 씬 꾸미기 씬");
    }

    public void ChangePanel(int idx)
    {
        if (curCor != null)
        {
            StopCoroutine(curCor);
        }
        transform.GetChild(idx).gameObject.SetActive(false);
        StartCoroutine(FadeIn(transform.GetChild(idx + 1).gameObject));
    }
    IEnumerator FadeIn(GameObject Scene2)
    {
        Scene2.SetActive(true);
        Scene2.GetComponent<CanvasGroup>().alpha = 0;
        float endTime = 20f;
        float startTime = 0f;
        while (startTime <= endTime)
        {
            Scene2.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(Scene2.GetComponent<CanvasGroup>().alpha, 1, startTime/endTime);
            if (Scene2.GetComponent<CanvasGroup>().alpha > 0.99f)
            {
                Scene2.GetComponent<CanvasGroup>().alpha = 1;
            }
            startTime += Time.deltaTime;
            yield return null;
        }
    }
}
