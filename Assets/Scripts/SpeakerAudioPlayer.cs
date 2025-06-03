using UnityEngine;
using UnityEngine.UI;

public class SpeakerAudioPlayer : MonoBehaviour
{
    public Button button;
    public AudioSource audioSource;

    private void OnEnable()
    {
        if (button != null)
            button.onClick.AddListener(OnButtonPressed);
    }

    private void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveListener(OnButtonPressed);
    }

    private void OnButtonPressed()
    {
        if (audioSource == null) return;

        if (audioSource.isPlaying)
            audioSource.Stop();
        else
            audioSource.Play();
    }
}
