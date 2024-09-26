using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine.Networking;
using static Gpm.Common.Util.XmlHelper;


public class JSWConnectionManager : MonoBehaviourPunCallbacks
{
    public GameObject roomPrefab;
    public Transform scrollContent;
    public GameObject[] panelList;
    bool isExit;

    List<RoomInfo> cachedRoomList = new List<RoomInfo>();
    private int currentRoomId;

    void Start()
    {
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_SCENEMOVE2);
        PhotonNetwork.JoinLobby();

        StartLogin();
    }

    private bool TrustAllCertificates(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    public void StartLogin()
    {
        if (AvatarInfo.instance.NickName.Length >= 0)
        {
            PhotonNetwork.GameVersion = "1.0.0";
            PhotonNetwork.NickName = AvatarInfo.instance.NickName;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        isExit = true;
        Debug.LogError("Disconnected from Server - " + cause);
        LobbyUIController.lobbyUI.btn_login.interactable = true;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");
        LobbyUIController.lobbyUI.ShowRoomPanel();

        string[] avatarInfo = AvatarInfo.instance.ReturnAvatarInfo();
        string nickName = AvatarInfo.instance.NickName;

        PhotonNetwork.NickName = AvatarInfo.instance.NickName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable {
            { "Backpack", avatarInfo[0]}, { "Body", avatarInfo[1] }, { "Eyebrow", avatarInfo[2] },
            { "Glasses", avatarInfo[3] }, { "Glove", avatarInfo[4] }, { "Hair", avatarInfo[5] },
            { "Hat", avatarInfo[6] }, { "Mustache", avatarInfo[7] }, { "Outerwear", avatarInfo[8]},
            { "Pants", avatarInfo[9] }, { "Shoe", avatarInfo[10] }, {"NickName", nickName }
        });
    }

    public void CreateRoom()
    {
        string roomName = LobbyUIController.lobbyUI.roomSetting[0].text;
        int playerCount = Convert.ToInt32(LobbyUIController.lobbyUI.roomSetting[1].text);

        print(roomName + " " + playerCount);
        if (roomName.Length > 0 && playerCount > 1)
        {
            RoomOptions roomOpt = new RoomOptions();
            roomOpt.MaxPlayers = playerCount;
            roomOpt.IsOpen = true;
            roomOpt.IsVisible = true;

            roomOpt.CustomRoomPropertiesForLobby = new string[] { "MASTER_NAME" };
            currentRoomId = Mathf.Abs(DateTime.Now.GetHashCode());
            roomOpt.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "RoomId", currentRoomId }, { "MASTER_NAME", PhotonNetwork.NickName }};


            //roomOpt.CustomRoomPropertiesForLobby = new string[] { "MASTER_NAME", "PASSWORD", "SCENE_NUMBER" };


            //// 키에 맞는 해시테이블 추가하기
            //Hashtable roomTable = new Hashtable();
            //roomTable.Add("MASTER_NAME", PhotonNetwork.NickName);
            //roomTable.Add("PASSWORD", 1234);
            //roomTable.Add("SCENE_NUMBER", LobbyUIController.lobbyUI.drop_mapSelection.value + 2);
            //roomOpt.CustomRoomProperties = roomTable;
            PhotonNetwork.CreateRoom(roomName, roomOpt, TypedLobby.Default);
        }
    }

    void ChangePanel(int offIndex, int onIndex)
    {
        panelList[offIndex].SetActive(false);
        panelList[onIndex].SetActive(true);
    }

    public void JoinRoom()
    {
        ChangePanel(1, 2);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(MethodInfo.GetCurrentMethod().Name + " is Call!");
        PhotonNetwork.LoadLevel(8);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.LogError(message);
        LobbyUIController.lobbyUI.PrintLog("입장 실패..." + message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        string playerMsg = $"{newPlayer.NickName}님이 입장하셨습니다.";
        LobbyUIController.lobbyUI.PrintLog(playerMsg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        string playerMsg = $"{otherPlayer.NickName}님이 퇴장하셨습니다.";
        LobbyUIController.lobbyUI.PrintLog(playerMsg);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList)
            {
                cachedRoomList.Remove(room);
            }
            else
            {
                if (cachedRoomList.Contains(room))
                {
                    cachedRoomList.Remove(room);
                }
                cachedRoomList.Add(room);
            }
        }

        for (int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }

        foreach (RoomInfo room in cachedRoomList)
        {
            GameObject go = Instantiate(roomPrefab, scrollContent);
            JSWRoomPanel roomPanel = go.GetComponent<JSWRoomPanel>();
            roomPanel.SetRoomInfo(room);
            roomPanel.btn_join.onClick.AddListener(() =>
            {
                PhotonNetwork.JoinRoom(room.Name);
            });
        }
    }

    public void MoveMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public ProfileGetManager profileManager;
    public void MoveProfileScene()
    {
        profileManager.SendDataToServer(DateTime.Now.ToString("yyyy-MM-dd"), true);
    }
}

public class BypassCertificate1 : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}

[Serializable]
public class CreateRoomData
{
    public int roomId;
    public long ownerId;
}