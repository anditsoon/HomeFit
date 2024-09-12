using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConsultingUIManager : MonoBehaviour
{
    public GameObject ConsultingUI;

    public void MoveMainScene()
    {
        //
        iTween.MoveTo(ConsultingUI, iTween.Hash("islocal", false,

                                                       "y", -540,

                                                       "time", 1.0f,

                                                       "easetype", iTween.EaseType.easeOutBounce,

                                                       "oncomplete", "",

                                                       "oncompletetarget", this.gameObject

        ));
    }   

}
