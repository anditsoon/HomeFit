using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConsultingUIManager : MonoBehaviour
{
    public GameObject ConsultingUI;
    public float FirstPos;

    private void Start()
    {
        FirstPos = gameObject.transform.position.y;
    }

    public void MoveMainScene()
    {
        //
        //iTween.MoveTo(ConsultingUI, iTween.Hash("islocal", false,

        //                                               "y", FirstPos,

        //                                               "time", 1.0f,

        //                                               "easetype", iTween.EaseType.easeOutBounce,

        //                                               "oncomplete", "",

        //                                               "oncompletetarget", this.gameObject

        //));
        SceneManager.LoadScene("MainScene");
    }   
}
