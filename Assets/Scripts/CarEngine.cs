using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CarEngine : MonoBehaviour
{
    public enum CarType { None, McLaren, Dodge }
    public ARPlacementInteractable mclarenPlacer;
    public ARPlacementInteractable dodgePlacer;
    public AudioSource mclarenAudioSource;
    public AudioSource dodgeAudioSource;
    public Button engineToggleButton;
    public CarType currentType = CarType.None;

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
                dodgeAudioSource.Stop();
                if (mclarenAudioSource.isPlaying)
                    mclarenAudioSource.Stop();
                else
                    mclarenAudioSource.Play();
                break;

            case CarType.Dodge:
                mclarenAudioSource.Stop();
                if (dodgeAudioSource.isPlaying)
                    dodgeAudioSource.Stop();
                else
                    dodgeAudioSource.Play();
                break;
        }
    }
}
