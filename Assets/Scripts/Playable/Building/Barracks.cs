using NUnit.Framework;
using UnityEngine;


public interface IUnitGenerator
{
    void Setup(UnitGenerator unitGenerator);
}

public class Barracks : Building, IUnitGenerator
{
    private new BarracksData data;
    private UnitGenerator unitGenerator;

    [SerializeField] private Vector2 spawnPointOffset; // building.position + spawnPoint would be the spawn point.

    public void Setup(UnitGenerator unitGenerator)
    {
        this.unitGenerator = unitGenerator;
        data = (BarracksData)base.data;
    }

    public void SpawnUnit()
    {
        
    }
}