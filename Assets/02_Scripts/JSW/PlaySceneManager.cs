using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviourPunCallbacks
{
    public GameObject myPlayer;

    public void MoveMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    void Start()
    {
        StartCoroutine(SpawnPlayer());

        // OnPhotonSerializeView에서 데이터 전송 빈도 수 설정하기 (per seconds)
        PhotonNetwork.SerializationRate = 30;
        // 대부분의 데이터 전송 빈도 수 설정하기(per seconds)
        PhotonNetwork.SendRate = 30;

        GameObject playerListUI = GameObject.Find("text_PlayerList");
       
    }

    IEnumerator SpawnPlayer()
    {
        // 룸에 입장이 완료될 때까지 기다린다.
        yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });


        Vector2 randomPos = Random.insideUnitCircle * 5.0f;
        Vector3 initPosition = new Vector3(randomPos.x, 0, randomPos.y);

        myPlayer = PhotonNetwork.Instantiate("Player", initPosition, Quaternion.identity);
    }
}
