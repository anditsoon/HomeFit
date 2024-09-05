using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AvartarUIManager : MonoBehaviour
{
    public TMP_Text text_Backpacks;
    public TMP_Text text_body;
    public TMP_Text text_Eyebrow;
    public TMP_Text text_Glasses;
    public TMP_Text text_Glove;
    public TMP_Text text_Hair;
    public TMP_Text text_Hat;
    public TMP_Text text_Mustache;
    public TMP_Text text_Outwear;
    public TMP_Text text_Pants;
    public TMP_Text text_Shoe;
    public GameObject playerPos;
    GameObject player;
    Animator anim;
    

    public int rotationSensitive;
    private float mouseXNum = 0;

    public TMP_Text all;



    // Start is called before the first frame update
    void Start()
    {
        text_Backpacks.color = Color.magenta;
        all.color = Color.magenta;
        player = GameObject.Find("Player");
        anim = player.GetComponent<Animator>();
        anim.CrossFade("A_Poses", 0f);

        player.transform.position = playerPos.transform.position;
        player.transform.rotation = playerPos.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mouseXNum = Input.GetAxis("Mouse X") * rotationSensitive * -1;
            player.transform.Rotate(0, mouseXNum, 0);
        }
    }

    public void ChangeTextColorBackPacks()
    {
        UIAllReset();
        text_Backpacks.color = Color.magenta;
    }

    public void ChangeTextColorBody()
    {
        UIAllReset();
        text_body.color = Color.magenta;
    }
    public void ChangeTextColorEyebrow()
    {
        UIAllReset();
        text_Eyebrow.color = Color.magenta;
    }
    public void ChangeTextColorGlasses()
    {
        UIAllReset();
        text_Glasses.color = Color.magenta;
    }
    public void ChangeTextColorGlove()
    {
        UIAllReset();
        text_Glove.color = Color.magenta;
    }
    public void ChangeTextColorHair()
    {
        UIAllReset();
        text_Hair.color = Color.magenta;
    }
    public void ChangeTextColorHat()
    {
        UIAllReset();
        text_Hat.color = Color.magenta;
    }
    public void ChangeTextColorMustache()
    {
        UIAllReset();
        text_Mustache.color = Color.magenta;
    }
    public void ChangeTextColorOutwear()
    {
        UIAllReset();
        text_Outwear.color = Color.magenta;
    }
    public void ChangeTextColorPants()
    {
        UIAllReset();
        text_Pants.color = Color.magenta;
    }
    public void ChangeTextColorShoe()
    {
        UIAllReset();
        text_Shoe.color = Color.magenta;
    }

    
    private void UIAllReset()
    {
        text_Backpacks.color = Color.black;
        text_body.color = Color.black;
        text_Eyebrow.color = Color.black;
        text_Glasses.color = Color.black;
        text_Glove.color = Color.black;
        text_Hair.color = Color.black;
        text_Hat.color = Color.black;
        text_Mustache.color = Color.black;
        text_Outwear.color = Color.black;
        text_Pants.color = Color.black;
        text_Shoe.color = Color.black;

        all.color = Color.magenta;
    }


    public void ReturnHome()
    {
        SceneManager.LoadScene(1);
    }
}
