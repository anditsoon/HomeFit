using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_CountJumpingJack : MonoBehaviour, IPunObservable
{
    public bool startGame;
    public bool isJumpingJack;
    public float jumpingJackCount;
    PhotonView pv;
    Y_MediaPipeTest mediapipe;
    Transform leftHandPos;
    Transform rightHandPos;

    public Y_TimerUI timerUI;

    public List<PhotonView> players = new List<PhotonView>();

    float startPelvisPos;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(jumpingJackCount);
        }
        // 그렇지 않고, 만일 데이터를 서버로부터 읽어오는 상태라면...
        else if (stream.IsReading)
        {
            jumpingJackCount = (float)stream.ReceiveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isJumpingJack = false;
        jumpingJackCount = 0;
        mediapipe = GetComponent<Y_MediaPipeTest>();
        leftHandPos = mediapipe.leftArmTarget.transform;
        rightHandPos = mediapipe.rightArmTarget.transform;
        pv = GetComponent<PhotonView>();
        timerUI = GameObject.Find("Canvas").GetComponent<Y_TimerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame && timerUI.hasStart)
        {
            startPelvisPos = mediapipe.startSP.y;

            /////////////////////
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                jumpingJackCount++;
            }
            /////////////////////

            print("손 좌표 : " + (leftHandPos.position.y - startPelvisPos) + " 오른손도!! : " + (rightHandPos.position.y - startPelvisPos));
            if (leftHandPos.position.y - startPelvisPos > 5f && rightHandPos.position.y - startPelvisPos > 5f && !isJumpingJack) // 4.6f
            {
                if (pv.IsMine)
                {
                    jumpingJackCount++;
                    isJumpingJack = true;
                    //print("!!!!!!!! 손 좌표 : " + leftHandPos.position.y + " 오른손도!! : " + rightHandPos.position.y);
                }
            }

            if (leftHandPos.position.y - startPelvisPos < 4f && rightHandPos.position.y - startPelvisPos < 4f && isJumpingJack) // 3.95f
            {
                if (pv.IsMine)
                {
                    isJumpingJack = false;
                    //print("?????????/ 손 좌표 : " + leftHandPos.position.y + " 오른손도!! : " + rightHandPos.position.y);
                }
            }

            Debug.LogError("점핑잭 횟수: " + jumpingJackCount);

        }
    }
}
