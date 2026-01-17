using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CustomResourceManagement.Prefabs.Playable;

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
        Barracks barracks = building as Barracks;
        if(barracks.CurrentState is not BuildingUnitTrainState)
        {
            for (int i = 0; i < progressInfoImages.Count; i++)
            {
                progressInfoImages[i].sprite = null;
                progressInfoImages[i].enabled = false;
            }

            return;
        }

        int j = 0;
        foreach (var sprite in building.GetTraningUnitSprites())
        {
            progressInfoImages[j].sprite = sprite;
            progressInfoImages[j].enabled = true;

            j++;
        }
        for (; j < barracks.GetData().GenerationSlotCount; j++)
        {
            progressInfoImages[j].enabled = true;
            progressInfoImages[j].sprite = null;
        }
    }
}
