using System.Collections.Generic;
using UnityEngine;
using static CustomResourceManagement.Prefabs.Playable;
public enum ResourceType
{
    Gold, Wood,
}

public class ResourceBank
{
    private readonly Dictionary<ResourceType, int> resourceAmounts;
    private readonly ResourceView view;

    public ResourceBank(ResourceView view)
    {
        resourceAmounts = new Dictionary<ResourceType, int>()
        {
            { ResourceType.Gold, 0 },
            { ResourceType.Wood, 0 },
        };

        this.view = view;
        view.UpdateResourceText(ResourceType.Gold, GetResourceAmount(ResourceType.Gold));
        view.UpdateResourceText(ResourceType.Wood, GetResourceAmount(ResourceType.Wood));

    }

    public ResourceBank()
    {
        resourceAmounts = new Dictionary<ResourceType, int>()
        {
            { ResourceType.Gold, 0 },
            { ResourceType.Wood, 0 },
        };
    }

    public void AddResource(ResourceType type, int amount)
    {
        resourceAmounts[type] += amount;
        view?.UpdateResourceText(type, GetResourceAmount(type));
    }

    public bool SpendResource(ResourceType type, int amount)
    {
        if (resourceAmounts[type] < amount)
            return false;

        resourceAmounts[type] -= amount;
        view?.UpdateResourceText(type, GetResourceAmount(type));
        return true;
    }

    public int GetResourceAmount(ResourceType type)
    {
        return resourceAmounts[type];
    }

    public bool CanBuild(BuildingData data)
    {
        return data.GoldRequired <= GetResourceAmount(ResourceType.Gold)
            && data.WoodRequired <= GetResourceAmount(ResourceType.Wood) ;
    }
}