using UnityEngine;
using UnityEngine.UI;

public class MultiModelToImage : MonoBehaviour
{
    public Camera renderCamera; // 모델을 렌더링할 카메라
    //public RawImage[] displayImages; // 여러 모델을 표시할 UI 요소
    public GameObject[] models; // 렌더링할 3D 모델들
    public GameObject CustomRawImage;
    public Transform CustomContent;

    private RenderTexture[] renderTextures;

    void Start()
    {
        AvarUIAllReset();
        SetAvarUI("Backpack");
    }


    public void ChangeAvarBackpacks()
    {
        AvarUIAllReset();
        SetAvarUI("Backpack");
    }

    public void ChangeAvarColorBody()
    {
        AvarUIAllReset();
        SetAvarUI("Body");
    }
    public void ChangeAvarColorEyebrow()
    {
        AvarUIAllReset();
        SetAvarUI("Eyebrow");
    }
    public void ChangeAvarColorGlasses()
    {
        AvarUIAllReset();
        SetAvarUI("Glasses");
    }
    public void ChangeAvarColorGlove()
    {
        AvarUIAllReset();
        SetAvarUI("Glove");
    }
    public void ChangeAvarColorHair()
    {
        AvarUIAllReset();
        SetAvarUI("Hair");
    }
    public void ChangeAvarColorHat()
    {
        AvarUIAllReset();
        SetAvarUI("Hat");
    }
    public void ChangeAvarColorMustache()
    {
        AvarUIAllReset();
        SetAvarUI("Mustache");
    }
    public void ChangeAvarColorOutwear()
    {
        AvarUIAllReset();
        SetAvarUI("Outerwear");
    }
    public void ChangeAvarColorPants()
    {
        AvarUIAllReset();
        SetAvarUI("Pants");
    }
    public void ChangeAvarColorShoe()
    {
        AvarUIAllReset();
        SetAvarUI("Shoe");
    }

    private void AvarUIAllReset()
    {
        for (int i =0; i < CustomContent.childCount;i++)
        {
            Destroy(CustomContent.GetChild(i).gameObject);
        }
    }

    private void SetAvarUI(string avar)
    {
        models = Resources.LoadAll<GameObject>("Prefabs/" + avar);
        renderTextures = new RenderTexture[models.Length];
        GameObject crii = Instantiate(CustomRawImage, CustomContent);
        crii.GetComponent<CustomRawImageScript>().SetItemPath("", avar);

        for (int i = 0; i < models.Length; i++)
        {
            models[i] = Instantiate(models[i]);
            GameObject cri = Instantiate(CustomRawImage, CustomContent);
            RawImage ri = cri.GetComponent<RawImage>();
            cri.GetComponent<CustomRawImageScript>().SetItemPath($"Meshes/{avar}/{i+1}",avar);
            models[i].transform.localScale = models[i].transform.localScale * 120;

            // 각 모델을 위한 Render Texture 생성
            renderTextures[i] = new RenderTexture(256, 256, 16);
            renderCamera.targetTexture = renderTextures[i];

            // 모델 위치 조정 및 렌더링
            PositionModel(models[i], avar);
            renderCamera.Render();

            // UI에 해당 Render Texture 할당
            //displayImage.texture = renderTextures[i];
            ri.texture = renderTextures[i];
            
            models[i].SetActive(false);
            Destroy(models[i]);

            if (i == models.Length - 1)
            {
                renderCamera.targetTexture = new RenderTexture(256, 256, 16);
            }
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
