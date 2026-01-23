using System;
using UnityEngine;

public class SpawnPositionSetter
{
    private Transform positionIndicator;
    public IUnitGenerator Building {  get; private set; }

    public event Action OnExitRequested;


    public SpawnPositionSetter(Transform positionIndicator)
    {
        this.positionIndicator = positionIndicator;
    }

    public void SetSpawner(IUnitGenerator building)
    {
        Building = building;
        positionIndicator.position = building.GetUnitSpawnPosition();
        positionIndicator.gameObject.SetActive(true);
    }

    public void StopSettingSpawnerPosition()
    {
        Building = null;
        positionIndicator.gameObject.SetActive(false);
        OnExitRequested?.Invoke();
    }

    public void UpdatePosition(Vector3 position)
    {
        if (Building == null) return;

        positionIndicator.position = position;
        Building.SetUnitSpawnPosition(position);
    }
}