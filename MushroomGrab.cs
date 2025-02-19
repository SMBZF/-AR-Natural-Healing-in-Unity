using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MushroomGrab : MonoBehaviour
{
    public AudioClip grabSound; // ץȡ��Ч
    private AudioSource audioSource; // ��Ч������
    private bool hasPlayedSound = false; // �����Ч�Ƿ��Ѳ���

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // ����ץȡ�¼�
    public void OnGrabbed(SelectEnterEventArgs args)
    {
        if (!hasPlayedSound)
        {
            PlayGrabSound();
            hasPlayedSound = true;
        }
    }

    private void PlayGrabSound()
    {
        if (grabSound != null)
        {
            audioSource.PlayOneShot(grabSound);
        }
    }
}
