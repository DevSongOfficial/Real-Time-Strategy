using UnityEngine;

// Buildings that provide resources such as Gold, Wood, Food
public class ResourceProvider : Building
{
    public new ResourceProviderData GetData()
    {
        return base.GetData() as ResourceProviderData;
    }
}