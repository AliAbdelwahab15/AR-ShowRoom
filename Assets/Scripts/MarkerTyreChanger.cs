using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerTyreChanger : MonoBehaviour
{
    public enum CarType { None, McLaren, Dodge }
    public enum ColorChoice { Red, Yellow, Blue, Black }
    public Button redButton;
    public Button yellowButton;
    public Button blueButton;
    public Button blackButton;
    public Material mclarenOriginalTyreMaterial;
    public Material mclarenRedTyreMaterial;
    public Material mclarenYellowTyreMaterial;
    public Material mclarenBlueTyreMaterial;
    public Material mclarenBlackTyreMaterial;
    public Material dodgeOriginalTyreMaterial;
    public GameObject currentCar;
    private CarType currentType = CarType.None;
    public Material currentOriginalTyres;
    public Dictionary<Renderer, int[]> tyreSlots = new Dictionary<Renderer, int[]>();
    public bool nextTyreColorArmed = false;
    private ColorChoice nextTyreChoice = ColorChoice.Red;

    private void OnEnable()
    {
        MarkerSpawner.OnMarkerPlaced += OnMarkerPlaced;

        redButton.onClick.AddListener(() => OnTyreButton(ColorChoice.Red));
        yellowButton.onClick.AddListener(() => OnTyreButton(ColorChoice.Yellow));
        blueButton.onClick.AddListener(() => OnTyreButton(ColorChoice.Blue));
        blackButton.onClick.AddListener(() => OnTyreButton(ColorChoice.Black));
    }

    private void OnDisable()
    {
        MarkerSpawner.OnMarkerPlaced -= OnMarkerPlaced;

        redButton.onClick.RemoveAllListeners();
        yellowButton.onClick.RemoveAllListeners();
        blueButton.onClick.RemoveAllListeners();
        blackButton.onClick.RemoveAllListeners();
    }

    private void OnTyreButton(ColorChoice choice)
    {
        nextTyreChoice = choice;
        nextTyreColorArmed = true;

        if (currentCar != null)
            ApplyTyreChoice(choice);
    }

    private void OnMarkerPlaced(GameObject car)
    {
        currentCar = car;

        // determine type by name
        if (car.name.Contains("McLaren")) currentType = CarType.McLaren;
        else if (car.name.Contains("Dodge")) currentType = CarType.Dodge;
        else currentType = CarType.None;

        currentOriginalTyres = (currentType == CarType.McLaren)
            ? mclarenOriginalTyreMaterial
            : dodgeOriginalTyreMaterial;

        // cache tyre slots
        tyreSlots.Clear();
        foreach (var rend in currentCar.GetComponentsInChildren<Renderer>())
        {
            var shared = rend.sharedMaterials;
            var list = new List<int>();

            for (int i = 0; i < shared.Length; i++)
                if (shared[i] == currentOriginalTyres)
                    list.Add(i);

            if (list.Count > 0)
                tyreSlots[rend] = list.ToArray();
        }

        // apply pre-selected colour if any
        if (nextTyreColorArmed)
        {
            ApplyTyreChoice(nextTyreChoice);
            nextTyreColorArmed = false;
        }
    }

    private void ApplyTyreChoice(ColorChoice choice)
    {
        if (currentCar == null) return;

        Material replaceMat = null;
        Color tintColor = Color.black;

        if (currentType == CarType.McLaren)
        {
            switch (choice)
            {
                case ColorChoice.Red: replaceMat = mclarenRedTyreMaterial; break;
                case ColorChoice.Yellow: replaceMat = mclarenYellowTyreMaterial; break;
                case ColorChoice.Blue: replaceMat = mclarenBlueTyreMaterial; break;
                case ColorChoice.Black: replaceMat = mclarenBlackTyreMaterial; break;
            }
        }
        else // Dodge
        {
            switch (choice)
            {
                case ColorChoice.Red: tintColor = Color.red; break;
                case ColorChoice.Yellow: tintColor = Color.yellow; break;
                case ColorChoice.Blue: tintColor = Color.blue; break;
                case ColorChoice.Black: tintColor = Color.black; break;
            }
        }

        foreach (var kv in tyreSlots)
        {
            var rend = kv.Key;
            var slots = kv.Value;
            var mats = rend.materials;

            foreach (int idx in slots)
            {
                if (currentType == CarType.McLaren)
                    mats[idx] = replaceMat;
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
