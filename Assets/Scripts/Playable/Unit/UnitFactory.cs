using UnityEngine;


public class UnitFactory : PlayableAbsFactory<Unit>
{
    public override Unit Create(EntityData data)
    {
        var prefab = data.Prefab.GetComponent<Unit>();
        var unit = GameObject.Instantiate<Unit>(prefab);
        return unit.SetUp(data);
    }
}