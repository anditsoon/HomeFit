using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Y_UIManager : MonoBehaviour
{
    public GameObject countdownText;
    public GameObject directionCanvas;
    public TMP_Text PoseRecCountdown;
    float countdownTime;
    public int showTime;
    float currTime = 0;
    float duration = 1;
    public bool isSelected = false;
    public bool canActive = true;

    Y_SetStandardPos setStandardPos;
    float canvasAlphaTime = 0;

    public GameObject chooseWorkOut;
    public Y_CountJumpingJack cntJumpingJack;
    public Y_CountSquatt cntSquat;
    public GameObject chooseWorkOutCanvas;

    void Start()
    {
        setStandardPos = GameObject.Find("Player").GetComponent<Y_SetStandardPos>();
    }


    void Update()
    {
        if(isSelected)
        {
            directionCanvas.SetActive(true);
            countdownTime = setStandardPos.duration;
            showTime = (int)(6 - countdownTime);
            if(canActive) Countdown();
            currTime += Time.deltaTime;
            if (currTime > duration)
            {
                currTime = 0;
            }
        }

        
    }

    CanvasRenderer[] canvasRenderers;
    void Countdown()
    {
        if (showTime <= 0)
        {
            PoseRecCountdown.text = "시작!";
            canvasRenderers = directionCanvas.GetComponentsInChildren<CanvasRenderer>();
            StartCoroutine(decreaseAlpha(canvasRenderers)); // 투명도 조절 후
            directionCanvas.SetActive(false); // UI 비활성화
            canActive = false;
        }
        else if (showTime < 6)
        {
            countdownText.SetActive(true);
            PoseRecCountdown.text = showTime.ToString();
        }
        
    }

    //IEnumerator deactivateCntUI()
    //{
    //    yield return new WaitForSeconds(1f);
    //    directionCanvas.SetActive(false);
    //}

    IEnumerator decreaseAlpha(CanvasRenderer[] canvasRenderes)
    {
        while(true)
        {
            canvasAlphaTime += Time.deltaTime;
            if(canvasAlphaTime > 1)
            {
                //chooseWorkOut.SetActive(true); // 스쿼트할지 팔벌려뛰기할지 고르는 UI 활성화
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


    //IEnumerator changeFontSize()
    //{
    //    while(currTime < duration)
    //    {
    //        float t = EaseOutQuint(duration / currTime);
    //        PoseRecCountdown.text = showTime.ToString();
    //        PoseRecCountdown.fontSize = Mathf.Lerp(50, 100, t);
    //        yield return null;
    //    }
    //}

    //float EaseOutQuint(float x)
    //{
    //    return 1 - Mathf.Pow(1 - x, 5);
    //}

    // 스쿼트 / 팔벌려뛰기 버튼 선택 시 호출되는 함수들
    public void SelectSquat()
    {
        cntSquat.enabled = true;
        isSelected = true;
        canvasRenderers = chooseWorkOutCanvas.GetComponentsInChildren<CanvasRenderer>();
        StartCoroutine(decreaseAlpha(canvasRenderers));
        chooseWorkOut.SetActive(false);
    }

    public void SelectJumpingJack()
    {
        cntJumpingJack.enabled = true;
        isSelected = true;
        canvasRenderers = chooseWorkOutCanvas.GetComponentsInChildren<CanvasRenderer>();
        StartCoroutine(decreaseAlpha(canvasRenderers));
        chooseWorkOut.SetActive(false);
    }
}
