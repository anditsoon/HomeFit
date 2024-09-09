using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainUIManager : MonoBehaviour
{
    public GameObject player;
    public TMP_Text nickNameText;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = player.GetComponent<Animator>();
        anim.CrossFade("Dance",0f);
        AvatarInfo.instance.SettingAvatar();
        nickNameText.text = AvatarInfo.instance.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        SceneManager.LoadScene("Y_ProtoScene 1");
    }

}
