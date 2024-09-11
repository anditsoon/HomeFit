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
    public GameObject player;
    public bool isChanged;
    public GameObject ChangePopup;
    Animator anim;
    

    public int rotationSensitive;
    private float mouseXNum = 0;

    public TMP_Text all;



    public string Backpack = null;
    public string Body = "Meshes/Body/1";
    public string Eyebrow = null;
    public string Glasses = null;
    public string Glove = null;
    public string Hair = null;
    public string Hat = null;
    public string Mustache = null;
    public string Outerwear = null;
    public string Pants = null;
    public string Shoe = null;

    // Start is called before the first frame update
    void Start()
    {


        text_Backpacks.color = Color.magenta;
        all.color = Color.magenta;
        anim = player.GetComponent<Animator>();
        anim.CrossFade("Idle", 0f);

        AvatarInfo.instance.SettingAvatar();

        Backpack = AvatarInfo.instance.Backpack;
        Body = AvatarInfo.instance.Body;
        Eyebrow = AvatarInfo.instance.Eyebrow;
        Glasses = AvatarInfo.instance.Glasses; ;
        Glove = AvatarInfo.instance.Glove;
        Hair = AvatarInfo.instance.Hair;
        Hat = AvatarInfo.instance.Hat;
        Mustache = AvatarInfo.instance.Mustache;
        Outerwear = AvatarInfo.instance.Outerwear;
        Pants = AvatarInfo.instance.Pants;
        Shoe = AvatarInfo.instance.Shoe;
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

    public void AlamIsChanged()
    {
        if (!isChanged)
        {
            ReturnHome();
        }
        else
        {
            ChangePopup.SetActive(true);
        }

    }

    public void ReturnHome()
    {
        SceneManager.LoadScene(1);
    }

    public void RetunHomeYes()
    {
        SettingtoAvar();
        SceneManager.LoadScene(1);
    }

    public void SettingtoAvar()
    {
        AvatarInfo.instance.Backpack = Backpack;
        AvatarInfo.instance.Body = Body;
        AvatarInfo.instance.Eyebrow = Eyebrow;
        AvatarInfo.instance.Glasses = Glasses;
        AvatarInfo.instance.Glove = Glove;
        AvatarInfo.instance.Hair = Hair;
        AvatarInfo.instance.Hat = Hat;
        AvatarInfo.instance.Mustache = Mustache;
        AvatarInfo.instance.Outerwear = Outerwear;
        AvatarInfo.instance.Pants = Pants;
        AvatarInfo.instance.Shoe = Shoe;
        isChanged = false;
    }
}
