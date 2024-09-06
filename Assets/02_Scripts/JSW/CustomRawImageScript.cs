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
        print(itemPath);
        GameObject itematPlayer = GameObject.Find(avarItem);
        //print(itematPlayer);
        //print(itematPlayer.GetComponent<SkinnedMeshRenderer>().sharedMesh.name);
        //print(itemPath);
        if (itemPath != "")
        {
            itematPlayer.GetComponent<SkinnedMeshRenderer>().sharedMesh = Resources.Load<Mesh>(itemPath);
        }
        else
        {
            itematPlayer.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
        }
       
    }
}
