using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitProfileView : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [Space]
    [SerializeField] private TextMeshProUGUI displayName;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI attackRangeText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;


    public void Refresh(Unit unit)
    {
        var data = unit.GetData();

        // Set static data
        profileImage.sprite = data.ProfileSprite;
        displayName.text = data.DisplayName;

        // Set dynamic data
        healthText.text = $"{unit.GetHealthSystem().CurrentHealth} / {data.MaxHealth}";
        damageText.text = data.Combat.AttackDamage.ToString();
        attackRangeText.text = data.Combat.AttackRange.ToString();
        attackSpeedText.text = $"{(1 / data.Combat.AttackCooldown):0.0}";
    }
}
