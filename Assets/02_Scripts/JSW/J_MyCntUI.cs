using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class J_MyCntUI : MonoBehaviour
{
    Vector3 dir;
    Vector3 startPos;
    float startFontSize;
    public RectTransform canvasRectTransform;
    Y_PlayerUIManager playerUIManager;
    public TMP_Text squatTxt;

    float time;
    public float _fadeTime = 1.5f;

    private void Start()
    {
        startPos = transform.position;
        startFontSize = squatTxt.fontSize;
        playerUIManager = GetComponentInParent<Y_PlayerUIManager>();
        dir = Vector3.up;
        //resetAnim();
    }

    void Update()
    {
        // 숫자가 바뀔 때에는 방향 리셋하고 처음 위치에서 처음 색과 폰트 크기로 시작
        if (playerUIManager.squatReset)
        {
            resetAnim();
            playerUIManager.squatReset = false;
        }
        else
        {
            // 위 방향으로 이동하기
            transform.Translate(dir * 0.5f * Time.deltaTime);

            // 점점 페이드 아웃
            if (time < _fadeTime)
            {
                squatTxt.color = new Color(0.4745098f, 0.8941177f, 0.5128013f, 1f - time / _fadeTime);
            }
            else
            {
                time = 0;
                this.gameObject.SetActive(false);
            }

            // 글씨 점점 커지게
            squatTxt.fontSize = startFontSize * (1 + time) * 0.7f;
        }
 
        time += Time.deltaTime;
    }

    public void resetAnim()
    {
        transform.position = startPos;
        // dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -1).normalized;
        squatTxt.color = new Color(0.8930817f, 0.862123f, 0.4746251f, 1f);
        squatTxt.fontSize = startFontSize;
        time = 0;
    }
}
