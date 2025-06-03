using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CarPlacement : MonoBehaviour
{
    [SerializeField] private ARPlacementInteractable mclarenPlacement;
    [SerializeField] private ARPlacementInteractable dodgePlacement;
    [SerializeField] private Button mclarenButton;
    [SerializeField] private Button dodgeButton;

    private void Awake()
    {
        mclarenButton.onClick.AddListener(ActivateMcLaren);
        dodgeButton.onClick.AddListener(ActivateDodge);
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
    }

    private void ActivateDodge()
    {
        mclarenPlacement.enabled = false;
        dodgePlacement.enabled = true;
    }
}
