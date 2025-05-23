using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button backButton;

    [Header("Scene")]
    [Tooltip("Exact name of your Start Menu scene as in Build Settings")]
    [SerializeField] private string sceneName = "Start Menu";

    private void OnEnable()
    {
        if (backButton != null)
            backButton.onClick.AddListener(LoadStartMenu);
    }

    private void OnDisable()
    {
        if (backButton != null)
            backButton.onClick.RemoveListener(LoadStartMenu);
    }

    private void LoadStartMenu()
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("BackToStartMenu: sceneName is empty.");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
}
