using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class ColorEvent : UnityEvent<Color> { }

public class ColorWheel : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Image previewImage;

    [Header("Event")]
    public ColorEvent onColorChanged;

    private void Start()
    {
        // hook all three sliders to our callback
        redSlider.onValueChanged.AddListener(_ => UpdateColor());
        greenSlider.onValueChanged.AddListener(_ => UpdateColor());
        blueSlider.onValueChanged.AddListener(_ => UpdateColor());
        UpdateColor();
    }

    private void UpdateColor()
    {
        var c = new Color(
            redSlider.value,
            greenSlider.value,
            blueSlider.value
        );
        if (previewImage != null)
            previewImage.color = c;

        onColorChanged.Invoke(c);
    }
}
