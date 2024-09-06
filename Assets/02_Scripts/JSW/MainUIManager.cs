using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    public GameObject playerPos;
    GameObject player;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = player.GetComponent<Animator>();
        anim.CrossFade("Dance",0f);
        player.transform.position = playerPos.transform.position;
        player.transform.rotation = playerPos.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveAvatarScene()
    {
        SceneManager.LoadScene("avatarScene");
    }
    public void MoveProfileScene()
    {
        SceneManager.LoadScene("ProfileScene");
    }
}
