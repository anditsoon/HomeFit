using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiModelToImage : MonoBehaviour
{
    public Camera renderCamera; // 모델을 렌더링할 카메라
    public RawImage[] displayImages; // 여러 모델을 표시할 UI 요소
    public GameObject[] models; // 렌더링할 3D 모델들

    private RenderTexture[] renderTextures;

    void Start()
    {
        models = Resources.LoadAll<GameObject>("Prefabs/Glove");
        renderTextures = new RenderTexture[models.Length];

        for (int i = 0; i < models.Length; i++)
        {
            models[i] = Instantiate(models[i]);
            models[i].transform.localScale = models[i].transform.localScale * 120;

            // 각 모델을 위한 Render Texture 생성
            renderTextures[i] = new RenderTexture(256, 256, 16);
            renderCamera.targetTexture = renderTextures[i];

            // 모델 위치 조정 및 렌더링
            PositionModel(models[i]);
            renderCamera.Render();

            // UI에 해당 Render Texture 할당
            displayImages[i].texture = renderTextures[i];
            //if (i == models.Length-1)
            //{
            //    break;
            //}
            models[i].SetActive(false);
        }
      
    }

    void PositionModel(GameObject model)
    {
        model.transform.position = renderCamera.transform.position + renderCamera.transform.forward * 1f - Vector3.up;
        model.transform.LookAt(renderCamera.transform);
    }
}
