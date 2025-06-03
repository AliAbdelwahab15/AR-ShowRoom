using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class MarkerSpawner : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject mclarenPrefab;
    public GameObject dodgePrefab;
    // Fires whenever we instantiate a new car from a marker
    public static event Action<GameObject> OnMarkerPlaced;
    // Keep track of spawned instances by marker name
    public readonly Dictionary<string, GameObject> _spawned = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImagesChanged;
    }

    private void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // 1) Added: spawn new cars
        foreach (var tracked in args.added)
            SpawnCarFor(tracked);

        // 2) Updated: move or hide existing cars
        foreach (var tracked in args.updated)
        {
            if (_spawned.TryGetValue(tracked.referenceImage.name, out var go))
            {
                bool visible = tracked.trackingState == TrackingState.Tracking;
                go.SetActive(visible);

                if (visible)
                {
                    go.transform.position = tracked.transform.position;
                    go.transform.rotation = tracked.transform.rotation;
                }
            }
        }

        // 3) Removed: destroy lost cars
        foreach (var tracked in args.removed)
        {
            if (_spawned.TryGetValue(tracked.referenceImage.name, out var go))
            {
                Destroy(go);
                _spawned.Remove(tracked.referenceImage.name);
            }
        }
    }

    private void SpawnCarFor(ARTrackedImage tracked)
    {
        GameObject prefab = null;
        switch (tracked.referenceImage.name)
        {
            case "McLaren":
                prefab = mclarenPrefab;
                break;
            case "Dodge":
                prefab = dodgePrefab;
                break;
        }

        if (prefab == null || _spawned.ContainsKey(tracked.referenceImage.name))
            return;

        var instance = Instantiate(
            prefab,
            tracked.transform.position,
            tracked.transform.rotation
        );

        instance.name = tracked.referenceImage.name;
        _spawned[tracked.referenceImage.name] = instance;

        // Notify any color-changer or other system that a new car appeared
        OnMarkerPlaced?.Invoke(instance);
    }
}
