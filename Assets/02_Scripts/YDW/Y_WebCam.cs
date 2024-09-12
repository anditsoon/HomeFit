using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Y_WebCam : MonoBehaviour
{
    WebCamTexture webCamTexture;

    public RawImage uiCamView;
    void Start()
    {
        WebCamDevice webCamDevice = WebCamTexture.devices[0];
        webCamTexture = new WebCamTexture(webCamDevice.name, 720, 720, 30);
        webCamTexture.Play();
        uiCamView.texture = webCamTexture;
        foreach(var c in WebCamTexture.devices)
        {
            print(c.name);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
