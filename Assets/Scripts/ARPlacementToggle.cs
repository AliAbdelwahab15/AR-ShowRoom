using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class ARPlacementToggle : MonoBehaviour
{
    [Header("Your Two Placement Interactables")]
    [SerializeField] private ARPlacementInteractable mclarenPlacement;
    [SerializeField] private ARPlacementInteractable dodgePlacement;

    [Header("UI Buttons")]
    [SerializeField] private Button mclarenButton;
    [SerializeField] private Button dodgeButton;

    private void Awake()
    {
        // hook up button clicks
        mclarenButton.onClick.AddListener(ActivateMcLaren);
        dodgeButton.onClick.AddListener(ActivateDodge);

        // optional: start with none or one active
        ActivateMcLaren();
    }

    private void OnDestroy()
    {
        // clean up listeners
        mclarenButton.onClick.RemoveListener(ActivateMcLaren);
        dodgeButton.onClick.RemoveListener(ActivateDodge);
    }

    private void ActivateMcLaren()
    {
        mclarenPlacement.enabled = true;
        dodgePlacement.enabled = false;
        Debug.Log("ARPlacementToggle: McLaren placer active");
    }

    private void ActivateDodge()
    {
        mclarenPlacement.enabled = false;
        dodgePlacement.enabled = true;
        Debug.Log("ARPlacementToggle: Dodge placer active");
    }
}
