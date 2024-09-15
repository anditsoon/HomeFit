using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ProfileUIManager : MonoBehaviour
{
    public Camera renderCamera;
    public RawImage chaRawImage;
    public GameObject player;
    public TMP_Text nickNameText;
    public GameObject CalenderUI;
    public GameObject Profile;
    public GameObject Calender;

    Animator anim;
 
    // Start is called before the first frame update
    void Start()
    {
        AvatarInfo.instance.SettingAvatar();
        anim = player.GetComponent<Animator>();
        anim.CrossFade("Idle", 0f);
        SetProfilePic();
        nickNameText.text = AvatarInfo.instance.NickName;
        easingProfile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void GameLobbyScene()
    {
        SceneManager.LoadScene("GameLobbyScene");
    }

    public void CalenderReset()
    {
        for (int i =0;i < CalenderUI.transform.childCount;i++)
        {
            CalenderUI.transform.GetChild(i).GetComponent<TMP_Text>().fontStyle = FontStyles.Normal; 
        }
    }

    void SetProfilePic()
    {
        RenderTexture renderTexture = new RenderTexture(256, 256, 16);
        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();
        chaRawImage.texture = renderTexture;
        renderCamera.targetTexture = new RenderTexture(256, 256, 16);
    }

    void easingProfile()
    {
        iTween.ScaleTo(Profile, iTween.Hash("scale", new Vector3(1,1,1),

                                                       "time", 0.5f,

                                                       "easetype", iTween.EaseType.easeOutBounce

        ));
    }
}

//