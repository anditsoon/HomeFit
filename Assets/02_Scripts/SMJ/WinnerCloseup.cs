using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerCloseup : MonoBehaviour
{
    public Y_RankUI rankUI;
    public Camera mainCamera;
    public float closeUpDistance = 2f; // 클로즈업 시 카메라와 대상 사이의 거리
    public float moveTime = 1f; // 클로즈업에 걸리는 시간 (초)

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        rankUI = FindObjectOfType<Y_RankUI>();
    }

    public void PerformWinnerCloseup()
    {
        Transform winnerTransform = FindWinnerTransform();

        if (winnerTransform != null)
        {
            StartCoroutine(SmoothCloseUp(winnerTransform));
        }
    }

    private System.Collections.IEnumerator SmoothCloseUp(Transform target)
    {
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;

        Vector3 targetPosition = target.position - target.forward * closeUpDistance;
        Quaternion targetRotation = Quaternion.LookRotation(target.position - targetPosition);

        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
    }

    private Transform FindWinnerTransform()
    {
        var players = FindObjectsOfType<PhotonView>();
        foreach (var player in players)
        {
            if (player.Owner.NickName == rankUI.name1st.text)
            {
                return player.transform;
            }
        }
        return null;
    }

}
