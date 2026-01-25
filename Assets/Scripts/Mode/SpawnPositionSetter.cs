using System;
using UnityEngine;

public class SpawnPositionSetter
{
    private Transform spawnPositionIndicator;
    private Transform mousePositionIndicator;
    public IUnitGenerator Building {  get; private set; }

    public SpawnPositionSetter(Transform spawnPositionIndicator, Transform mousePositionIndicator)
    {
        this.spawnPositionIndicator = spawnPositionIndicator;
        this.mousePositionIndicator = mousePositionIndicator;
    }

    public void SetSpawner(IUnitGenerator building)
    {
        Building = building;
        spawnPositionIndicator.position = building.GetUnitSpawnPosition();
        spawnPositionIndicator.gameObject.SetActive(true);
    }

    public void StartSettingSpawnPoint()
    {
        if (Building == null) return;

        mousePositionIndicator.gameObject.SetActive(true);
    }

    public void UpdateSpawnPoint(Vector3 position)
    {
        if (Building == null) return;

        spawnPositionIndicator.position = position;
        Building.SetUnitSpawnPosition(position);
    }

    public void StopSettingSpawnPoint()
    {
        Building = null;
        spawnPositionIndicator.gameObject.SetActive(false);
        mousePositionIndicator.gameObject.SetActive(false);
    }
}