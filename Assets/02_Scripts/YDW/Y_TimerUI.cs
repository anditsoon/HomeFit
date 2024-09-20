using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Y_TimerUI : MonoBehaviour
{
    public bool hasStart;

    public TMP_Text timerText;
    private float elapsedTime = 0f;

    public float duration;

    void Start()
    {
        duration = 5;
    }

    void Update()
    {
        if(hasStart)
        {
            elapsedTime += Time.deltaTime;

            // 경과 시간을 시:분:초 형식으로 변환
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            string timeText = string.Format("{0:00}:{1:00}",
                timeSpan.Minutes, timeSpan.Seconds);

            // UI 텍스트 업데이트
            timerText.text = timeText;

            if(elapsedTime > duration)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
