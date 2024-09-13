using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BadgeUIManager : MonoBehaviour
{
    public GameObject BadgeUI;
    public float FistX;

    public void Start()
    {
        FistX = gameObject.transform.position.x;
    }

    public void LoadMainScene()
    {
        ////
        //iTween.MoveTo(BadgeUI, iTween.Hash("islocal", false,

        //                                               "x",  FistX,

        //                                               "time", 1.0f,

        //                                               "easetype", iTween.EaseType.easeOutBounce,

        //                                               "oncomplete", "",

        //                                               "oncompletetarget", this.gameObject

        //));
        SceneManager.LoadScene("MainScene");
    }
}
