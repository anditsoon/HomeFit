using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarInfo : MonoBehaviour
{
    public static AvatarInfo instance;
    GameObject player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // 씬 전환이 되도 게임 오브젝트를 파괴하고 싶지않다.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 유저 기본 정보
    public string NickName = null;
    public string Birthday = null;
    public float Height = 0;
    public float Weight = 0;


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

    public void SettingAvatar()
    {
        string[] itemN = new string[11];
        itemN[0] = Backpack;
        itemN[1] = Body;
        itemN[2] = Eyebrow;
        itemN[3] = Glasses;
        itemN[4] = Glove;
        itemN[5] = Hair;
        itemN[6] = Hat;
        itemN[7] = Mustache;
        itemN[8] = Outerwear;
        itemN[9] = Pants;
        itemN[10] = Shoe;

        player = GameObject.Find("Player");
        for (int i= 0; i < 11;i++)
        {
            if (itemN[i] != null)
            {
                player.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh = Resources.Load<Mesh>(itemN[i]);
            }
            else
            {
                player.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            }
        }
        
    }

    public void SettingAvatarInPlay()
    {
        string[] itemN = new string[11];
        itemN[0] = Backpack;
        itemN[1] = Body;
        itemN[2] = Eyebrow;
        itemN[3] = Glasses;
        itemN[4] = Glove;
        itemN[5] = Hair;
        itemN[6] = Hat;
        itemN[7] = Mustache;
        itemN[8] = Outerwear;
        itemN[9] = Pants;
        itemN[10] = Shoe;

        player = GameObject.Find("Player");
        for (int i = 0; i < 11; i++)
        {
            if (itemN[i] != null)
            {
                player.transform.GetChild(0).GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh = Resources.Load<Mesh>(itemN[i]);
            }
            else
            {
                player.transform.GetChild(0).GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            }
        }

    }
}
