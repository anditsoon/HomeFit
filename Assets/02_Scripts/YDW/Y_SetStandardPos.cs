using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_SetStandardPos : MonoBehaviour
{
    Y_MediaPipeTest mediapipe;
    Y_CountSquatt countSquatt;
    Y_CountJumpingJack countJumpingJack;

    GameObject directionCanvas;

    public float duration;

    public Y_PlayerScreenUIManager playerScreenUIManager;
    public Y_TimerUI timerUI;


    bool hasStartPointSet;

    // Start is called before the first frame update
    void Start()
    {
        //isFullBody = false;
        duration = 0;
        mediapipe = GetComponent<Y_MediaPipeTest>();
        countSquatt = GetComponent<Y_CountSquatt>();
        countJumpingJack = GetComponent<Y_CountJumpingJack>();
        directionCanvas = GameObject.Find("DirectionCanvas");
        hasStartPointSet = false;
        timerUI = GameObject.Find("Canvas").GetComponent<Y_TimerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mediapipe.getV3FromLandmark(27).y < 1f && mediapipe.getV3FromLandmark(28).y < 1f 
            && mediapipe.getV3FromLandmark(11).y > 0.05f && mediapipe.getV3FromLandmark(12).y > 0.05f)
        {
            if(playerScreenUIManager.isSelected) duration += Time.deltaTime;
        }

        if(duration > 5f && !hasStartPointSet) // 업데이트에서 하니까 계속 되지 않게 hasStartPointSet bool 값 넣어줌
        {
            //골반 사이 위치 저장
            mediapipe.startSP = mediapipe.getStandardPoint();

            countSquatt.startGame = true;
            countJumpingJack.startGame = true;

            hasStartPointSet = true;

            timerUI.hasStart = true;

            //directionCanvas.SetActive(false);
        }
    }
}
