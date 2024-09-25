using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Loading;
using UnityEngine;
using UnityEngine.UI;

public class Y_UIManager : MonoBehaviour
{

    float canvasAlphaTime = 0;

    public GameObject chooseWorkOut;
    public Y_CountJumpingJack cntJumpingJack;
    public Y_CountSquatt cntSquat;
    public GameObject chooseWorkOutCanvas;
    public PlaySceneManager PSM;
    

    public GameObject LoadingPanel;
    public GameObject LoadingPanelNotMaster;

    bool isnextPanel;
    bool endSelect;

    GameObject playerScreenCanvas;

    public GameObject CD;
    public GameObject CD2;

    public GameObject squatPanel;
    public GameObject jumpingJackPanel;

    void Update()
    {
        if (!isnextPanel && PSM.myPlayer != null && PSM.myPlayer.GetComponent<JSWPhotonVoiceTest>().isMaster)
        {
            chooseWorkOut.SetActive(true);
            isnextPanel = true;
        }
        if (PSM.myPlayer != null && PSM.myPlayer.GetComponent<JSWPhotonVoiceTest>().AllplayerInRoom)
        {
            LoadingPanel.SetActive(false);
            if (!PSM.myPlayer.GetComponent<JSWPhotonVoiceTest>().isMaster && !endSelect)
            {
                LoadingPanelNotMaster.SetActive(true);
            }
            else
            {
                LoadingPanelNotMaster.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha9)) StartCoroutine(MoveUI("만세!"));
    }

    CanvasRenderer[] canvasRenderers;

    //public void CountDownUpdateCoroutine()
    //{
        
    //    PSM.myPlayer.GetComponent<JSWPhotonVoiceTest>().AllReadyGO_RPC();
    //}
    //IEnumerator CountDownUpdate()
    //{
    //    while (true)
    //    {

    //    }
    //}

    public IEnumerator decreaseAlpha(CanvasRenderer[] canvasRenderers)
    {
        while(true)
        {
            canvasAlphaTime += Time.deltaTime;
            if(canvasAlphaTime > 1)
            {
                canvasAlphaTime = 0;
                break;
            }

            foreach (CanvasRenderer canvasRenderer in canvasRenderers)
            {
                Color originalColor = canvasRenderer.GetColor();
                originalColor.a = 1 - canvasAlphaTime;
                canvasRenderer.SetColor(originalColor);
            }

            yield return null;
        }
    }

    public IEnumerator IncreaseAlpha(CanvasRenderer[] canvasRenderers)
    {
        while (true)
        {
            canvasAlphaTime += Time.deltaTime;
            if (canvasAlphaTime > 1)
            {
                canvasAlphaTime = 0;
                break;
            }

            foreach (CanvasRenderer canvasRenderer in canvasRenderers)
            {
                Color originalColor = canvasRenderer.GetColor();
                originalColor.a = canvasAlphaTime;
                canvasRenderer.SetColor(originalColor);
            }

            yield return null;
        }
    }

    float currTime;
    float middleTime = 1f;
    float stayTime = 2f;
    float endTime = 3f;

    public GameObject startPos;
    public GameObject middlePos;
    public GameObject endPos;

    Vector3 startVector;
    Vector3 middleVector;
    Vector3 endVector;

    public GameObject flyingTxt;

    //public IEnumerator MoveUI(String str)
    //{
    //    startVector = startPos.transform.position;
    //    middleVector = middlePos.transform.position;
    //    endVector = endPos.transform.position;

    //    flyingTxt.transform.position = startVector;

    //    flyingTxt.GetComponentInChildren<TMP_Text>().text = str;

    //    while(true)
    //    {
    //        currTime += Time.deltaTime;
    //        float t;

    //        if(currTime > endTime)
    //        {
    //            currTime = 0;
    //            flyingTxt.transform.position = startVector;
    //            break;
    //        }
    //        else if(currTime > stayTime)
    //        {
    //            t = EaseInQuint(currTime - stayTime);
    //            print("!!!!!!!!!!!!!!!!!" + t);
    //            flyingTxt.transform.position = new Vector3(Mathf.Lerp(transform.position.x, endVector.x, t * 0.1f), flyingTxt.transform.position.y, flyingTxt.transform.position.z);
    //        }
    //        else if(currTime > middleTime)
    //        {
    //            flyingTxt.transform.position = middleVector;
    //        }
    //        else
    //        {
    //            t = EaseOutQuint(currTime);
    //            print("???????????" + t);
    //            flyingTxt.transform.position = new Vector3(Mathf.Lerp(transform.position.x, middleVector.x, t * 0.1f), flyingTxt.transform.position.y, flyingTxt.transform.position.z);
    //        }

    //        yield return null;
    //    }
    //}

    public IEnumerator MoveUI(String str)
    {
        startVector = startPos.transform.position;
        middleVector = middlePos.transform.position;
        endVector = endPos.transform.position;

        flyingTxt.transform.position = startVector;

        flyingTxt.GetComponentInChildren<TMP_Text>().text = str;
        float t = 0;
        while (true)
        {
            t += Time.deltaTime;
            flyingTxt.transform.position = Vector3.Lerp(flyingTxt.transform.position, middleVector, t * 1f);
            if (t > middleTime)
            {
                t = 0;
                break;
            }
            yield return null;
        }
        while (true)
        {
            t += Time.deltaTime;
            flyingTxt.transform.position = Vector3.Lerp(flyingTxt.transform.position, endVector, t * 0.001f);
            if (t > middleTime)
            {
                t = 0;
                break;
            }
            yield return null;
        }
        while (true)
        {
            t += Time.deltaTime;
            flyingTxt.transform.position = Vector3.Lerp(flyingTxt.transform.position, endVector, t * 1f);
            if (t > middleTime)
            {
                t = 0;
                break;
            }
            yield return null;
        }
        flyingTxt.transform.position = startVector;
    }



    float EaseOutQuint(float x)
    {
        return 1 - (float)Math.Pow(1 - x, 3);
    }

    float EaseInQuint(float x)
    {
        return x * x * x;
    }


    // 스쿼트 / 팔벌려뛰기 버튼 선택 시 호출되는 함수들
    public void SelectSquat()
    {
        PSM.myPlayer.GetComponent<JSWPhotonVoiceTest>().ChooseSquateOrJump_RPC(1);
    }
    public void SelectSquat2()
    {
        endSelect = true;
        cntSquat = PSM.myPlayer.GetComponent<Y_CountSquatt>();
        cntSquat.enabled = true;
        playerScreenCanvas = PSM.myPlayer.transform.GetChild(3).gameObject;
        playerScreenCanvas.GetComponent<Y_PlayerScreenUIManager>().isSelected = true;
        canvasRenderers = chooseWorkOutCanvas.GetComponentsInChildren<CanvasRenderer>();
        StartCoroutine(decreaseAlpha(canvasRenderers));
        StartCoroutine(deactiveChooseWorkOut());
    }
    public void SelectJumpingJack()
    {
        PSM.myPlayer.GetComponent<JSWPhotonVoiceTest>().ChooseSquateOrJump_RPC(2);
    }

    public void SelectJumpingJack2()
    {
        endSelect = true;
        cntJumpingJack = PSM.myPlayer.GetComponent<Y_CountJumpingJack>();
        cntJumpingJack.enabled = true;
        playerScreenCanvas = PSM.myPlayer.transform.GetChild(3).gameObject;
        playerScreenCanvas.GetComponent<Y_PlayerScreenUIManager>().isSelected = true;
        canvasRenderers = chooseWorkOutCanvas.GetComponentsInChildren<CanvasRenderer>();
        StartCoroutine(decreaseAlpha(canvasRenderers));
        StartCoroutine(deactiveChooseWorkOut());
    }

    IEnumerator deactiveChooseWorkOut()
    {
        yield return new WaitForSeconds(1f);
        chooseWorkOut.SetActive(false);
    }

}
