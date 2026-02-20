using UnityEngine;
using CustomResourceManagement;
using System.Collections.Generic;

public class HealthBarGenerator
{
    private Transform canvas;
    private CameraController cameraController;

    private Dictionary<ITarget, HealthTracker> healthTrackers;

    public HealthBarGenerator(Transform canvas, CameraController cameraController)
    {
        this.canvas = canvas;
        this.cameraController = cameraController;

        healthTrackers = new Dictionary<ITarget, HealthTracker>();
    }

    public void GenerateAndSetTargetUnit(ITarget target)
    {
        var prefab_HealthBar = ResourceLoader.GetResource<HealthTracker>(Prefabs.UI.HealthTracker);
        var newHealthBar = GameObject.Instantiate<HealthTracker>(prefab_HealthBar, canvas.transform);
        newHealthBar.SetUp(cameraController, target);

        healthTrackers.Add(target, newHealthBar);
    }

    public void UnsetTargetUnit(ITarget target)
    {
        healthTrackers[target].Dispose();
    }
}
