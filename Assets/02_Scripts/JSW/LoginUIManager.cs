using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoginUIManager : MonoBehaviour
{
    public GameObject playerNameInput;
    public GameObject playerBirthInput;
    public GameObject playerWeightInput;
    public GameObject playerHeightInput;

    private string playerName = null;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Number1Button()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
    public void Number2Button()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
    }
    public void Number3Button()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
    }
    public void Number4ButtonKKL()
    {
        // 카카오 로그인
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(true);
    }
    public void Number5ButtoL()
    {
        AvatarInfo.instance.NickName = playerNameInput.GetComponent<TMP_InputField>().text;
        AvatarInfo.instance.Birthday = playerBirthInput.GetComponent<TMP_InputField>().text;
        AvatarInfo.instance.Height = float.Parse(playerHeightInput.GetComponent<TMP_InputField>().text);
        AvatarInfo.instance.Weight = float.Parse(playerWeightInput.GetComponent<TMP_InputField>().text);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(true);
    }

    public void Number6Button()
    {

        transform.GetChild(5).gameObject.SetActive(false);
        transform.GetChild(6).gameObject.SetActive(true);
    }
    public void Number7Button()
    {
        transform.GetChild(6).gameObject.SetActive(false);
        transform.GetChild(7).gameObject.SetActive(true);
    }
    public void Number8Button()
    {
        //next Scene
        //print("다음 씬 꾸미기 씬");
        SceneManager.LoadScene("avatarScene");
    }
}
