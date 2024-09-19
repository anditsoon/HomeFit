using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;
using UnityEngine.UI;
using Photon.Pun;

public class JSWPhotonVoiceTest : MonoBehaviour, IPunObservable
{
    public RawImage voiceIcon;
    PhotonVoiceView voiceView;
    PhotonView pv; 
    bool isTalking = false;

 

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        voiceView = GetComponent<PhotonVoiceView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            // 현재 말을 하고 있다면 보이스 아이콘을 활성화한다.
            voiceIcon.gameObject.SetActive(voiceView.IsRecording);
        }
        else
        {
            voiceIcon.gameObject.SetActive(isTalking);
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 만일, 데이터를 서버에 전송(Photonview.IsMine == true)하는 상태라면...
        if (stream.IsWriting)
        {
            stream.SendNext(voiceView.IsRecording);
        }
        // 그렇지 않고, 만일 데이터를 서버로부터 읽어오는 상태라면...
        else if (stream.IsReading)
        {
            isTalking = (bool)stream.ReceiveNext();
        }
    }
}
