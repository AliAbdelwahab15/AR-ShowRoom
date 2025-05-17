using UnityEngine;
using UnityEngine.UI;

public class RearLeftDoor : MonoBehaviour
{
    [Header("UI Toggle Button")]
    [SerializeField] private Button toggleButton;

    private Animator doorAnimator;
    private bool isOpen;

    private void Awake()
    {
        // Find the prefab root, then the RearLeftPivot under it
        Transform root = transform.root;
        Transform pivot = FindDeep(root, "RearLeftPivot");
        if (pivot == null)
        {
            //Debug.LogWarning("RearLeftPivot not found under prefab.");
            enabled = false;
            return;
        }

        // Grab its Animator and verify it has a controller
        doorAnimator = pivot.GetComponent<Animator>();
        if (doorAnimator == null || doorAnimator.runtimeAnimatorController == null)
        {
            //Debug.LogWarning("Animator or Controller missing on RearLeftPivot.");
            enabled = false;
            return;
        }

        // Reset triggers and start closed
        doorAnimator.ResetTrigger("OpenDoors");
        doorAnimator.ResetTrigger("CloseDoors");
        doorAnimator.SetTrigger("CloseDoors");
        isOpen = false;
    }

    private void Start()
    {
        if (toggleButton != null)
            toggleButton.onClick.AddListener(ToggleDoor);
    }

    private void OnDestroy()
    {
        if (toggleButton != null)
            toggleButton.onClick.RemoveListener(ToggleDoor);
    }

    private void ToggleDoor()
    {
        if (doorAnimator == null)
            return;

        if (isOpen)
        {
            doorAnimator.ResetTrigger("OpenDoors");
            doorAnimator.SetTrigger("CloseDoors");
        }
        else
        {
            doorAnimator.ResetTrigger("CloseDoors");
            doorAnimator.SetTrigger("OpenDoors");
        }

        isOpen = !isOpen;
    }

    // Recursive depth-first search for a child by exact name
    private Transform FindDeep(Transform parent, string name)
    {
        if (parent.name == name)
            return parent;

        foreach (Transform child in parent)
        {
            Transform found = FindDeep(child, name);
            if (found != null)
                return found;
        }
        return null;
    }
}
