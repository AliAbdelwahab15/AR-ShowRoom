using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public Text FPSText;
    public float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        if (FPSText == null) return;

        float fps = 1f / deltaTime;
        FPSText.text = $"FPS: {fps:0}";
    }
}
