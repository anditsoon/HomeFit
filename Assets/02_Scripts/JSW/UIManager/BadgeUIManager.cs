using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BadgeUIManager : MonoBehaviour
{
    public GameObject BadgeUI;

    
    public void LoadMainScene()
    {
        //
        iTween.MoveTo(BadgeUI, iTween.Hash("islocal", false,

                                                       "x", -960,

                                                       "time", 1.0f,

                                                       "easetype", iTween.EaseType.easeOutBounce,

                                                       "oncomplete", "",

                                                       "oncompletetarget", this.gameObject

        ));
        //SceneManager.LoadScene("MainScene");
    }
}
