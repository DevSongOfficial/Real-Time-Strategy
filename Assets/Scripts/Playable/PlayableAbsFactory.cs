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

    protected GameObject CreateSelectionIndicator(Playable playable, int radius, Vector3 offset = default)
    {
        if (selectionIndicatorFactory == null) return null;

        var indicator = selectionIndicatorFactory.Create();
        indicator.SetParent(playable.transform, false);
        indicator.localPosition = Vector3.zero.WithY(-playable.GetPositionDeltaY()) + offset;
        indicator.GetChild(0).localScale = (Vector3.one * radius).WithZ(1);
        indicator.gameObject.SetActive(false);
        return indicator.gameObject;
    }

    public abstract T Create(EntityData data, TeamContext teamContext);
}
