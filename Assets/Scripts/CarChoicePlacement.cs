using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class CarChoicePlacement : MonoBehaviour
{
    public ARPlacementInteractable mclarenPlacement;
    public ARPlacementInteractable dodgePlacement;
    public Button mclarenButton;
    public Button dodgeButton;

    private void Awake()
    {
        mclarenButton.onClick.AddListener(ActivateMcLaren);
        dodgeButton.onClick.AddListener(ActivateDodge);
        ActivateMcLaren();
    }

    private void OnDestroy()
    {
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
