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


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        btn_Image = GetComponent<Button>();
        btn_Image.onClick.AddListener(() =>
        {
            SettingAvar();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItemPath(string path, string item)
    {
        itemPath = path;
        avarItem = item;
    }

    public void SettingAvar()
    {
        GameObject itematPlayer = GameObject.Find(avarItem);
  
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
        if (avarN == "Backpack")
        {
            AvatarInfo.instance.Backpack = itemP;
        }
        if (avarN == "Body")
        {
            AvatarInfo.instance.Body = itemP;
        }
        if (avarN == "Eyebrow")
        {
            AvatarInfo.instance.Eyebrow = itemP;
        }
        if (avarN == "Glasses")
        {
            AvatarInfo.instance.Glasses = itemP;
        }
        if (avarN == "Glove")
        {
            AvatarInfo.instance.Glove = itemP;
        }
        if (avarN == "Hair")
        {
            AvatarInfo.instance.Hair = itemP;
        }
        if (avarN == "Hat")
        {
            AvatarInfo.instance.Hat = itemP;
        }
        if (avarN == "Mustache")
        {
            AvatarInfo.instance.Mustache = itemP;
        }
        if (avarN == "Outerwear")
        {
            AvatarInfo.instance.Outerwear = itemP;
        }
        if (avarN == "Pants")
        {
            AvatarInfo.instance.Pants = itemP;
        }
        if (avarN == "Shoe")
        {
            AvatarInfo.instance.Shoe = itemP;
        }
    }

}
