﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
public class CalenderImage : MonoBehaviour
{
    public Button btn_Cal;
    GameObject profileManager;
    TMP_Text CalenderDay;

    TMP_Text ExerciseCount;
    TMP_Text KcalCount;
    TMP_Text MinutesCount;

    float ExerciseCountNum = 0;
    float KcalCountNum = 0;
    float MinutesCountNum = 0;

    float NowExerciseCountNum = 0;
    float NowKcalCountNum = 0;
    float NowMinutesCountNum = 0;
    float tempMinutes = 0;

    public ProfileGetManager profileGet;

    private void Start()
    {
        profileManager = GameObject.Find("ProfileUIManager");

        CalenderDay = GameObject.Find("CalenderDay").GetComponent<TMP_Text>();

        ExerciseCount = GameObject.Find("ExerciseCount").transform.GetChild(0).GetComponent<TMP_Text>();
        KcalCount = GameObject.Find("KcalCount").transform.GetChild(0).GetComponent<TMP_Text>();
        MinutesCount = GameObject.Find("MinutesCount").transform.GetChild(0).GetComponent<TMP_Text>();

        // 첫시작 즉 이때 오늘 날짜에 해당하는 값 넣어 주면됨 
        ExerciseCountNum = AvatarInfo.instance.totalExercisePerformances;
        KcalCountNum = (float)AvatarInfo.instance.totalCaloriesBurned;
        MinutesCountNum = ExerciseCountNum / 2;

        ExerciseCount.text = ExerciseCountNum.ToString();
        KcalCount.text = ((int)Mathf.Ceil(KcalCountNum)).ToString();
        MinutesCount.text = (Mathf.Round(Mathf.Lerp(tempMinutes, MinutesCountNum, 100f) * 10f) * 0.1f).ToString();

        btn_Cal = GetComponent<Button>();
        btn_Cal.onClick.AddListener(() =>
        {
            SetBoldText();
        });
    }

    private void Update()
    {
        if (profileGet.isChangeDone)
        {
            if (GetComponent<TMP_Text>().fontStyle != FontStyles.Bold)
            {
                return;
            }
            NowExerciseCountNum = ExerciseCountNum;
            ExerciseCount.text = NowExerciseCountNum.ToString();
            NowKcalCountNum = (int)Mathf.Ceil(Mathf.Lerp(NowKcalCountNum, KcalCountNum, Time.deltaTime * 5));
            KcalCount.text = NowKcalCountNum.ToString();
            NowMinutesCountNum = Mathf.Round(Mathf.Lerp(tempMinutes, MinutesCountNum, 100f) * 10f) * 0.1f;
            Debug.Log(NowMinutesCountNum);
            MinutesCount.text = NowMinutesCountNum.ToString();
        }
    }

    public void SetBoldText()
    {
        // 그날 날짜들을 눌렀을 때 정보 업데이트
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);
        profileManager.GetComponent<ProfileUIManager>().CalenderReset();
        GetComponent<TMP_Text>().fontStyle = FontStyles.Bold;

        string num = GetComponent<TMP_Text>().text;

        profileManager.GetComponent<ProfileUIManager>().calenderUI.transform.GetChild(int.Parse(num) - 1).GetChild(0).gameObject.SetActive(true);

        if (num.Length == 1)
        {
            CalenderDay.text = "2024-09-0" + num;
        }
        else
        {
            CalenderDay.text = "2024-09-" + num;
        }
        NowExerciseCountNum = 0;
        NowKcalCountNum = 0;
        NowMinutesCountNum = 0;

        print(NowExerciseCountNum);

        // 그 날 날짜에 해당하는 수치 이때 넣어주면 됨
        ExerciseCountNum = AvatarInfo.instance.totalExercisePerformances;
        KcalCountNum = (float)AvatarInfo.instance.totalCaloriesBurned;
        MinutesCountNum = ExerciseCountNum / 2;
    }
}
