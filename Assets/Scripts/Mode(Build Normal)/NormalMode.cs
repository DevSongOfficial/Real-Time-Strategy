using UnityEngine;
using UnityEngine.EventSystems;

public sealed class NormalMode : ModeBase
{
    private readonly SelectionHandler selectionHandler;
    private readonly DragEventHandler dragEventHandler;

    private readonly InputManager inputManager;

    public NormalMode(InputManager inputManager, SelectionHandler selectionHandler, DragEventHandler dragEventHandler)
    {
        this.selectionHandler = selectionHandler;
        this.dragEventHandler = dragEventHandler;

        this.inputManager = inputManager;
    }

    public override void Enter()
    {
        dragEventHandler.OnUnitDetectedInDragArea += selectionHandler.SelectEntities;
    }

    public override void Exit()
    {
        dragEventHandler.OnUnitDetectedInDragArea -= selectionHandler.SelectEntities;
    }

    public override void Update()
    {
        dragEventHandler.HandleDragEvent();
    }

    public override void HandleInput()
    {
        if (!inputManager.IsPointerInClickableArea())
            return;

        if (inputManager.GetMouseButtonDown(0))
            selectionHandler.SelectTargetor(inputManager.GetMousePosition(), additive: inputManager.GetKey(KeyCode.LeftShift));
        else if (inputManager.GetMouseButtonDown(1))
            selectionHandler.SelectTarget(inputManager.GetMousePosition());
    }
}