using UnityEngine;

[CreateAssetMenu(fileName = "ResourceProviderData", menuName = "Scriptable Objects/Building Data/Resource Provider Data")]
public class ResourceProviderData : BuildingData
{
    [field: SerializeField] public ResourceType ResourceType { get; private set; }
    [field: SerializeField] public int AmountPerAction { get; private set; }
    [field: SerializeField] public int TimeToHarvest { get; private set; }
}