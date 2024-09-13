using UnityEngine;

public class Y_MeasureModelSize : MonoBehaviour
{
    public Vector3 modelSize;

    private void Awake()
    {
        // 모델 크기를 초기화합니다.
        modelSize = Vector3.zero;

        // 이 오브젝트와 자식 오브젝트에서 모든 MeshRenderer와 SkinnedMeshRenderer를 찾습니다.
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        // 바운드 초기화
        Bounds combinedBounds = new Bounds(transform.position, Vector3.zero);

        // MeshRenderer 바운드 결합
        foreach (MeshRenderer renderer in meshRenderers)
        {
            if (renderer != null)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }
        }

        // SkinnedMeshRenderer 바운드 결합
        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            if (renderer != null)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }
        }

        // 결합된 바운드의 크기를 모델 크기로 설정합니다.
        modelSize = combinedBounds.size;

        // 만약 렌더러가 없다면 경고 메시지를 표시합니다.
        if (meshRenderers.Length == 0 && skinnedMeshRenderers.Length == 0)
        {
            Debug.LogWarning("모델에 MeshRenderer 또는 SkinnedMeshRenderer 컴포넌트가 없습니다!");
        }
    }
}
