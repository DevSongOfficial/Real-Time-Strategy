using System;
using UnityEngine;

public class SetPositionMode : ModeBase
{
    private readonly SpawnPositionSetter controller;
    private readonly InputManager inputManager;

    public SetPositionMode(InputManager inputManager, SpawnPositionSetter controller)
    {
        this.controller = controller;
        this.inputManager = inputManager;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void HandleInput()
    {
        if (!inputManager.IsPointerInClickableArea()) return;

        if (inputManager.GetMouseButtonDown(0))
            controller.UpdatePosition(inputManager.GetSelectedMapPosition());

        if (inputManager.GetMouseButtonDown(1))
            controller.StopSettingSpawnerPosition();

    }

    public override void Update()
    {        
    }

}
