using Google.Protobuf.WellKnownTypes;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Y_PlayerScreenUIManager : MonoBehaviour
{
    public Y_SetStandardPos setStandardPos;
    public Y_UIManager uiManager;
    public GameObject directionCanvas;
    public GameObject countdownText;
    public GameObject resultPanel;

    public bool isSelected = false;
    public bool canActive = true;
    float countdownTime;
    public int showTime;
    float currTime = 0;
    float duration = 1;

    public TMP_Text PoseRecCountdown;

    bool startWorkOut = false;

    public Y_TimerUI timerUI;

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            directionCanvas.SetActive(true);
            countdownTime = setStandardPos.duration;
            showTime = (int)(6 - countdownTime);
            if (canActive) Countdown();
            currTime += Time.deltaTime;
            if (currTime > duration)
            {
                currTime = 0;
            }
        }

        if(startWorkOut)
        {
            startWorkOut = false;
        }

        //if(timerUI.elapsedTime > timerUI.duration && resultPanel.activeSelf)
        //{
        //    // 카메라 위치 변화 
        //    StartCoroutine(MoveCamera());
        //    resultPanel.SetActive(false);
        //}
    }

    public GameObject newCamPos;

    IEnumerator MoveCamera()
    {
        while (true)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCamPos.transform.position, Time.deltaTime * 10);
            if (Vector3.Distance(Camera.main.transform.position, newCamPos.transform.position) < 0.2f) break;
            yield return null;
        }
    }

    CanvasRenderer[] canvasRenderers;
    CanvasRenderer[] canvasRenderers2;

    void Countdown()
    {
        if (showTime <= 0)
        {
            uiManager = GameObject.Find("Canvas").GetComponent<Y_UIManager>();
            PoseRecCountdown.text = "시작!";

            // directionCanvas 비활성화
            canvasRenderers = directionCanvas.GetComponentsInChildren<CanvasRenderer>();
            StartCoroutine(uiManager.decreaseAlpha(canvasRenderers)); // 투명도 조절 후
            directionCanvas.SetActive(false);

            // resultPanel 활성화
            resultPanel.SetActive(true);
            canvasRenderers2 = resultPanel.GetComponentsInChildren<CanvasRenderer>();
            StartCoroutine(uiManager.IncreaseAlpha(canvasRenderers2));

            canActive = false;
            isSelected = false;
            startWorkOut = true;
        }
        else if (showTime < 6)
        {
            countdownText.SetActive(true);
            PoseRecCountdown.text = showTime.ToString();
        }
    }
}
