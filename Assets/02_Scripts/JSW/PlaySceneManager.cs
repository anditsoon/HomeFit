using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    void Start()
    {
        AvatarInfo.instance.SettingAvatarInPlay();
    }

    void Update()
    {
        
    }

    public void MoveMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
