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
    [SerializeField] private List<Button> progressInfoButtons;
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

        // TODO: make these be executed only when a new building is selected or some barracks-related events occur. not every frame.
        BindButtonEvents(building);
        FillTrainingSprites(building);
    }

    private void BindButtonEvents(Building building)
    {
        if (building is not Barracks barracks || building.CurrentState is BuildingUnderConstructionState)
            return;

        foreach (var button in progressInfoButtons)
            button.onClick.RemoveAllListeners();
        
        for (int i = 0; i < barracks.GetUnitCountInQueue(); i++)
        {
            int index = i;
            progressInfoButtons[i].onClick.AddListener(() => barracks.DequeueUnit(index));
        }
    }

    private void FillTrainingSprites(Building building)
    {
        if (building is not Barracks barracks  || 
            building.CurrentState is BuildingUnderConstructionState)
        {
            for (int i = 0; i < progressInfoButtons.Count; i++)
            {
                progressInfoButtons[i].image.sprite = null;
                progressInfoButtons[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < progressInfoButtons.Count; i++)
            {
                var maxCount = barracks.GetData().GenerationSlotCount;
                progressInfoButtons[i].enabled = i < maxCount;
            }
            int j = 0;
            foreach (var sprite in barracks.GetTraningUnitSprites())
            {
                progressInfoButtons[j].image.sprite = sprite;

                j++;
            }
            for (; j < barracks.GetData().GenerationSlotCount; j++)
            {
                progressInfoButtons[j].image.sprite = null;
            }
        }
    }
}