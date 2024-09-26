using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserItemManager : MonoBehaviour
{
    private readonly string UpdateItemUrl = "https://125.132.216.190:12502/api/character/";

    private void Start()
    {
        // SSL 인증서 검증 우회 설정
        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
    }

    public void OnSaveItemButtonClick()
    {
        string backpack = AvatarInfo.instance.Backpack;
        string body = AvatarInfo.instance.Body;
        string eyebrow = AvatarInfo.instance.Eyebrow;
        string glasses = AvatarInfo.instance.Glasses;
        string glove = AvatarInfo.instance.Glove;
        string hair = AvatarInfo.instance.Hair;
        string hat = AvatarInfo.instance.Hat;
        string mustache = AvatarInfo.instance.Mustache;
        string outerwear = AvatarInfo.instance.Outerwear;
        string pants = AvatarInfo.instance.Pants;
        string shoe = AvatarInfo.instance.Shoe;
        StartCoroutine(UpdateItemCoroutine(backpack, body, eyebrow, glasses, glove, hair, hat, mustache, outerwear, pants, shoe));
    }

    IEnumerator UpdateItemCoroutine(string _backpack, string _body, string _eyebrow, string _glasses, string _glove,
        string _hair, string _hat, string _mustache, string _outerwear, string _pants, string _shoe)
    {
        yield return new WaitForSeconds(0.1f);

        string jwtToken = PlayerPrefs.GetString("jwtToken");
        string userId = PlayerPrefs.GetString("userId");
        string url = UpdateItemUrl + userId;

        string jsonBody = JsonUtility.ToJson(new UpdateItemData
        {
            backpack = long.Parse(_backpack.Replace("Meshes/Backpack/", "")),
            body = long.Parse(_body.Replace("Meshes/Body/", "")),
            eyebrow = long.Parse(_eyebrow.Replace("Meshes/Eyebrow/", "")),
            glasses = long.Parse(_glasses.Replace("Meshes/Glasses/", "")),
            glove = long.Parse(_glove.Replace("Meshes/Glove/", "")),
            hair = long.Parse(_hair.Replace("Meshes/Hair/", "")),
            hat = long.Parse(_hat.Replace("Meshes/Hat/", "")),
            mustache = long.Parse(_mustache.Replace("Meshes/Mustache/", "")),
            outerwear = long.Parse(_outerwear.Replace("Meshes/Outerwear/", "")),
            pants = long.Parse(_pants.Replace("Meshes/Pants/", "")),
            shoe = long.Parse(_shoe.Replace("Meshes/Shoe/", ""))
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        using (UnityWebRequest www = new UnityWebRequest(url, "PUT"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            www.certificateHandler = new BypassCertificate();

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("아이템 업데이트 실패: " + www.error);
            }
            else
            {
                string responseBody = www.downloadHandler.text;
                Debug.Log("아이템 업데이트 성공. 서버 응답: " + responseBody);
            }
        }
    }
}
