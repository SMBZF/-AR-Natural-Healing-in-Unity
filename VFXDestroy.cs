using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MushroomGrabEffect : MonoBehaviour
{
    public GameObject targetVFX; // ��Ҫ���ٵ� VFX ����

    // ����ץȡ�¼�
    public void OnGrabbed(SelectEnterEventArgs args)
    {
        if (targetVFX != null)
        {
            Destroy(targetVFX); // ����Ŀ�� VFX ����
        }
    }
}
