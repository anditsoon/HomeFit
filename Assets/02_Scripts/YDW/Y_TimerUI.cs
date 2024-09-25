using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Y_TimerUI : MonoBehaviour
{
    public bool hasStart;
    public PlaySceneManager PSM;

    public TMP_Text timerText;
    public float elapsedTime = 0f;

    public float duration;

    public GameObject timerPanel;
    public GameObject timerTxt;
    public GameObject resultPanel;

    public Y_UIManager uiManager;
    public WinnerCloseup win;
    CanvasRenderer[] canvasRenderers;
    public bool allReadyGo;

    private bool isFlashing = false;

    Color oldColor;


    private DataSender dataSender;
    public WinnerCloseup winnerCloseUp;


    void Start()
    {
        dataSender = FindObjectOfType<DataSender>();

        duration = 30;
        oldColor = timerPanel.GetComponent<Image>().color;
        oldColor.a = 0.72f;
    }

    void Update()
    {

        if(hasStart && allReadyGo)
        {
            timerPanel.GetComponent<Image>().color = oldColor;

            timerTxt.SetActive(true);

            elapsedTime += Time.deltaTime;

            // 경과 시간을 분:초 형식으로 변환
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            string timeText = string.Format("{0:00}:{1:00}",
                timeSpan.Minutes, timeSpan.Seconds);

            timerText.text = timeText;

            // 시간이 5초 이하일 때 글씨 깜박이고 빨간색으로 변경
            if (elapsedTime >= duration - 5 && elapsedTime < duration)
            {
                if (!isFlashing)
                {
                    StartCoroutine(uiManager.MoveUI("5초 남았어요!"));
                    JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_CLOCKING);
                    JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_GAMEOVER);
                    JSWSoundManager.Get().AudioSourceEtc(1.3f);
                    StartCoroutine(FlashTimerText());
                }
            }

            if (elapsedTime > duration)
            {
                
                timerPanel.SetActive(false); // 타이머 없애고
                resultPanel.SetActive(true); // 결과창 서서히 띄운다

                win.PerformWinnerCloseup();

                // 결과창 서서히 띄운다 2
                canvasRenderers = resultPanel.GetComponentsInChildren<CanvasRenderer>();
                StartCoroutine(uiManager.IncreaseAlpha(canvasRenderers));

                PhotonView[] allPhotonViews = FindObjectsOfType<PhotonView>();

                foreach (PhotonView view in allPhotonViews)
                {
                    // IK 리깅 꺼 줌
                    view.gameObject.GetComponentInChildren<RigBuilder>().enabled = false;

                    // 우승한 플레이어는 승리한 포즈, 패배한 플레이어는 쓰러지게
                    if(view.Owner.NickName == winnerCloseUp.winnerName)
                    {
                        view.gameObject.GetComponentInChildren<Animator>().SetBool("IsWin", true);
                    }
                    else
                    {
                        view.gameObject.GetComponentInChildren<Animator>().SetBool("IsLose", true);
                    }    
                    
                }

                // 이 변수를 스쿼트/점핑잭 세는 스크립트에서 호출하고 해당 스크립트에서 더 이상 횟수 세지 못하게 한다
                hasStart = false;

                TimerEnded();
            }
        }

        void TimerEnded()
        {
            if (dataSender != null)
            {
                dataSender.SendDataToServer();
            }
        }

        // 타이머 텍스트 깜박이기
        IEnumerator FlashTimerText()
        {
            isFlashing = true;
            Color originalColor = timerText.color;

            while (elapsedTime < duration)
            {
                timerText.color = Color.red;
                yield return new WaitForSeconds(0.5f);
                timerText.color = originalColor;
                yield return new WaitForSeconds(0.5f);
            }

            JSWSoundManager.Get().AudioSourceEtc(1f);
            JSWSoundManager.Get().PlayBgmSound(JSWSoundManager.EBgmType.BGM_end);
            JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_SHOUTING);
            timerText.color = originalColor; // 타이머 종료 후 원래 색으로 복구
            isFlashing = false;
        }
    }

    


}


// 클릭시 함수를 rpc로 쏘기