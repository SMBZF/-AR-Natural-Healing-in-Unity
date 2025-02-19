using UnityEngine;

public class FlowerControlWithMPB : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    private Renderer objectRenderer;
    private float currentValue = 0f;
    private float targetValue = 1f;
    private float speed = 0.5f;
    private bool isGrowing = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("Renderer not found!");
            return;
        }

        // Initialize MPB
        mpb = new MaterialPropertyBlock();
        ResetGrow();
        StartGrowing();
    }

    public void ResetGrow()
    {
        isGrowing = false;
        currentValue = 0f;
        UpdateMaterialProperty();
    }

    public void StartGrowing()
    {
        if (isGrowing) return;
        isGrowing = true;
    }

    void Update()
    {
        if (isGrowing)
        {
            currentValue += speed * Time.deltaTime;
            if (currentValue >= targetValue)
            {
                currentValue = targetValue;
                isGrowing = false;
            }

            UpdateMaterialProperty();
            DebugDisplay.UpdateDebug($"Flower Grow: {currentValue:F2}"); // ��ʾ��ǰ Grow ֵ
        }
    }

    private void UpdateMaterialProperty()
    {
        mpb.SetFloat("Grow_", currentValue); // ���� MPB �е� Grow ֵ
        objectRenderer.SetPropertyBlock(mpb); // Ӧ�õ� Renderer
    }
}
