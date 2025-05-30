using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ScalingPrefab : MonoBehaviour
{
    [Header("Marker Prefabs")]
    public GameObject mclarenPrefab;
    public GameObject dodgePrefab;

    [Header("Toy Size (meters)")]
    [Tooltip("How long (Z-axis) the spawned car should be in the real world")]
    public float desiredLength = 0.15f; // 15 cm

    private Dictionary<string, GameObject> spawned = new Dictionary<string, GameObject>();
    private ARTrackedImageManager manager;

    void Awake()
    {
        manager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        manager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        manager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // 1) Added: spawn & scale
        foreach (var added in args.added)
        {
            string key = added.referenceImage.name;
            if (spawned.ContainsKey(key)) continue;

            GameObject prefab = key == "McLarenMarker" ? mclarenPrefab
                              : key == "DodgeMarker" ? dodgePrefab
                              : null;
            if (prefab == null) continue;

            var go = Instantiate(prefab, added.transform.position, added.transform.rotation);
            ScaleToLength(go);
            spawned[key] = go;
        }

        // 2) Updated: show/hide + reposition
        foreach (var updated in args.updated)
        {
            if (!spawned.TryGetValue(updated.referenceImage.name, out var go)) continue;

            bool tracking = updated.trackingState == TrackingState.Tracking;
            go.SetActive(tracking);
            if (tracking)
            {
                go.transform.position = updated.transform.position;
                go.transform.rotation = updated.transform.rotation;
            }
        }

        // 3) Removed: destroy
        foreach (var removed in args.removed)
        {
            if (spawned.TryGetValue(removed.referenceImage.name, out var go))
            {
                Destroy(go);
                spawned.Remove(removed.referenceImage.name);
            }
        }
    }

    void ScaleToLength(GameObject car)
    {
        var rend = car.GetComponentInChildren<Renderer>();
        if (rend == null) return;

        float modelLength = rend.bounds.size.z;
        if (modelLength <= 0f) return;

        float factor = desiredLength / modelLength;
        car.transform.localScale = car.transform.localScale * factor;
    }
}
