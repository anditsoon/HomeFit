using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Loading;
using Unity.VisualScripting;
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

    public Y_TimerUI timerUI;

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
            //t = EaseOutQuint(t);
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
            //t = EaseInQuint(t);
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



    //float EaseOutQuint(float x)
    //{
    //    return 1 - (float)Math.Pow(1 - x, 3);
    //}

    //float EaseInQuint(float x)
    //{
    //    return x * x * x;
    //}


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

    

    //public IEnumerator Flicker(List<CanvasRenderer> uiElements)
    //{
    //    bool isFadingOut = false;

    //    float[] alphas = new float[uiElements.Count];

    //    for (int i = 0; i < uiElements.Count; i++)
    //    {
    //        alphas[i] = uiElements[i].GetAlpha();
    //    }

    //    while (true)
    //    {
    //        if (timerUI.allReadyGo) // 플레이어가 모두 자세 인식을 마쳤으면
    //        {
    //            // Inactive
    //            flickerImg.SetActive(false);
    //            yield break;
    //        }

    //        for (int i = 0; i < uiElements.Count; i++)
    //        {
    //            if (isFadingOut)
    //                alphas[i] = Mathf.Lerp(alphas[i], 0, Time.deltaTime * fadeSpeed); // 서서히 투명해짐
    //            else
    //                alphas[i] = Mathf.Lerp(alphas[i], 1, Time.deltaTime * fadeSpeed); // 서서히 보임

    //            // 각 CanvasRenderer의 알파 값 설정
    //            uiElements[i].SetAlpha(alphas[i]);
    //        }

    //        // 알파 값이 거의 다 도달하면 상태 전환
    //        if (isFadingOut && alphas[0] <= 0.05f)
    //        {
    //            isFadingOut = false;
    //            yield return new WaitForSeconds(flickerInterval); // 잠시 대기 후 다시 서서히 보이기
    //        }
    //        else if (!isFadingOut && alphas[0] >= 0.95f)
    //        {
    //            isFadingOut = true;
    //            yield return new WaitForSeconds(flickerInterval); // 잠시 대기 후 다시 서서히 투명하게
    //        }

    //        yield return null; // 매 프레임마다 업데이트
    //    }

    //    //// 깜박임이 끝난 후 UI가 안보이는 상태로 유지
    //    //for (int i = 0; i < uiElements.Length; i++)
    //    //{
    //    //    uiElements[i].SetAlpha(0);
    //    //}
    //}

    //public float flickerDuration = 1f; // 한 번 깜박이는 데 걸리는 시간
    public float fadeSpeed = 2f;
    public float flickerInterval = 0.5f;
    public GameObject flickerImg;

    public IEnumerator Flicker(CanvasGroup uiElement)
    {
        float timer = 0f;
        bool isFadingOut = false;

        while (true)
        {
            if (timerUI.allReadyGo) // 플레이어가 모두 자세 인식을 마쳤으면
            {
                // Inactive
                flickerImg.SetActive(false);
                //yield break;
                break;
            }

            // 서서히 알파 값을 변경 (보임 -> 안보임, 안보임 -> 보임)
            if (isFadingOut)
                uiElement.alpha = Mathf.Lerp(uiElement.alpha, 0, Time.deltaTime * fadeSpeed); // 서서히 투명해짐
            else
                uiElement.alpha = Mathf.Lerp(uiElement.alpha, 1, Time.deltaTime * fadeSpeed); // 서서히 보임

            // 알파 값이 거의 다 도달하면 상태 전환
            if (isFadingOut && uiElement.alpha <= 0.05f)
            {
                isFadingOut = false;
                yield return new WaitForSeconds(flickerInterval); // 잠시 대기 후 다시 서서히 보이기
            }
            else if (!isFadingOut && uiElement.alpha >= 0.95f)
            {
                isFadingOut = true;
                yield return new WaitForSeconds(flickerInterval); // 잠시 대기 후 다시 서서히 투명하게
            }

            //timer += Time.deltaTime;
            yield return null; // 매 프레임마다 업데이트
        }

        // 깜박임이 끝난 후 UI가 보이는 상태로 유지
        uiElement.alpha = 1;
    }

}
