﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MultiModelToImage : MonoBehaviour
{
    public Camera renderCamera; // 모델을 렌더링할 카메라
    //public RawImage[] displayImages; // 여러 모델을 표시할 UI 요소
    public GameObject[] models; // 렌더링할 3D 모델들
    public GameObject CustomRawImage;
    public Transform CustomContent;

    private Coroutine coroutine;
    public AvartarUIManager AUI;

    private RenderTexture[] renderTextures;
    bool ClickOkay = true;

    void Start()
    {
        AvarUIAllReset();
        StartCoroutine(SetAvarUI("Backpack"));
    }


    public void ChangeAvarBackpacks()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Backpack"));
    }

    public void ChangeAvarColorBody()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Body"));
    }
    public void ChangeAvarColorEyebrow()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Eyebrow"));
    }
    public void ChangeAvarColorGlasses()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Glasses"));
    }
    public void ChangeAvarColorGlove()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Glove"));
    }
    public void ChangeAvarColorHair()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Hair"));
    }
    public void ChangeAvarColorHat()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Hat"));
    }
    public void ChangeAvarColorMustache()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Mustache"));
    }
    public void ChangeAvarColorOutwear()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Outerwear"));
    }
    public void ChangeAvarColorPants()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Pants"));
    }
    public void ChangeAvarColorShoe()
    {
        AvarUIAllReset();
        coroutine = StartCoroutine(SetAvarUI("Shoe"));
    }

    private void AvarUIAllReset()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        for (int i =0; i < CustomContent.childCount;i++)
        {
            Destroy(CustomContent.GetChild(i).gameObject);
        }
        ClickOkay = true;
    }

    IEnumerator SetAvarUI(string avar)
    {
        if (ClickOkay == true)
        {
            ClickOkay = false;
            models = Resources.LoadAll<GameObject>("Prefabs/" + avar);
            renderTextures = new RenderTexture[models.Length];
            GameObject crii = Instantiate(CustomRawImage, CustomContent);
            crii.GetComponent<CustomRawImageScript>().SetItemPath("", avar);

            for (int i = 0; i < models.Length; i++)
            {
                if (models[i] != null)
                {
                    models[i] = Instantiate(models[i]);

                    GameObject cri = Instantiate(CustomRawImage, CustomContent);
                    RawImage ri = cri.GetComponent<RawImage>();
                    cri.GetComponent<CustomRawImageScript>().SetItemPath($"Meshes/{avar}/{i + 1}", avar);
                    CheckingSame(avar, $"Meshes/{avar}/{i + 1}", cri);
                    models[i].transform.localScale = models[i].transform.localScale * 120;

                    // 각 모델을 위한 Render Texture 생성
                    renderTextures[i] = new RenderTexture(256, 256, 16);
                    renderCamera.targetTexture = renderTextures[i];

                    // 모델 위치 조정 및 렌더링
                    PositionModel(models[i], avar);
                    renderCamera.Render();

                    // UI에 해당 Render Texture 할당
                    ri.texture = renderTextures[i];

                    models[i].SetActive(false);
                    Destroy(models[i]);

                    if (i == models.Length - 1)
                    {
                        renderCamera.targetTexture = new RenderTexture(256, 256, 16);
                    }

                    if (i % 22 == 0 && i != 0)
                    {
                        renderCamera.targetTexture = new RenderTexture(256, 256, 16);
                        yield return new WaitForSeconds(1f);
                    }
                }
                
            }
            ClickOkay = true;
        }
    }

    void CheckingSame(string avar, string name, GameObject rawimage)
    {
        if (avar == "Backpack" && AUI.Backpack == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
        if (avar == "Body" && AUI.Body == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
        if (avar == "Eyebrow" && AUI.Eyebrow == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
        if (avar == "Glasses"&& AUI.Glasses == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
        if (avar == "Glove" && AUI.Glove == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
        if (avar == "Hair" && AUI.Hair == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
        if (avar == "Hat" && AUI.Hat == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
        if (avar == "Mustache" && AUI.Mustache == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
        if (avar == "Outerwear" && AUI.Outerwear == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
        if (avar == "Pants" && AUI.Pants == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
        if (avar == "Shoe" && AUI.Shoe == name)
        {
            AUI.prevCheckButton = rawimage;
            rawimage.GetComponent<CustomRawImageScript>().checking.SetActive(true);
        }
    }

    void PositionModel(GameObject model, string avar)
    {
        if (avar == "Shoe")
        {
            model.transform.position = renderCamera.transform.position + renderCamera.transform.forward * 1f;
            model.transform.LookAt(renderCamera.transform);
        }
        else if (avar == "Hair" || avar == "Hat"){
            model.transform.position = renderCamera.transform.position + renderCamera.transform.forward * 1f - Vector3.up * 2f;
            model.transform.LookAt(renderCamera.transform);
            model.transform.Rotate(45, 0, 0);
        }
        else if (avar == "Mustache" || avar == "Glasses" || avar == "Eyebrow" || avar == "Body")
        {
            model.transform.position = renderCamera.transform.position + renderCamera.transform.forward * 1f - Vector3.up * 1f;
            model.transform.LookAt(renderCamera.transform);
            model.transform.localScale = model.transform.localScale * 2;
            model.transform.localPosition = model.transform.localPosition - Vector3.up * 1.7f - Vector3.forward*2;
        }
 
        else
        {
            model.transform.position = renderCamera.transform.position + renderCamera.transform.forward * 1f - Vector3.up * 1f;
            model.transform.LookAt(renderCamera.transform);
        }
       
    }

   
}
