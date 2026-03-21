using NUnit.Framework;
using UnityEngine;
using static CustomResourceManagement.Prefabs.Playable;


public class UnitFactory : PlayableAbsFactory<Unit>
{
    private readonly IPlacementEvent placementEvent;
    private readonly EntityProfilePanel profilePanel;
    private readonly GridSystem gridSystem;

    public UnitFactory(SelectionIndicatorFactory selectionIndicatorFactory, 
        IPlacementEvent placementEvent, EntityProfilePanel profilePanel,GridSystem gridSystem)
    {
        this.selectionIndicatorFactory = selectionIndicatorFactory;
        this.placementEvent = placementEvent;
        this.profilePanel = profilePanel;
        this.gridSystem = gridSystem;

        base.Setup(selectionIndicatorFactory);
    }
    public override Unit Create(EntityData data, TeamContext teamContext)
    {
        var prefab = data.Prefab.GetComponent<Unit>();
        var unit = GameObject.Instantiate<Unit>(prefab);

        // Set selection indicator.
        var selectionIndicator = CreateSelectionIndicator(unit, unit.GetData().SelectionIndicatorPositionOffset);

        return unit.SetUp(data, teamContext, selectionIndicator, profilePanel, placementEvent, gridSystem);
    }
}