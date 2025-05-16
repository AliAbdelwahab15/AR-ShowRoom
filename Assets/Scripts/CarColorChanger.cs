using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CarColorChanger : MonoBehaviour
{
    private enum CarType { None, McLaren, Dodge }
    private enum ColorChoice { Red, Yellow, Blue, Black }

    [Header("AR Placement Interactables")]
    [SerializeField] private ARPlacementInteractable mclarenPlacer;
    [SerializeField] private ARPlacementInteractable dodgePlacer;

    [Header("UI Buttons")]
    [SerializeField] private Button redButton;
    [SerializeField] private Button yellowButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button blackButton;  // added

    [Header("McLaren Materials")]
    [SerializeField] private Material mclarenOriginalMaterial;
    [SerializeField] private Material mclarenRedMaterial;
    [SerializeField] private Material mclarenYellowMaterial;
    [SerializeField] private Material mclarenBlueMaterial;
    [SerializeField] private Material mclarenBlackMaterial; // added

    [Header("Dodge Material")]
    [SerializeField] private Material dodgeOriginalMaterial;

    // state
    private GameObject currentCar;
    private CarType currentType = CarType.None;
    private Dictionary<Renderer, int[]> paintSlots = new Dictionary<Renderer, int[]>();

    // next-spawn color
    private bool nextColorArmed = false;
    private ColorChoice nextChoice = ColorChoice.Red;

    private void OnEnable()
    {
        mclarenPlacer.objectPlaced.AddListener(args => OnCarPlaced(args, CarType.McLaren));
        dodgePlacer.objectPlaced.AddListener(args => OnCarPlaced(args, CarType.Dodge));

        redButton.onClick.AddListener(() => OnColorButton(ColorChoice.Red));
        yellowButton.onClick.AddListener(() => OnColorButton(ColorChoice.Yellow));
        blueButton.onClick.AddListener(() => OnColorButton(ColorChoice.Blue));
        blackButton.onClick.AddListener(() => OnColorButton(ColorChoice.Black)); // added
    }

    private void OnDisable()
    {
        mclarenPlacer.objectPlaced.RemoveAllListeners();
        dodgePlacer.objectPlaced.RemoveAllListeners();

        redButton.onClick.RemoveAllListeners();
        yellowButton.onClick.RemoveAllListeners();
        blueButton.onClick.RemoveAllListeners();
        blackButton.onClick.RemoveAllListeners(); // added
    }

    private void OnColorButton(ColorChoice choice)
    {
        // Arm next spawn and repaint current if any
        nextChoice = choice;
        nextColorArmed = true;
        if (currentCar != null)
            ApplyChoiceToCar(currentCar, currentType, choice);
    }

    private void OnCarPlaced(ARObjectPlacementEventArgs args, CarType type)
    {
        currentCar = args.placementObject;
        currentType = type;
        paintSlots.Clear();

        Material original = (type == CarType.McLaren)
            ? mclarenOriginalMaterial
            : dodgeOriginalMaterial;

        // Cache which material slots are paint
        foreach (Renderer rend in currentCar.GetComponentsInChildren<Renderer>())
        {
            Material[] shared = rend.sharedMaterials;
            List<int> slots = new List<int>();
            for (int i = 0; i < shared.Length; i++)
            {
                if (shared[i] == original)
                    slots.Add(i);
            }
            if (slots.Count > 0)
                paintSlots[rend] = slots.ToArray();
        }

        // If a color was armed, apply it immediately
        if (nextColorArmed)
        {
            ApplyChoiceToCar(currentCar, currentType, nextChoice);
            nextColorArmed = false;
        }
    }

    private void ApplyChoiceToCar(GameObject car, CarType type, ColorChoice choice)
    {
        Material replaceMat = null;
        Color tint = Color.white;

        if (type == CarType.McLaren)
        {
            switch (choice)
            {
                case ColorChoice.Red: replaceMat = mclarenRedMaterial; break;
                case ColorChoice.Yellow: replaceMat = mclarenYellowMaterial; break;
                case ColorChoice.Blue: replaceMat = mclarenBlueMaterial; break;
                case ColorChoice.Black: replaceMat = mclarenBlackMaterial; break; // added
            }
        }
        else // Dodge
        {
            switch (choice)
            {
                case ColorChoice.Red: tint = Color.red; break;
                case ColorChoice.Yellow: tint = Color.yellow; break;
                case ColorChoice.Blue: tint = Color.blue; break;
                case ColorChoice.Black: tint = Color.black; break; // added
            }
        }

        // Apply to all paint slots
        foreach (var kv in paintSlots)
        {
            Renderer rend = kv.Key;
            int[] slots = kv.Value;
            Material[] mats = rend.materials;

            for (int i = 0; i < slots.Length; i++)
            {
                int idx = slots[i];
                if (type == CarType.McLaren)
                    mats[idx] = replaceMat;
                else // Dodge
                {
                    if (mats[idx].HasProperty("_BaseColor"))
                        mats[idx].SetColor("_BaseColor", tint);
                    else if (mats[idx].HasProperty("_Color"))
                        mats[idx].SetColor("_Color", tint);
                }
            }
            rend.materials = mats;
        }
    }
}
