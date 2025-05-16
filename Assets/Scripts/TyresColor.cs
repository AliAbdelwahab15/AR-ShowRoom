using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class TyresColor : MonoBehaviour
{
    private enum CarType { None, McLaren, Dodge }
    private enum ColorChoice { Red, Blue, Yellow, Black }

    [Header("AR Placers")]
    [SerializeField] private ARPlacementInteractable mclarenPlacer;
    [SerializeField] private ARPlacementInteractable dodgePlacer;

    [Header("McLaren Tyre Materials")]
    [SerializeField] private Material mclarenOriginalTyreMaterial;
    [SerializeField] private Material mclarenRedTyreMaterial;
    [SerializeField] private Material mclarenBlueTyreMaterial;
    [SerializeField] private Material mclarenYellowTyreMaterial;
    [SerializeField] private Material mclarenBlackTyreMaterial;

    [Header("Dodge Tyre Material")]
    [SerializeField] private Material dodgeOriginalTyreMaterial;

    [Header("UI Buttons")]
    [SerializeField] private Button redButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button yellowButton;
    [SerializeField] private Button blackButton;

    // runtime state
    private GameObject currentCar;
    private CarType currentType = CarType.None;
    private Material currentOriginalTyreMaterial;
    private Dictionary<Renderer, int[]> tyreSlots = new Dictionary<Renderer, int[]>();

    // next-spawn color
    private bool nextColorArmed = false;
    private ColorChoice nextChoice = ColorChoice.Red;

    private void OnEnable()
    {
        mclarenPlacer.objectPlaced.AddListener(args => OnCarPlaced(args, CarType.McLaren));
        dodgePlacer.objectPlaced.AddListener(args => OnCarPlaced(args, CarType.Dodge));

        redButton.onClick.AddListener(() => OnColorButton(ColorChoice.Red));
        blueButton.onClick.AddListener(() => OnColorButton(ColorChoice.Blue));
        yellowButton.onClick.AddListener(() => OnColorButton(ColorChoice.Yellow));
        blackButton.onClick.AddListener(() => OnColorButton(ColorChoice.Black));
    }

    private void OnDisable()
    {
        mclarenPlacer.objectPlaced.RemoveAllListeners();
        dodgePlacer.objectPlaced.RemoveAllListeners();

        redButton.onClick.RemoveAllListeners();
        blueButton.onClick.RemoveAllListeners();
        yellowButton.onClick.RemoveAllListeners();
        blackButton.onClick.RemoveAllListeners();
    }

    private void OnColorButton(ColorChoice choice)
    {
        // arm next spawn
        nextChoice = choice;
        nextColorArmed = true;

        // recolor current car immediately if any
        if (currentCar != null)
            ApplyTyreColor(choice);
    }

    private void OnCarPlaced(ARObjectPlacementEventArgs args, CarType type)
    {
        currentCar = args.placementObject;
        currentType = type;
        currentOriginalTyreMaterial = (type == CarType.McLaren)
            ? mclarenOriginalTyreMaterial
            : dodgeOriginalTyreMaterial;

        CacheTyreSlots();

        // if a color was armed, apply it on spawn
        if (nextColorArmed)
        {
            ApplyTyreColor(nextChoice);
            nextColorArmed = false;
        }
    }

    private void CacheTyreSlots()
    {
        tyreSlots.Clear();
        if (currentCar == null || currentOriginalTyreMaterial == null)
            return;

        foreach (var rend in currentCar.GetComponentsInChildren<Renderer>())
        {
            var shared = rend.sharedMaterials;
            var slots = new List<int>();
            for (int i = 0; i < shared.Length; i++)
            {
                if (shared[i] == currentOriginalTyreMaterial)
                    slots.Add(i);
            }
            if (slots.Count > 0)
                tyreSlots[rend] = slots.ToArray();
        }
    }

    private void ApplyTyreColor(ColorChoice choice)
    {
        if (currentCar == null || tyreSlots.Count == 0)
            return;

        Material replacementMat = null;
        Color tintColor = Color.white;

        if (currentType == CarType.McLaren)
        {
            switch (choice)
            {
                case ColorChoice.Red: replacementMat = mclarenRedTyreMaterial; break;
                case ColorChoice.Blue: replacementMat = mclarenBlueTyreMaterial; break;
                case ColorChoice.Yellow: replacementMat = mclarenYellowTyreMaterial; break;
                case ColorChoice.Black: replacementMat = mclarenBlackTyreMaterial; break;
            }
        }
        else // Dodge
        {
            switch (choice)
            {
                case ColorChoice.Red: tintColor = Color.red; break;
                case ColorChoice.Blue: tintColor = Color.blue; break;
                case ColorChoice.Yellow: tintColor = Color.yellow; break;
                case ColorChoice.Black: tintColor = Color.black; break;
            }
        }

        foreach (var kv in tyreSlots)
        {
            var rend = kv.Key;
            var mats = rend.materials;
            foreach (var idx in kv.Value)
            {
                if (currentType == CarType.McLaren && replacementMat != null)
                    mats[idx] = replacementMat;
                else
                {
                    if (mats[idx].HasProperty("_BaseColor"))
                        mats[idx].SetColor("_BaseColor", tintColor);
                    else if (mats[idx].HasProperty("_Color"))
                        mats[idx].SetColor("_Color", tintColor);
                }
            }
            rend.materials = mats;
        }
    }
}
