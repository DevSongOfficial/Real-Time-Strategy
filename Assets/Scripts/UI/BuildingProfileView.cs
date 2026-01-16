using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingProfileView : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [Space]
    [SerializeField] private TextMeshProUGUI displayName;
    [SerializeField] private TextMeshProUGUI progressLabelText;
    [SerializeField] private Image progressFill;

    public void Refresh(Building building)
    {
        var data = building.GetData();

        // Set static data
        profileImage.sprite = data.ProfileSprite;
        displayName.text = data.DisplayName;
        
        // Set dynamic data
        healthText.text = $"{building.GetHealthSystem().CurrentHealth} / {data.MaxHealth}";
        progressLabelText.text = $"...";
        progressFill.fillAmount = building.GetProgressRate();

    }
}
