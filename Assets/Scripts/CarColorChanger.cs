using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CarColorChanger : MonoBehaviour
{
    private enum Mode { Paint, Tyres }

    public ARPlacementInteractable mclarenPlacer;
    public ARPlacementInteractable dodgePlacer;
    public Material mclarenOriginalPaint;
    public Material dodgeOriginalPaint;
    public Material mclarenOriginalTyre;
    public Material dodgeOriginalTyre;
    public Button paintModeButton;
    public Button tyreModeButton;
    public ColorWheel colorWheel; // your existing wheel

    // state
    private Mode currentMode = Mode.Paint;
    public GameObject currentCar;
    public Material paintOriginal, tyreOriginal;
    public Dictionary<Renderer, int[]> paintSlots = new Dictionary<Renderer, int[]>();
    public Dictionary<Renderer, int[]> tyreSlots = new Dictionary<Renderer, int[]>();

    private void OnEnable()
    {
        mclarenPlacer.objectPlaced.AddListener(args => CacheNewCar(args.placementObject, true));
        dodgePlacer.objectPlaced.AddListener(args => CacheNewCar(args.placementObject, false));

        paintModeButton.onClick.AddListener(() => currentMode = Mode.Paint);
        tyreModeButton.onClick.AddListener(() => currentMode = Mode.Tyres);

        colorWheel.onColorChanged.AddListener(ApplyColor);
    }

    private void OnDisable()
    {
        mclarenPlacer.objectPlaced.RemoveAllListeners();
        dodgePlacer.objectPlaced.RemoveAllListeners();

        paintModeButton.onClick.RemoveAllListeners();
        tyreModeButton.onClick.RemoveAllListeners();

        colorWheel.onColorChanged.RemoveAllListeners();
    }

    private void CacheNewCar(GameObject car, bool isMcLaren)
    {
        currentCar = car;

        // pick originals
        paintOriginal = isMcLaren ? mclarenOriginalPaint : dodgeOriginalPaint;
        tyreOriginal = isMcLaren ? mclarenOriginalTyre : dodgeOriginalTyre;

        paintSlots.Clear();
        tyreSlots.Clear();

        if (paintOriginal != null)
            CacheSlots(car, paintOriginal, paintSlots);

        if (tyreOriginal != null)
            CacheSlots(car, tyreOriginal, tyreSlots);
    }

    private void CacheSlots(GameObject car, Material original, Dictionary<Renderer, int[]> slotsDict)
    {
        foreach (var rend in car.GetComponentsInChildren<Renderer>())
        {
            var shared = rend.sharedMaterials;
            var list = new List<int>();
            for (int i = 0; i < shared.Length; i++)
                if (shared[i] == original)
                    list.Add(i);

            if (list.Count > 0)
                slotsDict[rend] = list.ToArray();
        }
    }

    private void ApplyColor(Color c)
    {
        if (currentCar == null) return;

        var target = currentMode == Mode.Paint ? paintSlots : tyreSlots;

        foreach (var kv in target)
        {
            var rend = kv.Key;
            var mats = rend.materials; // get modifiable copy

            foreach (int idx in kv.Value)
            {
                if (mats[idx].HasProperty("_BaseColor"))
                    mats[idx].SetColor("_BaseColor", c);
                else if (mats[idx].HasProperty("_Color"))
                    mats[idx].SetColor("_Color", c);
            }

            rend.materials = mats; // write back
        }
    }
}
