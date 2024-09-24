using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    private readonly string RemoveRoomUrl = "https://125.132.216.190:12502/api/room";

    private int currentRoomId;

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

            yield return StartCoroutine(SendRoomInfo("DELETE", currentRoomId.ToString()));

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

    private IEnumerator SendRoomInfo(string method, string roomId)
    {
        Debug.Log($"Sending {method} request for room: {roomId}");
        string url = $"{RemoveRoomUrl}/{roomId}";
        Debug.Log($"Request URL: {url}");

        string jwtToken = PlayerPrefs.GetString("jwtToken");
        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogError("JWT token is missing or empty");
            yield break;
        }
        Debug.Log($"JWT Token (first 20 chars): {jwtToken.Substring(0, Mathf.Min(jwtToken.Length, 20))}...");

        UnityWebRequest www = UnityWebRequest.Delete(url);
        www.SetRequestHeader("Authorization", "Bearer " + jwtToken);
        www.certificateHandler = new BypassCertificate1();

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"{method} request failed. Error: {www.error}");
            Debug.LogError($"Response Code: {www.responseCode}");
            Debug.LogError($"Response Headers: {www.GetResponseHeaders()}");
            if (www.downloadHandler != null && www.downloadHandler.text != null)
            {
                Debug.LogError($"Response Body: {www.downloadHandler.text}");
            }
            else
            {
                Debug.LogError("Response Body is null");
            }
        }
        else
        {
            Debug.Log($"{method} request successful!");
            if (www.downloadHandler != null && www.downloadHandler.text != null)
            {
                Debug.Log($"Response: {www.downloadHandler.text}");
            }
            else
            {
                Debug.Log("Response Body is null");
            }
        }

        www.certificateHandler.Dispose();
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