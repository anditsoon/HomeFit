using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
 
    // Start is called before the first frame update
    void Start()
    {
        AvatarInfo.instance.SettingAvatar();
        SetProfilePic();
        nickNameText.text = AvatarInfo.instance.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    void SetProfilePic()
    {
        RenderTexture renderTexture = new RenderTexture(256, 256, 16);
        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();
        chaRawImage.texture = renderTexture;
        renderCamera.targetTexture = new RenderTexture(256, 256, 16);
    }
}

