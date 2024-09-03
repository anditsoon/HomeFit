using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AvartarUIManager : MonoBehaviour
{
    public TMP_Text text_Face;
    public TMP_Text text_Hair;
    public TMP_Text text_UpperC;
    public TMP_Text text_UnderC;
    public TMP_Text text_OnepieceC;
    public TMP_Text text_ShoesC;
    public TMP_Text text_AckC;
    public TMP_Text text_headC;
    public TMP_Text text_specC;

    public TMP_Text all;



    // Start is called before the first frame update
    void Start()
    {
        text_Face.color = Color.magenta;
        all.color = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTextColor1()
    {
        UIAllReset();
        text_Face.color = Color.magenta;
    }

    public void ChangeTextColor2()
    {
        UIAllReset();
        text_Hair.color = Color.magenta;
    }
    public void ChangeTextColor3()
    {
        UIAllReset();
        text_UpperC.color = Color.magenta;
    }
    public void ChangeTextColor4()
    {
        UIAllReset();
        text_UnderC.color = Color.magenta;
    }
    public void ChangeTextColor5()
    {
        UIAllReset();
        text_OnepieceC.color = Color.magenta;
    }
    public void ChangeTextColor6()
    {
        UIAllReset();
        text_ShoesC.color = Color.magenta;
    }
    public void ChangeTextColor7()
    {
        UIAllReset();
        text_AckC.color = Color.magenta;
    }
    public void ChangeTextColor8()
    {
        UIAllReset();
        text_headC.color = Color.magenta;
    }
    public void ChangeTextColor9()
    {
        UIAllReset();
        text_specC.color = Color.magenta;
    }


    private void UIAllReset()
    {
        text_Face.color = Color.black;
        text_Hair.color = Color.black;
        text_UpperC.color = Color.black;
        text_UnderC.color = Color.black;
        text_OnepieceC.color = Color.black;
        text_ShoesC.color = Color.black;
        text_AckC.color = Color.black;
        text_headC.color = Color.black;
        text_specC.color = Color.black;

        all.color = Color.magenta;

    }

}
