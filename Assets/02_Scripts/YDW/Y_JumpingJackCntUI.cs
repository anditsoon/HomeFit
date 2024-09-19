using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Y_JumpingJackCntUI : MonoBehaviour
{
    Vector3 dir;
    Vector3 startPos;
    float startFontSize;
    public RectTransform canvasRectTransform;
    Y_PlayerUIManager playerUIManager;
    public TMP_Text jumpingJackTxt;

    float time;
    public float _fadeTime = 1.5f;

    private void Start()
    {
        startPos = transform.position;
        startFontSize = jumpingJackTxt.fontSize;
        playerUIManager = GetComponentInParent<Y_PlayerUIManager>();
        dir = Vector3.up;
        //resetAnim();
    }

    void Update()
    {
        // 숫자가 바뀔 때에는 방향 리셋하고 처음 위치에서 처음 색과 폰트 크기로 시작
        if (playerUIManager.jumpingJackReset)
        {
            resetAnim();
            playerUIManager.jumpingJackReset = false;
        }
        else
        {
            // 위 방향으로 이동하기
            transform.Translate(dir * 0.5f * Time.deltaTime);

            // 점점 페이드 아웃
            if (time < _fadeTime)
            {
                jumpingJackTxt.color = new Color(0.6666666f, 0.1949248f, 0.03144645f, 1f - time / _fadeTime);
            }
            else
            {
                time = 0;
                this.gameObject.SetActive(false);
            }

            // 글씨 점점 커지게
            jumpingJackTxt.fontSize = startFontSize * (1 + time) * 0.7f;
        }

        time += Time.deltaTime;
    }

    public void resetAnim()
    {
        transform.position = startPos;
        // dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -1).normalized;
        jumpingJackTxt.color = new Color(0.6666666f, 0.1949248f, 0.03144645f, 1f);
        jumpingJackTxt.fontSize = startFontSize;
        time = 0;
    }
}

