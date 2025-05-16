using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CarEngine : MonoBehaviour
{
    private enum CarType { None, McLaren, Dodge }

    [Header("AR Placement Interactables")]
    [SerializeField] private ARPlacementInteractable mclarenPlacer;
    [SerializeField] private ARPlacementInteractable dodgePlacer;

    [Header("Engine AudioSources")]
    [Tooltip("AudioSource configured with the McLaren start sound")]
    [SerializeField] private AudioSource mclarenAudioSource;
    [Tooltip("AudioSource configured with the Dodge start sound")]
    [SerializeField] private AudioSource dodgeAudioSource;

    [Header("UI Toggle Button")]
    [SerializeField] private Button engineToggleButton;

    private CarType currentType = CarType.None;

    private void OnEnable()
    {
        mclarenPlacer.objectPlaced.AddListener(_ => currentType = CarType.McLaren);
        dodgePlacer.objectPlaced.AddListener(_ => currentType = CarType.Dodge);
        engineToggleButton.onClick.AddListener(ToggleEngineSound);
    }

    private void OnDisable()
    {
        mclarenPlacer.objectPlaced.RemoveAllListeners();
        dodgePlacer.objectPlaced.RemoveAllListeners();
        engineToggleButton.onClick.RemoveAllListeners();
    }

    private void ToggleEngineSound()
    {
        switch (currentType)
        {
            case CarType.McLaren:
                // Stop the other audio
                dodgeAudioSource.Stop();
                // Toggle McLaren audio
                if (mclarenAudioSource.isPlaying)
                    mclarenAudioSource.Stop();
                else
                    mclarenAudioSource.Play();
                break;

            case CarType.Dodge:
                // Stop the other audio
                mclarenAudioSource.Stop();
                // Toggle Dodge audio
                if (dodgeAudioSource.isPlaying)
                    dodgeAudioSource.Stop();
                else
                    dodgeAudioSource.Play();
                break;

            default:
                Debug.LogWarning("No car has been spawned yet.");
                break;
        }
    }
}
