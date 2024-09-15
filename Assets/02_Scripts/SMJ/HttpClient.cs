using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient : MonoBehaviour
{
    private static HttpClient instance;
    public static HttpClient Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("HttpClient");
                instance = go.AddComponent<HttpClient>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    public IEnumerator Get(string url, Action<string> onSuccess, Action<string> onError, Dictionary<string, string> headers = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            AddHeaders(webRequest, headers);
            yield return webRequest.SendWebRequest();
            HandleResponse(webRequest, onSuccess, onError);
        }
    }

    public IEnumerator Post(string url, WWWForm form, Action<string> onSuccess, Action<string> onError, Dictionary<string, string> headers = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            AddHeaders(webRequest, headers);
            yield return webRequest.SendWebRequest();
            HandleResponse(webRequest, onSuccess, onError);
        }
    }

    public IEnumerator PostJson(string url, string jsonBody, Action<string> onSuccess, Action<string> onError, Dictionary<string, string> headers = null)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            AddHeaders(webRequest, headers);
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            HandleResponse(webRequest, onSuccess, onError);
        }
    }

    private void AddHeaders(UnityWebRequest webRequest, Dictionary<string, string> headers)
    {
        if (headers != null)
        {
            foreach (var header in headers)
            {
                webRequest.SetRequestHeader(header.Key, header.Value);
            }
        }
    }

    private void HandleResponse(UnityWebRequest webRequest, Action<string> onSuccess, Action<string> onError)
    {
        if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
            webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            onError?.Invoke(webRequest.error);
        }
        else
        {
            onSuccess?.Invoke(webRequest.downloadHandler.text);
        }
    }
}