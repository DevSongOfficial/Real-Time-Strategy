using System.Collections.Generic;
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
    [SerializeField] private List<Image> progressInfoImages;
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
        progressLabelText.text = building.GetProgressLabelName();
        progressFill.fillAmount = building.GetProgressRate();

        FillTrainingSprites(building);
    }

    private void FillTrainingSprites(Building building)
    {
        if (building is not Barracks barracks  || 
            building.CurrentState is BuildingUnderConstructionState || 
            building.CurrentState is BuildingIdleState )
        {
            for (int i = 0; i < progressInfoImages.Count; i++)
            {
                progressInfoImages[i].sprite = null;
                progressInfoImages[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < progressInfoImages.Count; i++)
            {
                var maxCount = barracks.GetData().GenerationSlotCount;
                progressInfoImages[i].enabled = i < maxCount;
            }
            int j = 0;
            foreach (var sprite in building.GetTraningUnitSprites())
            {
                progressInfoImages[j].sprite = sprite;

                j++;
            }
            for (; j < barracks.GetData().GenerationSlotCount; j++)
            {
                progressInfoImages[j].sprite = null;
            }
        }
    }
}