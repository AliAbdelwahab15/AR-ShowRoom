using UnityEngine;
using UnityEngine.UI;

public class RearLeftDoor : MonoBehaviour
{
    public Button toggleButton;
    public Animator doorAnimator;
    public bool isOpen;

    private void Awake()
    {
        Transform root = transform.root;
        Transform pivot = FindDeep(root, "RearLeftPivot");
        if (pivot == null)
        {
            enabled = false;
            return;
        }

        doorAnimator = pivot.GetComponent<Animator>();
        if (doorAnimator == null || doorAnimator.runtimeAnimatorController == null)
        {
            enabled = false;
            return;
        }

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
