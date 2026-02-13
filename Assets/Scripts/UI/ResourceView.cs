using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ResourceView : MonoBehaviour
{
    [Header("Resource Amount")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI capacityText;

    public void UpdateResourceText(ResourceType type, int amount)
    {
        if(type == ResourceType.Gold)
            goldText.text = "Gold:" + amount.ToString();
        else if (type == ResourceType.Wood)
            woodText.text = "Wood:" + amount.ToString();
    }



    public void UpdateCapacityText(int amount, int total)
    {
        capacityText.text = $"Capacity: {amount} / {total}";
    }
}
