using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackMenu : MonoBehaviour
{
    public Button backButton;
    public string sceneName = "Start Menu";

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
        SceneManager.LoadScene(sceneName);
    }
}
