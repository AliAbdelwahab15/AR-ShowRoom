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
        // subscribe to the marker spawn event
        MarkerSpawner.OnMarkerPlaced += OnCarPlaced;
        rotateToggleButton.onClick.AddListener(ToggleRotation);
    }

    void OnDisable()
    {
        MarkerSpawner.OnMarkerPlaced -= OnCarPlaced;
        rotateToggleButton.onClick.RemoveListener(ToggleRotation);
    }

    // called by MarkerSpawner when a car prefab is instantiated
    private void OnCarPlaced(GameObject car)
    {
        currentCar = car;
        isRotating = false;
    }

    // toggle rotation on/off
    private void ToggleRotation()
    {
        if (currentCar == null)
        {
            Debug.LogWarning("No car has been spawned yet.");
            return;
        }
        isRotating = !isRotating;
    }

    void Update()
    {
        if (isRotating && currentCar != null)
        {
            // rotate around world Y axis
            currentCar.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
        }
    }
}
