using System.Collections.Generic;
using UnityEngine;

public interface IRegisterUnit
{
    void RegisterUnit(Unit unit);
}

public class UnitRegistry : IRegisterUnit
{
    private List<Unit> allUnits;

    public UnitRegistry() { allUnits = new List<Unit>();}

    public void RegisterUnit(Unit unit)
    {
        allUnits.Add(unit);
    }

    public void UnregisterUnit(Unit unit)
    {
        allUnits.Remove(unit);
    }

    public IEnumerable<ITransformProvider> GetTransforms()
    {
        return allUnits;
    }
}
