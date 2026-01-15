using UnityEngine;
using CustomResourceManagement;

public class HealthBarGenerator
{
    private Transform canvas;
    private CameraController cameraController;

    public HealthBarGenerator(Transform canvas, CameraController cameraController)
    {
        this.canvas = canvas;
        this.cameraController = cameraController;
    }

    public void GenerateAndSetTargetUnit(ITarget target)
    {
        var prefab_HealthBar = ResourceLoader.GetResource<HealthTracker>(Prefabs.UI.HealthTracker);
        var newHealthBar = GameObject.Instantiate<HealthTracker>(prefab_HealthBar, canvas.transform);
        newHealthBar.SetUp(cameraController, target);
    }
}
