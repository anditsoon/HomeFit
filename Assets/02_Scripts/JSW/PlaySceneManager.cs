using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Voice.PUN;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlaySceneManager : MonoBehaviourPunCallbacks
{
    public GameObject myPlayer;
    public Transform[] playerPositions;
    public UDPPoseHandler UDPPoseH;

    public static PlaySceneManager instance;

    bool leftRoom;
    bool disconnect;

    private Hashtable CP;

    private int currentRoomId;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // 씬 전환이 되도 게임 오브젝트를 파괴하고 싶지않다.
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MoveMainScene()
    {
        StartCoroutine(LeaveRoom());
    }

    private IEnumerator LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            currentRoomId = (int)PhotonNetwork.CurrentRoom.CustomProperties["RoomId"];
            Debug.Log($"Preparing to leave room: {currentRoomId}");

            yield return new WaitForSeconds(0.3f);

            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.Log("Not in a room");
            SceneManager.LoadScene(1);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom called");
        base.OnLeftRoom();

        SceneManager.LoadScene(1);
    }

    IEnumerator LeftRoom()
    {
        while (true)
        {
            if (disconnect && leftRoom) break;
            yield return null;
        }
        disconnect = false;
        leftRoom = false;
        SceneManager.LoadScene(1);
    }

    void Start()
    {
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        StartCoroutine(SpawnPlayer());

        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.SendRate = 30;

        GameObject playerListUI = GameObject.Find("text_PlayerList");
        CP = PhotonNetwork.LocalPlayer.CustomProperties;

        JSWSoundManager.Get().PlayBgmSound(JSWSoundManager.EBgmType.BGM_Play);
    }

    IEnumerator SpawnPlayer()
    {
        yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });

        Vector2 randomPos = Random.insideUnitCircle * 5.0f;
        Vector3 initPosition = new Vector3(randomPos.x, 0, randomPos.y);

        myPlayer = PhotonNetwork.Instantiate("Player", initPosition, Quaternion.identity);
        UDPPoseH.displayWebCam = myPlayer.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(1).GetComponent<RawImage>();
        string[] avatarsetting = new string[11];
        avatarsetting[0] = (string)CP["Backpack"];
        avatarsetting[1] = (string)CP["Body"];
        avatarsetting[2] = (string)CP["Eyebrow"];
        avatarsetting[3] = (string)CP["Glasses"];
        avatarsetting[4] = (string)CP["Glove"];
        avatarsetting[5] = (string)CP["Hair"];
        avatarsetting[6] = (string)CP["Hat"];
        avatarsetting[7] = (string)CP["Mustache"];
        avatarsetting[8] = (string)CP["Outerwear"];
        avatarsetting[9] = (string)CP["Pants"];
        avatarsetting[10] = (string)CP["Shoe"];
        myPlayer.GetComponent<JSWPhotonVoiceTest>().SettingAvatar_RPC(avatarsetting, (string)CP["NickName"]);
    }
}