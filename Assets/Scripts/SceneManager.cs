using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button markerBasedButton;
    [SerializeField] private Button markerlessButton;

    [Header("Scene Names")]
    [Tooltip("Exact name of your marker-based scene in Build Settings")]
    [SerializeField] private string markerBasedSceneName;
    [Tooltip("Exact name of your marker-less scene in Build Settings")]
    [SerializeField] private string markerlessSceneName;

    private void OnEnable()
    {
        if (markerBasedButton != null)
            markerBasedButton.onClick.AddListener(LoadMarkerBasedScene);
        if (markerlessButton != null)
            markerlessButton.onClick.AddListener(LoadMarkerlessScene);
    }

    private void OnDisable()
    {
        if (markerBasedButton != null)
            markerBasedButton.onClick.RemoveListener(LoadMarkerBasedScene);
        if (markerlessButton != null)
            markerlessButton.onClick.RemoveListener(LoadMarkerlessScene);
    }

    private void LoadMarkerBasedScene()
    {
        if (string.IsNullOrEmpty(markerBasedSceneName))
        {
            Debug.LogWarning("SceneSwitcher: marker-based scene name not set.");
            return;
        }
        SceneManager.LoadScene(markerBasedSceneName);
    }

    private void LoadMarkerlessScene()
    {
        if (string.IsNullOrEmpty(markerlessSceneName))
        {
            Debug.LogWarning("SceneSwitcher: marker-less scene name not set.");
            return;
        }
        SceneManager.LoadScene(markerlessSceneName);
    }
}
