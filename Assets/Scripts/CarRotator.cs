using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CarRotator : MonoBehaviour
{
    public ARPlacementInteractable mclarenPlacer;
    public ARPlacementInteractable dodgePlacer;
    public Button rotateToggleButton;
    public float rotationSpeed = 30f;
    public GameObject currentCar;
    public bool isRotating = false;

    private void OnEnable()
    {
        mclarenPlacer.objectPlaced.AddListener(OnCarPlaced);
        dodgePlacer.objectPlaced.AddListener(OnCarPlaced);
        rotateToggleButton.onClick.AddListener(ToggleRotation);
    }

    private void OnDisable()
    {
        mclarenPlacer.objectPlaced.RemoveListener(OnCarPlaced);
        dodgePlacer.objectPlaced.RemoveListener(OnCarPlaced);
        rotateToggleButton.onClick.RemoveListener(ToggleRotation);
    }

    private void OnCarPlaced(ARObjectPlacementEventArgs args)
    {
        currentCar = args.placementObject;
        isRotating = false;
    }

    private void ToggleRotation()
    {
        isRotating = !isRotating;
    }

    private void Update()
    {
        if (isRotating && currentCar != null)
        {
            currentCar.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
        }
    }
}
