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
        spawnPositionIndicator.gameObject.SetActive(true);
        mousePositionIndicator.gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 position)
    {
        spawnPositionIndicator.position = position;
        Building.SetUnitSpawnPosition(position);
    }

    public void StopSettingSpawnPoint()
    {
        spawnPositionIndicator.gameObject.SetActive(false);
        mousePositionIndicator.gameObject.SetActive(false);
    }
}