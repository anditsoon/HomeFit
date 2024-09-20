using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    public GameObject player;
    public TMP_Text nickNameText;
    public GameObject mainBackground;
    public GameObject CamPos;
    public Image exerciseGauge;
    public TMP_Text exerciseGaugeText;
    public GameObject backGround;

    public GameObject badgeScene;
    public GameObject cousultingScene;

    float exerciseNowNum = 0;
    float exerciseNum = 50;


    Animator anim;
    bool moveUpDown;
    // Start is called before the first frame update
    void Start()
    {
        anim = player.GetComponent<Animator>();
        anim.CrossFade("Dance", 0f);
        AvatarInfo.instance.SettingAvatar();
        nickNameText.text = AvatarInfo.instance.NickName;
        //easingMove(backGround);

    }

    // Update is called once per frame
    void Update()
    {
        UpDownImage();
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, CamPos.transform.position, Time.deltaTime * 10);
        exerciseNowNum = Mathf.Lerp(exerciseNowNum, exerciseNum, Time.deltaTime);
        exerciseGaugeText.text = ((int)Mathf.Ceil(exerciseNowNum)) + "%";
        exerciseGauge.fillAmount = exerciseNowNum / 100;
    }

    public void MoveAvatarScene()
    {
        SceneManager.LoadScene("avatarScene");
    }
    public void MoveProfileScene()
    {
        SceneManager.LoadScene("ProfileScene");
    }

    public void MovePlayScene()
    {
        //SceneManager.LoadScene("Y_ProtoScene 1");
        SceneManager.LoadScene("J_AlphaScene_Photon");
    }

    public void MoveBadgeScene()
    {
        //easingMoveBadge(badgeScene);
        SceneManager.LoadScene("BadgeScene");
    }

    public void MoveGameLobbyScene()
    {
        SceneManager.LoadScene("GameLobbyScene");
    }

    public void MoveConsultingScene()
    {
        //easingMoveConsulting(cousultingScene);
        SceneManager.LoadScene("ConsultingScene");
    }

    void UpDownImage()
    {
        if (mainBackground.transform.position.y < -8)
        {
            moveUpDown = true;
        }
        else if (mainBackground.transform.position.y > 0)
        {
            moveUpDown = false;
        }

        if (moveUpDown)
        {
            mainBackground.transform.position += Vector3.up * 0.05f;
        }
        else if (!moveUpDown)
        {
            mainBackground.transform.position += -1 * Vector3.up * 0.05f;
        }
    }

    void easingMove(GameObject gameObjectUI)
    {
        iTween.MoveTo(gameObjectUI, iTween.Hash("islocal", false,

                                                       "x", 1200,

                                                       "time", 1.0f,

                                                       "easetype", iTween.EaseType.easeOutBounce,

                                                       "oncomplete", "",

                                                       "oncompletetarget", this.gameObject

        ));

    }

    void easingMoveBadge(GameObject gameObjectUI)
    {
        iTween.MoveTo(gameObjectUI, iTween.Hash("islocal", false,

                                                       "x", 0,

                                                       "time", 1.0f,

                                                       "easetype", iTween.EaseType.easeOutBounce,

                                                       "oncomplete", "",

                                                       "oncompletetarget", this.gameObject

        ));

    }

    void easingMoveConsulting(GameObject gameObjectUI)
    {
        iTween.MoveTo(gameObjectUI, iTween.Hash("islocal", false,

                                                       "y", 0,

                                                       "time", 1.0f,

                                                       "easetype", iTween.EaseType.easeOutBounce,

                                                       "oncomplete", "",

                                                       "oncompletetarget", this.gameObject

        ));

    }
}
