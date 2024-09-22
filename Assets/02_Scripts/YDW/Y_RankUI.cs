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

    Y_TimerUI timerUI;


    void Update()
    {
        
    }

    public void CalculateRanking()
    {
        var rankings = PhotonNetwork.PlayerList
            .Select(player =>
        {
            PhotonView photonView = PhotonView.Find(player.ActorNumber);

            Y_CountSquatt countSquatt = photonView.GetComponent<Y_CountSquatt>();
            float squatCount = countSquatt != null ? countSquatt.squatCount : 0;

            return new
            {
                Player = player.NickName,
                SquatCount = squatCount
            };
        })
            .OrderByDescending(p => p.SquatCount)
            .ToList();

        if (rankings.Count > 0)
        {
            name1st.text = rankings[0].Player;
            cnt1st.text = rankings[0].SquatCount.ToString();
        }

        if (rankings.Count > 1)
        {
            name2st.text = rankings[1].Player;
            cnt2st.text = rankings[1].SquatCount.ToString();
        }

        if (rankings.Count > 2)
        {
            name3st.text = rankings[2].Player;
            cnt3st.text = rankings[2].SquatCount.ToString();
        }
    }
}
