using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class DodgeInteractiveColor : MonoBehaviour
{
    [Header("AR Placement Interactable")]
    [SerializeField] private ARPlacementInteractable placementInteractable;

    [Header("Color Buttons")]
    [SerializeField] private Button redButton;
    [SerializeField] private Button yellowButton;
    [SerializeField] private Button blueButton;

    [Header("Paint Material Asset")]
    [SerializeField] private Material originalPaintMaterial;

    // the most recently spawned car
    private GameObject currentCar;

    // for each renderer, which material slots are the paint slots
    private Dictionary<Renderer, int[]> paintSlots = new Dictionary<Renderer, int[]>();

    private void OnEnable()
    {
        placementInteractable.objectPlaced.AddListener(OnCarPlaced);
        redButton.onClick.AddListener(() => RecolorCurrentCar(Color.red));
        yellowButton.onClick.AddListener(() => RecolorCurrentCar(Color.yellow));
        blueButton.onClick.AddListener(() => RecolorCurrentCar(Color.blue));
    }

    private void OnDisable()
    {
        placementInteractable.objectPlaced.RemoveListener(OnCarPlaced);
        redButton.onClick.RemoveAllListeners();
        yellowButton.onClick.RemoveAllListeners();
        blueButton.onClick.RemoveAllListeners();
    }

    private void OnCarPlaced(ARObjectPlacementEventArgs args)
    {
        // remember the new car
        currentCar = args.placementObject;

        // rebuild our map of which slots to recolor
        paintSlots.Clear();
        foreach (Renderer rend in currentCar.GetComponentsInChildren<Renderer>())
        {
            Material[] shared = rend.sharedMaterials;
            List<int> indices = new List<int>();
            for (int i = 0; i < shared.Length; i++)
            {
                if (shared[i] == originalPaintMaterial)
                    indices.Add(i);
            }
            if (indices.Count > 0)
                paintSlots[rend] = indices.ToArray();
        }

        Debug.Log("Tracked paint slots on " + paintSlots.Count + " renderers.");
    }

    private void RecolorCurrentCar(Color tint)
    {
        if (currentCar == null)
        {
            Debug.LogWarning("No car has been placed yet.");
            return;
        }

        int totalChanged = 0;
        foreach (var entry in paintSlots)
        {
            Renderer rend = entry.Key;
            int[] slots = entry.Value;
            Material[] mats = rend.materials;
            for (int j = 0; j < slots.Length; j++)
            {
                int idx = slots[j];
                if (mats[idx].HasProperty("_BaseColor"))
                    mats[idx].SetColor("_BaseColor", tint);
                else if (mats[idx].HasProperty("_Color"))
                    mats[idx].SetColor("_Color", tint);
                totalChanged++;
            }
            rend.materials = mats;
        }

        Debug.Log("Recolored " + totalChanged + " slots to color " + tint + ".");
    }
}
