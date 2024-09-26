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
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class JSWConnectionManager : MonoBehaviourPunCallbacks
{
    public GameObject roomPrefab;
    public Transform scrollContent;
    public GameObject[] panelList;
    bool isExit;

    private readonly string CreateRoomUrl = "https://125.132.216.190:12502/api/room";

    List<RoomInfo> cachedRoomList = new List<RoomInfo>();
    private int currentRoomId;

    void Start()
    {
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_SCENEMOVE2);
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
        StartCoroutine(SendRoomInfo("POST", currentRoomId.ToString(), PlayerPrefs.GetString("userId")));
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

    public void DeleteRoomToServer()
    {
        Debug.Log(PhotonNetwork.CountOfPlayersInRooms);
        if (PhotonNetwork.CountOfPlayersInRooms == 0)
        {
            Debug.Log(PhotonNetwork.CountOfPlayersInRooms);
            int roomId = (int)PhotonNetwork.CurrentRoom.CustomProperties["RoomId"];
            StartCoroutine(SendRoomInfo("DELETE", roomId.ToString()));
        }
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

    private IEnumerator SendRoomInfo(string method, string roomId, string ownerId = null)
    {
        string url;
        if (method == "POST")
        {
            url = CreateRoomUrl;
        }
        else
        {
            Debug.LogError("Invalid HTTP method");
            yield break;
        }

        string jwtToken = PlayerPrefs.GetString("jwtToken");
        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogError("JWT token is missing or empty");
            yield break;
        }

        UnityWebRequest www;

        CreateRoomData roomData = new CreateRoomData
        {
            roomId = int.Parse(roomId),
            ownerId = long.Parse(ownerId)
        };
        string jsonData = JsonUtility.ToJson(roomData);
        www = UnityWebRequest.Put(url, jsonData);
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");

        www.SetRequestHeader("Authorization", "Bearer " + jwtToken);

        www.certificateHandler = new BypassCertificate1();

        Debug.Log($"Sending {method} request to {url}");
        Debug.Log($"Authorization: Bearer {jwtToken.Substring(0, Math.Min(jwtToken.Length, 10))}...");
        if (method == "POST")
        {
            Debug.Log($"Request body: {www.uploadHandler.data}");
        }

        yield return www.SendWebRequest();

        Debug.Log($"Response Code: {www.responseCode}");
        Debug.Log($"Response Headers: {www.GetResponseHeaders()}");
        Debug.Log($"Response Body: {www.downloadHandler.text}");

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"{method} request to {url} failed. Error: {www.error}");
            Debug.LogError($"Full response: {www.downloadHandler.text}");
        }
        else
        {
            Debug.Log($"{method} request to {url} successful!");
        }

        www.certificateHandler.Dispose();
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