using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLobbyUIManager : MonoBehaviour
{

    public string Category = "";
    public int People = 0;

    // 카테고리 설정

    public void ButtonSquat()
    {
        Category = "Squat";
    }

    public void ButtonJumping()
    {
        Category = "Jumping";
    }

    public void ButtonPushUp()
    {
        Category = "PushUp";
    }

    // 인원 수 설정

    public void ButtonPeople1()
    {
        People = 1;
    }
    public void ButtonPeople2()
    {
        People = 2;
    }
    public void ButtonPeople3()
    {
        People = 3;
    }
    public void ButtonPeople4()
    {
        People = 4;
    }

    //방만들기
    public void ButtonMakingRoom()
    {

    }

    // 방입장
    public void ButtonEnterRoom()
    {

    }
}
