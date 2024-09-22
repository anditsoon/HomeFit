using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerCloseup : MonoBehaviour
{
    public Y_RankUI rankUI;
    public Camera mainCamera;
    public float closeUpDuration = 5f; // 클로즈업 지속 시간 (초)
    public float closeUpDistance = 2f; // 클로즈업 시 카메라와 대상 사이의 거리

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private float closeUpStartTime;
    private bool isClosingUp = false;

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
            // 현재 카메라 위치와 회전 저장
            originalCameraPosition = mainCamera.transform.position;
            originalCameraRotation = mainCamera.transform.rotation;

            // 클로즈업 시작
            isClosingUp = true;
            closeUpStartTime = Time.time;

            // 코루틴으로 부드러운 이동 구현 가능
            StartCoroutine(SmoothCloseUp(winnerTransform));
        }
    }

    private System.Collections.IEnumerator SmoothCloseUp(Transform target)
    {
        Vector3 targetPosition = target.position - target.forward * closeUpDistance;
        Quaternion targetRotation = Quaternion.LookRotation(target.position - targetPosition);

        float elapsedTime = 0f;
        float moveTime = 1f; // 1초 동안 이동

        while (elapsedTime < moveTime)
        {
            mainCamera.transform.position = Vector3.Lerp(originalCameraPosition, targetPosition, elapsedTime / moveTime);
            mainCamera.transform.rotation = Quaternion.Slerp(originalCameraRotation, targetRotation, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
    }

    private void Update()
    {
        // 클로즈업 시간이 지나면 원래 카메라 위치로 돌아가기
        if (isClosingUp && Time.time - closeUpStartTime > closeUpDuration)
        {
            StartCoroutine(SmoothReturnToOriginal());
            isClosingUp = false;
        }
    }

    private System.Collections.IEnumerator SmoothReturnToOriginal()
    {
        Vector3 currentPosition = mainCamera.transform.position;
        Quaternion currentRotation = mainCamera.transform.rotation;

        float elapsedTime = 0f;
        float moveTime = 1f; // 1초 동안 이동

        while (elapsedTime < moveTime)
        {
            mainCamera.transform.position = Vector3.Lerp(currentPosition, originalCameraPosition, elapsedTime / moveTime);
            mainCamera.transform.rotation = Quaternion.Slerp(currentRotation, originalCameraRotation, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalCameraPosition;
        mainCamera.transform.rotation = originalCameraRotation;
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
