using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class McLarenColor : MonoBehaviour
{
    [Header("AR Placement")]
    [SerializeField] private ARPlacementInteractable placementInteractable;

    [Header("UI")]
    [SerializeField] private Button changeNextButton;

    [Header("Materials")]
    [Tooltip("Drag in the original 'Body1' material asset here")]
    [SerializeField] private Material originalMaterial;

    [Tooltip("Drag in the replacement 'Body 1 2' material asset here")]
    [SerializeField] private Material replacementMaterial;

    // when true, the next spawned prefab will have its body swapped
    private bool swapNext = false;

    private void OnEnable()
    {
        placementInteractable.objectPlaced.AddListener(OnObjectPlaced);
        changeNextButton.onClick.AddListener(() => swapNext = true);
    }

    private void OnDisable()
    {
        placementInteractable.objectPlaced.RemoveListener(OnObjectPlaced);
        changeNextButton.onClick.RemoveAllListeners();
    }

    private void OnObjectPlaced(ARObjectPlacementEventArgs args)
    {
        if (!swapNext)
            return;

        swapNext = false;
        GameObject spawned = args.placementObject;
        int count = 0;

        // For each Renderer in the spawned prefab:
        foreach (Renderer rend in spawned.GetComponentsInChildren<Renderer>())
        {
            // sharedMaterials holds the actual assets from the prefab
            Material[] shared = rend.sharedMaterials;
            // materials is the runtime instance array we can modify
            Material[] mats = rend.materials;

            for (int i = 0; i < shared.Length; i++)
            {
                // if this slot was using your originalMaterial asset, swap it
                if (shared[i] == originalMaterial)
                {
                    mats[i] = replacementMaterial;
                    count++;
                }
            }

            // apply back
            rend.materials = mats;
        }

        Debug.Log("Swapped " + count + " Body1 slot(s) to " + replacementMaterial.name + ".");
    }
}
