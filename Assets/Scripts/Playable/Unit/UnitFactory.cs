using UnityEngine;


public class UnitFactory : PlayableAbsFactory<Unit>
{
    public override Unit Create(EntityData data)
    {
        var unit = data.Prefab.GetComponent<Unit>();
        return GameObject.Instantiate<Unit>(unit);
    }

    public Unit Create(Transform prefab)
    {
        var unit = prefab.GetComponent<Unit>();
        return GameObject.Instantiate<Unit>(unit);
    }
}