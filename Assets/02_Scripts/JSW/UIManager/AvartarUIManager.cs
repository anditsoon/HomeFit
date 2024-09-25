using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AvartarUIManager : MonoBehaviour
{
    public GameObject G_Backpacks;
    public GameObject G_body;
    public GameObject G_Eyebrow;
    public GameObject G_Glasses;
    public GameObject G_Glove;
    public GameObject G_Hair;
    public GameObject G_Hat;
    public GameObject G_Mustache;
    public GameObject G_Outwear;
    public GameObject G_Pants;
    public GameObject G_Shoe;


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


        G_Backpacks.transform.GetChild(0).gameObject.SetActive(true);
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
        G_Backpacks.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ChangeTextColorBody()
    {
        UIAllReset();
        G_body.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ChangeTextColorEyebrow()
    {
        UIAllReset();
        G_Eyebrow.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ChangeTextColorGlasses()
    {
        UIAllReset();
        G_Glasses.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ChangeTextColorGlove()
    {
        UIAllReset();
        G_Glove.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ChangeTextColorHair()
    {
        UIAllReset();
        G_Hair.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ChangeTextColorHat()
    {
        UIAllReset();
        G_Hat.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ChangeTextColorMustache()
    {
        UIAllReset();
        G_Mustache.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ChangeTextColorOutwear()
    {
        UIAllReset();
        G_Outwear.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ChangeTextColorPants()
    {
        UIAllReset();
        G_Pants.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ChangeTextColorShoe()
    {
        UIAllReset();
        G_Shoe.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void UIAllReset()
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_BTN);
        G_Backpacks.transform.GetChild(0).gameObject.SetActive(false);
        G_body.transform.GetChild(0).gameObject.SetActive(false);
        G_Eyebrow.transform.GetChild(0).gameObject.SetActive(false);
        G_Glasses.transform.GetChild(0).gameObject.SetActive(false);
        G_Glove.transform.GetChild(0).gameObject.SetActive(false);
        G_Hair.transform.GetChild(0).gameObject.SetActive(false);
        G_Hat.transform.GetChild(0).gameObject.SetActive(false);
        G_Mustache.transform.GetChild(0).gameObject.SetActive(false);
        G_Outwear.transform.GetChild(0).gameObject.SetActive(false);
        G_Pants.transform.GetChild(0).gameObject.SetActive(false);
        G_Shoe.transform.GetChild(0).gameObject.SetActive(false);
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
