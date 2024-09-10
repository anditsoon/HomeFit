using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_CountSquatt : MonoBehaviour
{
    public bool startGame;
    public bool isSquatting;
    public float squattCount;
    Y_MediaPipeTest mediapipe;
    Transform pelvisPos;

    // Start is called before the first frame update
    void Start()
    {
        isSquatting = false;
        squattCount = 0;
        mediapipe = GetComponent<Y_MediaPipeTest>();
        pelvisPos = mediapipe.spineTrans;
    }

    // Update is called once per frame
    void Update()
    {
        if(startGame)
        {
            print("y 좌표 : " + pelvisPos.position.y);
            if (pelvisPos.position.y < 3.25f && !isSquatting)
            {
                squattCount++;
                isSquatting = true;
            }
        
            if(pelvisPos.position.y > 3.35f && isSquatting)
            {
                isSquatting = false;
            }

            print("스쿼트 횟수: " + squattCount);

        }
    }
}
