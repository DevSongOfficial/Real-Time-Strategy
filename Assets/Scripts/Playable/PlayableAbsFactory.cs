using UnityEngine;
using CustomResourceManagement;

public abstract class PlayableAbsFactory<T> where T : Playable
{
    protected ISelectionEvent selectionEvent;
    protected SelectionIndicatorFactory selectionIndicatorFactory;

    protected void Setup(ISelectionEvent selectionEvent, SelectionIndicatorFactory selectionIndicatorFactory)
    {
        this.selectionEvent = selectionEvent;
        this.selectionIndicatorFactory = selectionIndicatorFactory;
    }

    public abstract T Create(EntityData data, Team team);
}
