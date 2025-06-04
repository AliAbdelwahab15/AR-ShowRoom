using UnityEngine;
using UnityEngine.UI;

public class MarkerCarRotator : MonoBehaviour
{
    public GameObject currentCar;
    public bool isRotating = false;
    public Button rotateToggleButton;
    public float rotationSpeed = 30.0f;

    void OnEnable()
    {
        MarkerSpawner.OnMarkerPlaced += OnCarPlaced;
        rotateToggleButton.onClick.AddListener(ToggleRotation);
    }

    void OnDisable()
    {
        MarkerSpawner.OnMarkerPlaced -= OnCarPlaced;
        rotateToggleButton.onClick.RemoveListener(ToggleRotation);
    }

    private void OnCarPlaced(GameObject car)
    {
        currentCar = car;
        isRotating = false;
    }

    private void ToggleRotation()
    {
        isRotating = !isRotating;
    }

    void Update()
    {
        if (isRotating && currentCar != null)
        {
            currentCar.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
        }
    }
}
