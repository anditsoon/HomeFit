using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Pun;

public class JSWVoiceManager : MonoBehaviourPun
{

    Recorder recorder;

    // Start is called before the first frame update
    void Start()
    {
        recorder = GetComponent<Recorder>();
    }

    // Update is called once per frame
    void Update()
    {
        // 만일, M키를 누르면 음소거한다.
        if(Input.GetKeyDown(KeyCode.M))
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        }

        // photonView.Owner.ActorNumber
        // 포톤뷰의 엑터 넘버!
        // 방에 입장한 순서대로

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            recorder.TargetPlayers = new int[] { (int)KeyCode.Alpha0 - 48 };
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            recorder.TargetPlayers = new int[] { (int)KeyCode.Alpha1 - 48 };
        }
    }
}
