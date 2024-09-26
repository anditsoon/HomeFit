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
    public GameObject prevCheckButton;
    Animator anim;
    public Animator animUI;
    

    public int rotationSensitive;
    private float mouseXNum = 0;



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

    public GameObject CamPosOpening;
    public GameObject CamPosEnding;
    public GameObject endingDestroyUI1;
    public GameObject endingDestroyUI2;
    Color SelectingColor = Color.blue;

    bool Ending = false;


    // Start is called before the first frame update
    void Start()
    {
        

        text_Backpacks.color = SelectingColor;
        anim = player.GetComponent<Animator>();
        anim.CrossFade("Idle", 0f);
        JSWSoundManager.Get().PlayBgmSound(JSWSoundManager.EBgmType.BGM_AVAR);
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_SCENEMOVE);

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

        if (!Ending)
        {
            // -0.5, 2, -10
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, CamPosOpening.transform.position, Time.deltaTime * 10);
        }
        else
        {
            // -2 -0.2 -4
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, CamPosEnding.transform.position, Time.deltaTime * 10);
        }



    }

    public void ChangeTextColorBackPacks()
    {
        UIAllReset();
        text_Backpacks.color = SelectingColor;
    }

    public void ChangeTextColorBody()
    {
        UIAllReset();
        text_body.color = SelectingColor;
    }
    public void ChangeTextColorEyebrow()
    {
        UIAllReset();
        text_Eyebrow.color = SelectingColor;
    }
    public void ChangeTextColorGlasses()
    {
        UIAllReset();
        text_Glasses.color = SelectingColor;
    }
    public void ChangeTextColorGlove()
    {
        UIAllReset();
        text_Glove.color = SelectingColor;
    }
    public void ChangeTextColorHair()
    {
        UIAllReset();
        text_Hair.color = SelectingColor;
    }
    public void ChangeTextColorHat()
    {
        UIAllReset();
        text_Hat.color = SelectingColor;
    }
    public void ChangeTextColorMustache()
    {
        UIAllReset();
        text_Mustache.color = SelectingColor;
    }
    public void ChangeTextColorOutwear()
    {
        UIAllReset();
        text_Outwear.color = SelectingColor;
    }
    public void ChangeTextColorPants()
    {
        UIAllReset();
        text_Pants.color = SelectingColor;
    }
    public void ChangeTextColorShoe()
    {
        UIAllReset();
        text_Shoe.color = SelectingColor;
    }

    
    private void UIAllReset()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_BTN);
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
    }

    public void AlamIsChanged()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_BTN);
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
        StartCoroutine(ending());
    }

    public void RetunHomeYes()
    {
        SettingtoAvar();
        StartCoroutine(ending());
    }

    IEnumerator ending()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_DECOEND);
        endingDestroyUI1.SetActive(false);
        endingDestroyUI2.SetActive(false);
        player.transform.forward = Camera.main.transform.forward * -1;
        ChangePopup.SetActive(false);
        anim.CrossFade("Win", 0f);
        animUI.SetBool("isEnd", true);
        Ending = true;
        yield return new WaitForSeconds(1.3f);
        //SceneManager.LoadScene(1);
        SceneManager.LoadSceneAsync(1);
    }

    public void SettingtoAvar()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_BTN);
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
