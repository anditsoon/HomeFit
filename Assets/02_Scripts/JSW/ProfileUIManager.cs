using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileUIManager : MonoBehaviour
{
    public Camera renderCamera;
    public RawImage chaRawImage;
    GameObject player;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = player.GetComponent<Animator>();
        anim.CrossFade("A_Poses", 0f);
        StartCoroutine("SetProfilePic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    IEnumerator SetProfilePic()
    {
        yield return new WaitForSeconds(0.03f);
        RenderTexture renderTexture = new RenderTexture(256, 256, 16);
        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();
        chaRawImage.texture = renderTexture;
        renderCamera.targetTexture = new RenderTexture(256, 256, 16);
    }
}

