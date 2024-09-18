using System.Collections;
using System.Collections.Generic;
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
    public bool canActive = true;

    Y_SetStandardPos setStandardPos;
    Y_CountSquatt cntSquatScript;
    float prevSquatNum = 0;
    float squatNum = 0;
    public TMP_Text squatCnt;


    void Start()
    {
        cntSquatScript = GameObject.Find("Player").GetComponent<Y_CountSquatt>();
        setStandardPos = GameObject.Find("Player").GetComponent<Y_SetStandardPos>();
    }


    void Update()
    {
        countdownTime = setStandardPos.duration;
        showTime = (int)(6 - countdownTime);
        if(canActive) Countdown();
        currTime += Time.deltaTime;
        if (currTime > duration)
        {
            currTime = 0;
        }

        squatNum = cntSquatScript.squattCount;
        if(prevSquatNum <= squatNum)
        {
            squatCnt.text = squatNum.ToString();
            prevSquatNum = squatNum;
        }
    }

    void Countdown()
    {
        if (showTime <= 0)
        {
            PoseRecCountdown.text = "시작!";
            StartCoroutine(deactivateCntUI()); // 일정 시간 이후 UI 비활성화
            canActive = false;
        }
        else if (showTime < 6)
        {
            countdownText.SetActive(true);
            PoseRecCountdown.text = showTime.ToString();
        }
        
    }

    IEnumerator deactivateCntUI()
    {
        yield return new WaitForSeconds(1f);
        directionCanvas.SetActive(false);
    }

    IEnumerator changeFontSize()
    {
        while(currTime < duration)
        {
            float t = EaseOutQuint(duration / currTime);
            PoseRecCountdown.text = showTime.ToString();
            PoseRecCountdown.fontSize = Mathf.Lerp(50, 100, t);
            yield return null;
        }
    }

    float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }

}
