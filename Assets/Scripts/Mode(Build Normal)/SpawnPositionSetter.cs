using System;
using UnityEngine;

public class SpawnPositionSetter
{
    private Transform positionIndicator;
    private IUnitGenerator building;

    public event Action OnExitRequested;


    public SpawnPositionSetter(Transform positionIndicator)
    {
        this.positionIndicator = positionIndicator;
    }

    public void SetSpawner(IUnitGenerator building)
    {
        this.building = building;
        positionIndicator.position = building.GetUnitSpawnPosition();
        positionIndicator.gameObject.SetActive(true);
    }

    public void StopSettingSpawnerPosition()
    {
        building = null;
        positionIndicator.gameObject.SetActive(false);

        OnExitRequested?.Invoke();
    }

    public void UpdatePosition(Vector3 position)
    {
        if (building == null) return;

        positionIndicator.position = position;
        building.SetUnitSpawnPosition(position);
    }
}