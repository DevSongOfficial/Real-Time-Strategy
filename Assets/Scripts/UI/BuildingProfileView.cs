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

        if (building is ResourceProvider)
            return;

        // TODO: make these be executed only when a new building is selected or some barracks-related events occur. not every frame.
        BindButtonEvents(building);
        FillTrainingSprites(building);
    }

    public void DisableProgressInfoButtons()
    {
        foreach (var button in progressInfoButtons)
            button.gameObject.SetActive(false);
    }

    public void ClearProgressInfoButtons()
    {
        foreach (var button in progressInfoButtons)
        {
            button.image.sprite = null;
            button.enabled = false;
        }
    }

    public void SetupUnitSlotSprites(int maxSlotCount)
    {
        for (int i = 0; i < maxSlotCount; i++)
        {
            progressInfoButtons[i].gameObject.SetActive(true);
        }
    }

    public void FillUnitSlotSprites(Sprite sprite, int index)
    {
        progressInfoButtons[index].image.sprite = sprite;
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
        if (building is not Barracks barracks  || building.CurrentState is BuildingUnderConstructionState)
        {
            ClearProgressInfoButtons();
        }
        else
        {
            var maxCount = barracks.GetData().GenerationSlotCount;
            for (int i = 0; i < maxCount; i++)
            {
                progressInfoButtons[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < progressInfoButtons.Count; i++)
            {
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