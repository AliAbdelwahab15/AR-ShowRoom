using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class ColorEvent : UnityEvent<Color> { }

public class ColorWheel : MonoBehaviour
{
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Image previewImage;
    public ColorEvent onColorChanged;

    private void Start()
    {
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
