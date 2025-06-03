using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class DodgeColor : MonoBehaviour
{
    public ARPlacementInteractable placementInteractable;

    [Header("UI Buttons")]
    public Button redButton;
    public Button yellowButton;
    public Button blueButton;
    public Material originalPaintMaterial;
    public Color nextColor;
    public bool hasNextColor = false;

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
