using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_CountJumpingJack : MonoBehaviour
{
    public bool startGame;
    public bool isJumpingJack;
    public float jumpingJackCount;
    Y_MediaPipeTest mediapipe;
    Transform leftHandPos;
    Transform rightHandPos;

    // Start is called before the first frame update
    void Start()
    {
        isJumpingJack = false;
        jumpingJackCount = 0;
        mediapipe = GetComponent<Y_MediaPipeTest>();
        leftHandPos = mediapipe.leftArmTarget.transform;
        rightHandPos = mediapipe.rightArmTarget.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame)
        {
            print("손 좌표 : " + leftHandPos.position.y + " 오른손도!! : " + rightHandPos.position.y);
            if (leftHandPos.position.y < 3.35f && rightHandPos.position.y < 3.35f && !isJumpingJack) // 3.25
            {
                jumpingJackCount++;
                isJumpingJack = true;
            }

            if (leftHandPos.position.y > 3.6f && rightHandPos.position.y < 3.35f && isJumpingJack) // 3.35
            {
                isJumpingJack = false;
            }

            print("점핑잭 횟수: " + jumpingJackCount);

        }
    }
}
