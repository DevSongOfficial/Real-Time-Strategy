using UnityEngine;
using static CustomResourceManagement.Prefabs.Playable;


public class UnitFactory : PlayableAbsFactory<Unit>
{
    private IPlacementEvent placementEvent;

    public UnitFactory(ISelectionEvent selectionEvent, SelectionIndicatorFactory selectionIndicatorFactory, IPlacementEvent placementEvent)
    {
        this.selectionIndicatorFactory = selectionIndicatorFactory;
        this.placementEvent = placementEvent;

        base.Setup(selectionEvent, selectionIndicatorFactory);
    }
    public override Unit Create(EntityData data, Team team)
    {
        var prefab = data.Prefab.GetComponent<Unit>();
        var unit = GameObject.Instantiate<Unit>(prefab);

        // Set selection indicator.
        var selectionIndicator = selectionIndicatorFactory.Create();
        selectionIndicator.parent = unit.transform;
        selectionIndicator.localPosition = Vector3.zero.WithY(-unit.PositionDeltaY) + data.SelectionIndicatorPositionOffset;
        selectionIndicator.GetChild(0).localScale = (Vector3.one * data.RadiusOnTerrain).WithZ(1);
        selectionIndicator.gameObject.SetActive(false);

        return unit.SetUp(data, team, selectionIndicator.gameObject, placementEvent);
    }
}