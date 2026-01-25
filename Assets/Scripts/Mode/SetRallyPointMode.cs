using System;
using UnityEngine;

public class SetRallyPointMode : ModeBase
{
    private readonly RallyPointSetter controller;
    private readonly InputManager inputManager;

    public SetRallyPointMode(InputManager inputManager, RallyPointSetter controller)
    {
        this.controller = controller;
        this.inputManager = inputManager;
    }

    public override void Enter()
    {
        controller.StartSettingRallyPoint();
    }

    public override void Exit()
    {
        controller.StopSettingRallyPoint();
    }

    public override void HandleInput()
    {
        if (!inputManager.IsPointerInClickableArea()) return;

        if (inputManager.GetMouseButtonDown(0))
            controller.SetRallyPoint(inputManager.GetMousePositionOnGround());

        if (inputManager.GetMouseButtonDown(1))
            transitionRequester.RequestTransition(Mode.Normal);
    }

    public override void Update() {}
}