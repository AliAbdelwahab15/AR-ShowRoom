using System;
using UnityEngine;
using UnityEngine.UI;

public class MarkerCarEngine : MonoBehaviour
{
    private enum CarType { None, McLaren, Dodge }

    [Header("Engine AudioSources")]
    [Tooltip("AudioSource with the McLaren start sound")]
    [SerializeField] private AudioSource mclarenAudioSource;
    [Tooltip("AudioSource with the Dodge start sound")]
    [SerializeField] private AudioSource dodgeAudioSource;

    [Header("UI Toggle Button")]
    [SerializeField] private Button engineToggleButton;

    private CarType currentType = CarType.None;

    private void OnEnable()
    {
        // Subscribe when a marker spawns a car
        MarkerSpawner.OnMarkerPlaced += OnMarkerPlaced;
        engineToggleButton.onClick.AddListener(ToggleEngineSound);
    }

    private void OnDisable()
    {
        MarkerSpawner.OnMarkerPlaced -= OnMarkerPlaced;
        engineToggleButton.onClick.RemoveAllListeners();
    }

    private void OnMarkerPlaced(GameObject car)
    {
        string n = car.name;  // e.g. "McLaren(Clone)" or "Dodge(Clone)"
        if (n.Contains("McLaren"))
        {
            currentType = CarType.McLaren;
        }
        else if (n.Contains("Dodge"))
        {
            currentType = CarType.Dodge;
        }
        else
        {
            currentType = CarType.None;
        }
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

            default:
                Debug.LogWarning("No car has been spawned yet.");
                break;
        }
    }
}
