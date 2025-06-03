using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    public Button markerBasedButton;
    public Button markerlessButton;
    public string markerBasedSceneName;
    public string markerlessSceneName;

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
        SceneManager.LoadScene(markerBasedSceneName);
    }

    private void LoadMarkerlessScene()
    {
        SceneManager.LoadScene(markerlessSceneName);
    }
}
