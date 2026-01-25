using System;
using UnityEngine;

public class RallyPointSetter
{
    private Transform rallyPositionIndicator;
    private Transform mousePositionIndicator;
    public IUnitGenerator Building {  get; private set; }

    public RallyPointSetter(Transform spawnPositionIndicator, Transform mousePositionIndicator)
    {
        this.rallyPositionIndicator = spawnPositionIndicator;
        this.mousePositionIndicator = mousePositionIndicator;
    }

    public void Setup(IUnitGenerator building)
    {
        Building = building;
        rallyPositionIndicator.position = building.GetUnitRallyPoint();
        rallyPositionIndicator.gameObject.SetActive(true);
    }

    public void StartSettingRallyPoint()
    {
        rallyPositionIndicator.gameObject.SetActive(true);
        mousePositionIndicator.gameObject.SetActive(true);
    }

    public void SetRallyPoint(Vector3 position)
    {
        rallyPositionIndicator.position = position;
        Building.SetUnitRallyPoint(position);
    }

    public void StopSettingRallyPoint()
    {
        rallyPositionIndicator.gameObject.SetActive(false);
        mousePositionIndicator.gameObject.SetActive(false);
    }
}