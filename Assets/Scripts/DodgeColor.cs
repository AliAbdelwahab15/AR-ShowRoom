using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class DodgeColor : MonoBehaviour
{
    [Header("AR Placement Interactable")]
    [SerializeField] private ARPlacementInteractable placementInteractable;

    [Header("UI Buttons")]
    [SerializeField] private Button redButton;
    [SerializeField] private Button yellowButton;
    [SerializeField] private Button blueButton;

    [Header("Original Paint Material (Body1)")]
    [SerializeField] private Material originalPaintMaterial;

    // The color chosen for the *next* spawn
    private Color nextColor;
    private bool hasNextColor = false;

    private void OnEnable()
    {
        // Listen for spawn
        placementInteractable.objectPlaced.AddListener(OnCarPlaced);

        // Pick your next spawn color
        redButton.onClick.AddListener(() => ArmNext(Color.red));
        yellowButton.onClick.AddListener(() => ArmNext(Color.yellow));
        blueButton.onClick.AddListener(() => ArmNext(Color.blue));
    }

    private void OnDisable()
    {
        placementInteractable.objectPlaced.RemoveListener(OnCarPlaced);
        redButton.onClick.RemoveAllListeners();
        yellowButton.onClick.RemoveAllListeners();
        blueButton.onClick.RemoveAllListeners();
    }

    private void ArmNext(Color c)
    {
        nextColor = c;
        hasNextColor = true;
        Debug.Log("Next spawn will be tinted " + c);
    }

    private void OnCarPlaced(ARObjectPlacementEventArgs args)
    {
        if (!hasNextColor)
            return;

        hasNextColor = false;
        var carInstance = args.placementObject;
        ApplyColor(carInstance, nextColor);
        Debug.Log("Spawned car tinted " + nextColor);
    }

    private void ApplyColor(GameObject root, Color tint)
    {
        // Go through each Renderer in the new car
        foreach (var rend in root.GetComponentsInChildren<Renderer>())
        {
            var shared = rend.sharedMaterials;  // the original prefab materials
            var mats = rend.materials;        // the runtime copies we can modify

            for (int i = 0; i < shared.Length; i++)
            {
                // only recolor the paint slot
                if (shared[i] == originalPaintMaterial)
                {
                    // URP uses "_BaseColor"; built-in uses "_Color"
                    if (mats[i].HasProperty("_BaseColor"))
                        mats[i].SetColor("_BaseColor", tint);
                    else if (mats[i].HasProperty("_Color"))
                        mats[i].SetColor("_Color", tint);
                }
            }

            rend.materials = mats;
        }
    }
}
