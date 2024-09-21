using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Y_RankUI : MonoBehaviour
{
    public TMP_Text name1st;
    public TMP_Text name2st;
    public TMP_Text name3st;

    public TMP_Text cnt1st;
    public TMP_Text cnt2st;
    public TMP_Text cnt3st;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateRanking()
    {
        var rankings = PhotonNetwork.PlayerList
            .Select(player =>
        {
            PhotonView photonView = PhotonView.Find(player.ActorNumber);
            return new
            {
                Player = photonView,
                //SquatCount = 
            };
        });
    }
}
