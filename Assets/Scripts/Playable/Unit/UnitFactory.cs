using UnityEngine;
using static CustomResourceManagement.Prefabs.Playable;


public class UnitFactory : PlayableAbsFactory<Unit>
{
    private SelectionIndicatorFactory selectionIndicatorFactory;

    public UnitFactory(SelectionIndicatorFactory selectionIndicatorFactory)
    {
        this.selectionIndicatorFactory = selectionIndicatorFactory;
    }
    public override Unit Create(EntityData data)
    {
        var prefab = data.Prefab.GetComponent<Unit>();
        var unit = GameObject.Instantiate<Unit>(prefab);

        var selectionIndicator = selectionIndicatorFactory.Create();
        selectionIndicator.parent = unit.transform;
        selectionIndicator.localPosition = Vector3.zero.WithY(0.01f);
        selectionIndicator.GetChild(0).localScale = (Vector3.one * data.RadiusOnTerrain).WithZ(1);

        return unit.SetUp(data, selectionIndicator.gameObject);
    }
}