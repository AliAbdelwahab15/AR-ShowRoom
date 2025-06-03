using UnityEngine;
using UnityEngine.UI;

public class SpeakerAudioPlayer : MonoBehaviour
{
    public Button button;
    public AudioSource audioSource;

    private void OnEnable()
    {
        if (button != null)
            button.onClick.AddListener(PlayAudio);
    }

    private void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveListener(PlayAudio);
    }

    private void PlayAudio()
    {
        if (audioSource != null)
            audioSource.Play();
    }
}
