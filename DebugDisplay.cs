using UnityEngine;
using UnityEngine.UI;

public class DebugDisplay : MonoBehaviour
{
    public Text debugText; // ��ʾ������Ϣ�� Text ���

    private static DebugDisplay instance; // ����ģʽ���������ű�����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // ȷ��ֻ��һ��ʵ��
        }
    }

    // ���� Debug ��Ϣ
    public static void UpdateDebug(string message)
    {
        if (instance != null && instance.debugText != null)
        {
            instance.debugText.text = message;
        }
    }
}
