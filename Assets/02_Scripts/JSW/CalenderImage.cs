using System.Collections;
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

    private void Start()
    {
        profileManager = GameObject.Find("ProfileUIManager");

        CalenderDay = GameObject.Find("CalenderDay").GetComponent<TMP_Text>();

        ExerciseCount = GameObject.Find("ExerciseCount").transform.GetChild(0).GetComponent<TMP_Text>();
        KcalCount = GameObject.Find("KcalCount").transform.GetChild(0).GetComponent<TMP_Text>();
        MinutesCount = GameObject.Find("MinutesCount").transform.GetChild(0).GetComponent<TMP_Text>();

        btn_Cal = GetComponent<Button>();
        btn_Cal.onClick.AddListener(() =>
        {
            SetBoldText();
        });
    }
    

    public void SetBoldText()
    {
        profileManager.GetComponent<ProfileUIManager>().CalenderReset();
        GetComponent<TMP_Text>().fontStyle = FontStyles.Bold;
        string num = GetComponent<TMP_Text>().text;
        if (num.Length == 1)
        {
            CalenderDay.text = "2024-09-0" + num;
        }
        else
        {
            CalenderDay.text = "2024-09-" + num;
        }

        ExerciseCount.text = Random.Range(1, 5).ToString();
        KcalCount.text = Random.Range(1, 200).ToString();
        MinutesCount.text = Random.Range(1, 60).ToString();
    }
}
