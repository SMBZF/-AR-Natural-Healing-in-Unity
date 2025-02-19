using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerBackButton : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty m_XButtonAction; // �� X ��ť�� InputAction

    [SerializeField]
    private InputActionProperty m_BButtonAction; // �� B ��ť�� InputAction

    void OnEnable()
    {
        // ���� X �� B ��ť�����¼�
        m_XButtonAction.action.performed += OnButtonPressed;
        m_BButtonAction.action.performed += OnButtonPressed;
    }

    void OnDisable()
    {
        // �Ƴ��¼�����
        m_XButtonAction.action.performed -= OnButtonPressed;
        m_BButtonAction.action.performed -= OnButtonPressed;
    }

    void OnButtonPressed(InputAction.CallbackContext context)
    {
        BackToMenu();
    }

    void BackToMenu()
    {
        string menuSceneName = "Scenes/MetaMenu"; // �滻Ϊ���ʵ�ʳ���·��
        if (Application.CanStreamedLevelBeLoaded(menuSceneName))
        {
            Debug.Log($"Returning to scene: {menuSceneName}");
            SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
        }
    }
}
