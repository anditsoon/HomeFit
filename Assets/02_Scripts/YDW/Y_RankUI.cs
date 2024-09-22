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
    public GameObject rankPanel;

    public GameObject img3rd;

    private void Start()
    {
        timerUI = GameObject.Find("Canvas").GetComponent<Y_TimerUI>();
    }

    void Update()
    {
        if(rankPanel.activeSelf) // !timerUI.hasStart && 
        {
            CalculateRanking();
        }
    }

    public void CalculateRanking()
    {
        var rankings = PhotonNetwork.PlayerList
            .Select(player =>
            {
                PhotonView photonView = null;
                foreach (var view in PhotonNetwork.PhotonViewCollection)
                {
                    if (view.Owner == player)
                    {
                        photonView = view;
                        break;
                    }
                }

                Y_CountSquatt countSquatt = photonView.gameObject.GetComponent<Y_CountSquatt>();
                float squatCount = countSquatt != null ? countSquatt.squatCount : 0;

                return new
                {
                    Player = player.NickName,
                    SquatCount = Mathf.Clamp(squatCount, 0, 9999)
                };
            })
            .OrderByDescending(p => p.SquatCount)
            .ToList();


        if (rankings.Count > 1)
        {
            name1st.text = rankings[0].Player;
            cnt1st.text = rankings[0].SquatCount.ToString() + "회";
            name2st.text = rankings[1].Player;
            cnt2st.text = rankings[1].SquatCount.ToString() + "회";
        }

        if (rankings.Count > 2)
        {
            name3st.text = rankings[2].Player;
            cnt3st.text = rankings[2].SquatCount.ToString() + "회";
        }
        else // 어차피 두 명 이상이어야 방을 만들 수 있음
        {
            img3rd.SetActive(false);
        }
    }
}
