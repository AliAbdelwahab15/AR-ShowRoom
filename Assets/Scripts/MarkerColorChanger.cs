using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerColorChanger : MonoBehaviour
{
    public enum Mode { Paint, Tyres }
    public Material mclarenOriginalPaint;
    public Material dodgeOriginalPaint;
    public Material mclarenOriginalTyre;
    public Material dodgeOriginalTyre;
    public Button paintModeButton;
    public Button tyreModeButton;
    public ColorWheel colorWheel;
    private Mode currentMode = Mode.Paint;
    public GameObject currentCar;
    public Material paintOriginal;
    public Material tyreOriginal;
    public Dictionary<Renderer, int[]> paintSlots = new Dictionary<Renderer, int[]>();
    public Dictionary<Renderer, int[]> tyreSlots = new Dictionary<Renderer, int[]>();

    private void OnEnable()
    {
        // Subscribe to marker-based spawns
        MarkerSpawner.OnMarkerPlaced += OnMarkerPlaced;

        // Mode buttons
        paintModeButton.onClick.AddListener(() => currentMode = Mode.Paint);
        tyreModeButton.onClick.AddListener(() => currentMode = Mode.Tyres);

        // Color wheel
        colorWheel.onColorChanged.AddListener(ApplyColor);
    }

    private void OnDisable()
    {
        MarkerSpawner.OnMarkerPlaced -= OnMarkerPlaced;

        paintModeButton.onClick.RemoveAllListeners();
        tyreModeButton.onClick.RemoveAllListeners();
        colorWheel.onColorChanged.RemoveAllListeners();
    }

    private void OnMarkerPlaced(GameObject car)
    {
        currentCar = car;
        paintSlots.Clear();
        tyreSlots.Clear();

        bool isMcLaren = car.name.Contains("McLaren");
        paintOriginal = isMcLaren ? mclarenOriginalPaint : dodgeOriginalPaint;
        tyreOriginal = isMcLaren ? mclarenOriginalTyre : dodgeOriginalTyre;

        if (paintOriginal != null)
            CacheSlots(car, paintOriginal, paintSlots);
        if (tyreOriginal != null)
            CacheSlots(car, tyreOriginal, tyreSlots);
    }

    private void CacheSlots(GameObject car, Material original, Dictionary<Renderer, int[]> dict)
    {
        foreach (var rend in car.GetComponentsInChildren<Renderer>())
        {
            var shared = rend.sharedMaterials;
            var list = new List<int>();
            for (int i = 0; i < shared.Length; i++)
                if (shared[i] == original)
                    list.Add(i);
            if (list.Count > 0)
                dict[rend] = list.ToArray();
        }
    }

    private void ApplyColor(Color c)
    {
        if (currentCar == null) return;

        var target = (currentMode == Mode.Paint) ? paintSlots : tyreSlots;
        foreach (var kv in target)
        {
            var rend = kv.Key;
            var mats = rend.materials;
            foreach (int idx in kv.Value)
            {
                if (mats[idx].HasProperty("_BaseColor"))
                    mats[idx].SetColor("_BaseColor", c);
                else if (mats[idx].HasProperty("_Color"))
                    mats[idx].SetColor("_Color", c);
            }
            rend.materials = mats;
        }
    }
}
