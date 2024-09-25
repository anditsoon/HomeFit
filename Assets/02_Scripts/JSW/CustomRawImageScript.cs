using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomRawImageScript : MonoBehaviour
{
    public string itemPath;
    public string avarItem;
    public Button btn_Image;
    public GameObject Player;
    public Mesh mesh;
    public GameObject checking;
    Animator anim;
    GameObject AvatarUiManage;


    // Start is called before the first frame update
    void Start()
    {
        
        Player = GameObject.Find("Player");
        AvatarUiManage = GameObject.Find("AvartarUIManager");
        anim = Player.GetComponent<Animator>();
        anim.SetBool("Wearing", true);
        btn_Image = GetComponent<Button>();
        btn_Image.onClick.AddListener(() =>
        {
            SettingAvar();
        });
    }

    public void SetItemPath(string path, string item)
    {
        itemPath = path;
        avarItem = item;
    }

    public void SettingAvar()
    {
        if (AvatarUiManage.GetComponent<AvartarUIManager>().prevCheckButton != null)
        {
            AvatarUiManage.GetComponent<AvartarUIManager>().prevCheckButton.GetComponent<CustomRawImageScript>().checking.SetActive(false);
        }
        checking.SetActive(true);
        AvatarUiManage.GetComponent<AvartarUIManager>().prevCheckButton = gameObject;
        GameObject itematPlayer = GameObject.Find(avarItem);
        anim.CrossFade("Jumping", 0f);
        if (itemPath != "")
        {
            itematPlayer.GetComponent<SkinnedMeshRenderer>().sharedMesh = Resources.Load<Mesh>(itemPath);
            SetAvarInfo(itemPath, avarItem);
        }
        else
        {
            itematPlayer.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            SetAvarInfo("", avarItem);
        }
    }

    

    public void SetAvarInfo(string itemP, string avarN)
    {
        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_AVATARDECO);
        AvartarUIManager AUM = AvatarUiManage.GetComponent<AvartarUIManager>();
        AUM.isChanged = true;

        if (avarN == "Backpack")
        {
            AUM.Backpack = itemP;
        }
        if (avarN == "Body")
        {
            AUM.Body = itemP;
        }
        if (avarN == "Eyebrow")
        {
            AUM.Eyebrow = itemP;
        }
        if (avarN == "Glasses")
        {
            AUM.Glasses = itemP;
        }
        if (avarN == "Glove")
        {
            AUM.Glove = itemP;
        }
        if (avarN == "Hair")
        {
            AUM.Hair = itemP;
        }
        if (avarN == "Hat")
        {
            AUM.Hat = itemP;
        }
        if (avarN == "Mustache")
        {
            AUM.Mustache = itemP;
        }
        if (avarN == "Outerwear")
        {
            AUM.Outerwear = itemP;
        }
        if (avarN == "Pants")
        {
            AUM.Pants = itemP;
        }
        if (avarN == "Shoe")
        {
            AUM.Shoe = itemP;
        }
    }

}
