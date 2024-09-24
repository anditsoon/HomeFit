using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Y_PlayerUIManager : MonoBehaviour
{
    public Y_CountSquatt cntSquatScript;
    public Y_CountJumpingJack cntJumpingJackScript;
    Y_SquatCntUI squatCntUIScript;
    Y_JumpingJackCntUI jumpingJackCntUIScript;

    float prevSquatNum = 0;
    float squatNum = 0;
    float prevJumpingJackNum = 0;
    float jumpingJackNum;
    public TMP_Text squatCntUI;
    public TMP_Text jumpingJackCntUI;

    public GameObject squatCntUIgo;
    public bool squatReset = false;
    public GameObject jumpingJackCntUIgo;
    public bool jumpingJackReset = false;

    public GameObject MyCntUIgo;
    public PhotonView pv;

    // Start is called before the first frame update
    void LateStart()
    {
        squatCntUIScript = GetComponentInChildren<Y_SquatCntUI>();
        jumpingJackCntUIScript = GetComponentInChildren<Y_JumpingJackCntUI>();
    }

    // Update is called once per frame
    void Update()
    {
        squatNum = cntSquatScript.squatCount;
        if (prevSquatNum < squatNum)
        {
            if (pv.IsMine)
            {
                MyCntUIgo.SetActive(true);
                MyCntUIgo.GetComponent<TMP_Text>().text = squatNum.ToString();
            }
            else
            {
                squatCntUIgo.SetActive(true);
                squatCntUI.text = squatNum.ToString();
            }
            squatReset = true;
            prevSquatNum = squatNum;
        }

        jumpingJackNum = cntJumpingJackScript.jumpingJackCount;
        if (prevJumpingJackNum < jumpingJackNum)
        {
            if (pv.IsMine)
            {
                MyCntUIgo.SetActive(true);
                MyCntUIgo.GetComponent<TMP_Text>().text = jumpingJackNum.ToString();
            }
            else
            {
                jumpingJackCntUIgo.SetActive(true);
                jumpingJackCntUI.text = jumpingJackNum.ToString();
            }
            jumpingJackReset = true;
            prevJumpingJackNum = jumpingJackNum;
        }
    }




}
