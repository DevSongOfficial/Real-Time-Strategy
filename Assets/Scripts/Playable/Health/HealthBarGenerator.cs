using UnityEngine;
using CustomResourceManagement;

public class HealthBarGenerator
{
    private Transform canvas;
    private Camera mainCamera;

    public HealthBarGenerator(Transform canvas, Camera mainCamera)
    {
        this.canvas = canvas;
        this.mainCamera = mainCamera;
    }

    public void GenerateAndSetTargetUnit(ITarget targetUnit)
    {
        var prefab_HealthBar = ResourceLoader.GetResource<HealthTracker>(Prefabs.UI.HealthTracker);
        var newHealthBar = GameObject.Instantiate<HealthTracker>(prefab_HealthBar, canvas.transform);
        newHealthBar.SetUp(mainCamera, new Target(targetUnit));
    }
}
