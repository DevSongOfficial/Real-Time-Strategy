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
        controller.StartSettingSpawnPoint();
    }

    public override void Exit()
    {
        controller.StopSettingSpawnPoint();
    }

    public override void HandleInput()
    {
        if (!inputManager.IsPointerInClickableArea()) return;

        if (inputManager.GetMouseButtonDown(0))
            controller.UpdateSpawnPoint(inputManager.GetMousePositionOnGround());

        if (inputManager.GetMouseButtonDown(1))
            transitionRequester.RequestTransition(Mode.Normal);
    }

    public override void Update()
    {        
    }
}


