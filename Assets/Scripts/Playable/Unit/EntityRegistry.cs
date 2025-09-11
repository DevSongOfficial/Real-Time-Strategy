using System.Collections.Generic;
using UnityEngine;

public interface IUnitRegisterer
{
    void RegisterUnit(Unit unit);
}


public class EntityRegistry : IUnitRegisterer
{
    private List<Unit> allUnits;
    private List<ISelectable> selectedEntities; // entities selected(clicked or dragged) by player.


    public EntityRegistry() 
    { 
        allUnits = new List<Unit>();
        selectedEntities = new List<ISelectable>();
    }

    public void RegisterUnit(Unit unit)
    {
        allUnits.Add(unit);
    }

    public void UnregisterUnit(Unit unit)
    {
        allUnits.Remove(unit);
    }

    public IEnumerable<ITransformProvider> GetTransformsOfUnits()
    {
        return allUnits;
    }

    public List<ISelectable> GetSelectedEntities()
    {
        return selectedEntities;
    }
}
