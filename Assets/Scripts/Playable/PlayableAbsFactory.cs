using CustomResourceManagement;
using NUnit.Framework;
using UnityEngine;

public abstract class PlayableAbsFactory<T> where T : Playable
{
    protected SelectionIndicatorFactory selectionIndicatorFactory;


    protected void Setup(SelectionIndicatorFactory selectionIndicatorFactory)
    {
        this.selectionIndicatorFactory = selectionIndicatorFactory;
    }

    protected GameObject CreateSelectionIndicator(Playable playable, Vector3 offset = default)
    {
        var indicator = selectionIndicatorFactory.Create();
        indicator.SetParent(playable.transform, false);
        indicator.localPosition = Vector3.zero.WithY(-playable.GetPositionDeltaY()) + offset;
        indicator.GetChild(0).localScale = (Vector3.one * playable.GetData().RadiusOnTerrain).WithZ(1);
        indicator.gameObject.SetActive(false);
        return indicator.gameObject;
    }

    public abstract T Create(EntityData data, TeamContext teamContext);
}
