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
    public GameObject calenderUI;
    public GameObject profile;
    //public GameObject calender;

    Animator anim;
 
    // Start is called before the first frame update
    void Start()
    {
        AvatarInfo.instance.SettingAvatar();
        anim = player.GetComponent<Animator>();
        anim.CrossFade("Idle", 0f);
        SetProfilePic();
        nickNameText.text = AvatarInfo.instance.NickName;
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_SCENEMOVE2);
        easingProfile();
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
        for (int i =0;i < calenderUI.transform.childCount;i++)
        {
            calenderUI.transform.GetChild(i).GetChild(1).GetComponent<TMP_Text>().fontStyle = FontStyles.Normal;
            calenderUI.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
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
        print("1");
        iTween.ScaleTo(profile, iTween.Hash("scale", new Vector3(1,1,1),

                                                       "time", 0.5f,

                                                       "easetype", iTween.EaseType.easeOutBounce

        ));
    }
}