using UnityEngine;
using System.Collections.Generic;
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
}