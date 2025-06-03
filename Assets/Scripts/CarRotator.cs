using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CarRotator : MonoBehaviour
{
    [SerializeField] private ARPlacementInteractable mclarenPlacer;
    [SerializeField] private ARPlacementInteractable dodgePlacer;
    [SerializeField] private Button rotateToggleButton;
    [SerializeField] private float rotationSpeed = 30f;

    private GameObject currentCar;
    private bool isRotating = false;

    private void OnEnable()
    {
        // Bind the placement event directly to our handler with ARObjectPlacementEventArgs
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

    // Called whenever either placer spawns its prefab
    private void OnCarPlaced(ARObjectPlacementEventArgs args)
    {
        currentCar = args.placementObject;
        isRotating = false; // reset rotation state on new spawn
    }

    // Toggles rotation of the currentCar
    private void ToggleRotation()
    {
        if (currentCar == null)
        {
            Debug.LogWarning("No car has been spawned yet.");
            return;
        }

        isRotating = !isRotating;
    }

    private void Update()
    {
        if (isRotating && currentCar != null)
        {
            // Rotate around world Y axis
            currentCar.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
        }
    }
}
