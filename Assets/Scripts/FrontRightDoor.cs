using UnityEngine;
using UnityEngine.UI;

public class FrontRightDoor : MonoBehaviour
{
    [Header("UI Toggle Button")]
    [SerializeField] private Button toggleButton;

    private Animator doorAnimator;
    private bool isOpen;

    private void Awake()
    {
        // find the pivot under this prefab instance
        Transform root = transform.root;
        Transform pivot = FindDeep(root, "FrontRightPivot");
        if (pivot == null)
        {
            //Debug.LogWarning("FrontRightPivot not found under prefab.");
            enabled = false;
            return;
        }

        // get its Animator
        doorAnimator = pivot.GetComponent<Animator>();
        if (doorAnimator == null || doorAnimator.runtimeAnimatorController == null)
        {
            //Debug.LogWarning("Animator or Controller missing on FrontRightPivot.");
            enabled = false;
            return;
        }

        // start closed
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
        if (doorAnimator == null) return;

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
