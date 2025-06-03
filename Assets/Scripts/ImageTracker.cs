using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracker : MonoBehaviour
{
    public GameObject mclarenPrefab;
    public GameObject dodgePrefab;
    private ARTrackedImageManager _imageManager;
    private Dictionary<string, GameObject> _spawned = new Dictionary<string, GameObject>();

    void Awake()
    {
        _imageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        _imageManager.trackedImagesChanged += OnImagesChanged;
    }

    void OnDisable()
    {
        _imageManager.trackedImagesChanged -= OnImagesChanged;
    }

    void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Newly detected images
        foreach (var tracked in args.added)
            InstantiateFor(tracked);

        // Updated (move/rotate or hide if lost)
        foreach (var tracked in args.updated)
        {
            if (_spawned.TryGetValue(tracked.referenceImage.name, out var go))
            {
                go.SetActive(tracked.trackingState == TrackingState.Tracking);
                go.transform.position = tracked.transform.position;
                go.transform.rotation = tracked.transform.rotation;
            }
        }

        // Removed images
        foreach (var tracked in args.removed)
        {
            if (_spawned.TryGetValue(tracked.referenceImage.name, out var go))
            {
                Destroy(go);
                _spawned.Remove(tracked.referenceImage.name);
            }
        }
    }

    private void InstantiateFor(ARTrackedImage tracked)
    {
        GameObject prefabToSpawn = null;
        switch (tracked.referenceImage.name)
        {
            case "McLaren":
                prefabToSpawn = mclarenPrefab;
                break;
            case "Dodge":
                prefabToSpawn = dodgePrefab;
                break;
        }

        if (prefabToSpawn != null && !_spawned.ContainsKey(tracked.referenceImage.name))
        {
            var instance = Instantiate(
                prefabToSpawn,
                tracked.transform.position,
                tracked.transform.rotation
            );
            instance.name = tracked.referenceImage.name;
            _spawned[tracked.referenceImage.name] = instance;
        }
    }
}
