using CustomResourceManagement;
using Unity.AppUI.UI;
using UnityEngine;

public class UnitGenerator
{
    public System.Action<Unit> OnUnitGenerated;

    private UnitFactory unitFactory;
    IUnitRegisterer unitRegistry;

    public UnitGenerator(UnitFactory unitFactory, IUnitRegisterer unitRegistry)
    {
        this.unitFactory = unitFactory;
        this.unitRegistry = unitRegistry;
    }

    public void Generate(EntityData unitData, int numberOfUnit = 1)
    {
        for (int i = 0; i < numberOfUnit; i++)
        {
            var newUnit = unitFactory.Create(unitData, i > 3 ? Team.Green : Team.Red);
            var randomPosition = new Vector3(Random.Range(26, 35), 2, Random.Range(20, 36));
            newUnit.SetPosition(randomPosition);

            unitRegistry.RegisterUnit(newUnit);

            OnUnitGenerated.Invoke(newUnit);
        }
    }
}