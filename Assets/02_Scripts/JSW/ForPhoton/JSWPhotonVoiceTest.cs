using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class JSWPhotonVoiceTest : MonoBehaviourPunCallbacks, IPunObservable
{
    public RawImage voiceIcon;
    public TMP_Text names;
    public bool isMaster;

    Y_UIManager y_uiManager;
    Y_TimerUI y_timerUI;

    PhotonVoiceView voiceView;
    PhotonView pv;

    public int currentPlayers;
    public int maxPlayers;

    bool isTalking = false;
    private string[] avatarSettings;
    string nickName;

    public GameObject GameStartReady;
    bool otherHasStart;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        voiceView = GetComponent<PhotonVoiceView>();
        gameObject.transform.position = PlaySceneManager.instance.playerPositions[photonView.Owner.ActorNumber - 1].position + Vector3.up * 1.4f;
        //AvatarInfo.instance.SettingAvatarInPlay(gameObject);
        y_uiManager = GameObject.Find("Canvas").GetComponent<Y_UIManager>();
        y_timerUI = GameObject.Find("Canvas").GetComponent<Y_TimerUI>();

    }

    bool isStart;
    public bool AllplayerInRoom;

    // Update is called once per frame
    void Update()
    {
        currentPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;

        if (currentPlayers == maxPlayers)
        {
            AllplayerInRoom = true;
        }

        if (PhotonNetwork.IsMasterClient && pv.IsMine && currentPlayers == maxPlayers && !isStart)
        {
            isMaster = true;
            isStart = true;
        }


        if (pv.IsMine)
        {
            // 현재 말을 하고 있다면 보이스 아이콘을 활성화한다.
            voiceIcon.gameObject.SetActive(voiceView.IsRecording);
            GameStartReady.SetActive(y_timerUI.hasStart); // Pose에서 받아온 값
        }
        else
        {
            voiceIcon.gameObject.SetActive(isTalking);
            GameStartReady.SetActive(otherHasStart); // 에서 받아온 값
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 만일, 데이터를 서버에 전송(Photonview.IsMine == true)하는 상태라면...
        if (stream.IsWriting && voiceView != null)
        {
            stream.SendNext(voiceView.IsRecording);
            stream.SendNext(y_timerUI.hasStart);
        }
        // 그렇지 않고, 만일 데이터를 서버로부터 읽어오는 상태라면...
        else if (stream.IsReading)
        {
            isTalking = (bool)stream.ReceiveNext();
            otherHasStart = (bool)stream.ReceiveNext();
        }
    }

    public void SettingAvatar_RPC(string[] avatarsetting, string nickname)
    {
        pv = GetComponent<PhotonView>();
        pv.RPC(nameof(SettingAvatar), RpcTarget.AllBuffered, avatarsetting, nickname);
    }


    [PunRPC]
    public void SettingAvatar(string[] avatarsetting, string nickname)
    {
        for (int i = 0; i < 11; i++)
        {
            if (avatarsetting[i] != null)
            {
                gameObject.transform.GetChild(1).GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh = Resources.Load<Mesh>(avatarsetting[i]);
            }
            else
            {
                gameObject.transform.GetChild(1).GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            }
        }
        names.text = nickname;
        avatarSettings = avatarsetting;
        nickName = nickname;
    }


    public void ChooseSquateOrJump_RPC(int chooseNum)
    {
        pv = GetComponent<PhotonView>();
        pv.RPC(nameof(ChooseSquateOrJump), RpcTarget.AllBuffered, chooseNum);
    }

    [PunRPC]
    public void ChooseSquateOrJump(int chooseNum)
    {
        if (chooseNum == 1)
        {
            y_uiManager.SelectSquat2();
        }
        else if (chooseNum == 2)
        {

            y_uiManager.SelectJumpingJack2();
        }
    }

    public void AllReadyGO_RPC()
    {
        pv = GetComponent<PhotonView>();
        pv.RPC(nameof(AllReadyGo), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void AllReadyGo()
    {
        y_timerUI.allReadyGo = true;
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        if (pv.IsMine && avatarSettings != null)
        {
            pv.RPC(nameof(SettingAvatar), RpcTarget.AllBuffered, avatarSettings,nickName);
        }
    }
}
