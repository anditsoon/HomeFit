using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
//using UnityEditor.SearchService;

public class LoginUIManager : MonoBehaviour
{
    public GameObject playerNameInput;
    public GameObject playerBirthInput;
    public GameObject playerWeightInput;
    public GameObject playerHeightInput;
    public Button makeAvaterButton;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField usernameRegitsterInput;
    public TMP_InputField passwordRegitsterInput;

    public GameObject user;

    private UserInfoManager userManager;

    private string playerName = null;

    public static LoginUIManager instance;

    public bool isLogin = false;
    public bool isRegist = false;
    public bool isInfoChange = false;

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
        userManager.OnRegisterStatusChanged += HandleRegisterStatusChanged;
        userManager.OnUpdateInfoStatusChanged += HandleUpdateInfoStatusChanged;
    }
    void OnDestroy()
    {
        // 이벤트 구독 해제
        userManager.OnLoginStatusChanged -= HandleLoginStatusChanged;
        userManager.OnRegisterStatusChanged -= HandleRegisterStatusChanged;
        userManager.OnUpdateInfoStatusChanged -= HandleUpdateInfoStatusChanged;
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

    void HandleRegisterStatusChanged(bool isRegister)
    {
        if (isRegister)
        {
            isRegist = true;
        }
        else
        {
            isRegist = false;
        }
    }

    void HandleUpdateInfoStatusChanged(bool isInfoChanged)
    {
        if (isInfoChanged)
        {
            isInfoChange = true;
        }
        else
        {
            isInfoChange = false;
        }
    }

    public void Number1Button()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);
        ChangePanel(0);
    }
    public void Number2Button()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);
        ChangePanel(1);
    }
    public void Number3Button()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);
        ChangePanel(2);
    }
    public void Number4ButtonGL()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);
        ChangePanel(3);
    }

    public void Number4ButtonRE()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);
        transform.GetChild(3).gameObject.SetActive(false);
        ChangePanel(4);
    }

    public void Login()
    {
        
        StartCoroutine(RoopCheck());
    }

    public void Register()
    {
        
        StartCoroutine(RoopCheckRegister());
    }

    IEnumerator RoopCheck()
    {
        while (true)
        {
            if (isLogin == true)
            {
                //ChangePanel(5);
                //로그인 성공!
                //씬이동
                SceneManager.LoadScene("MainScene");
                //TODO: 아이템 동기화 로직 구현
                break;
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
                //n초 후 로그인 안되면 실패 ui
            }
        }
    }
    IEnumerator RoopCheckRegister()
    {
        while (true)
        {
            if (isRegist == true)
            {
                ChangePanel(5);
                break;
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
                //n초 후 가입 안되면 실패 ui
            }
        }
    }

    IEnumerator RoopCheckUpdateInfo()
    {
        while (true)
        {
            if (isInfoChange == true)
            {
                ChangePanel(6);
                break;
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
                //n초 후 가입 안되면 실패 ui
            }
        }
    }

    public void Number5ButtoL()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);
        AvatarInfo.instance.NickName = playerNameInput.GetComponent<TMP_InputField>().text;
        AvatarInfo.instance.Birthday = playerBirthInput.GetComponent<TMP_InputField>().text;
        AvatarInfo.instance.Height = float.Parse(playerHeightInput.GetComponent<TMP_InputField>().text);
        AvatarInfo.instance.Weight = float.Parse(playerWeightInput.GetComponent<TMP_InputField>().text);
        StartCoroutine(RoopCheckUpdateInfo());
    }

    public void Number6Button()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);
        ChangePanel(7);
    }
    public void Number7Button()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);
        ChangePanel(8);
    }
    public void Number8Button()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);
        SceneManager.LoadScene("AvatarScene");
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
