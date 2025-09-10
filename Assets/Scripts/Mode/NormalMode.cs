using UnityEngine;
using UnityEngine.EventSystems;

public sealed class NormalMode : ModeBase
{
    private readonly SelectionHandler selectionHandler;
    private readonly DragEventHandler dragEventHandler;

    public NormalMode(SelectionHandler selectionHandler, DragEventHandler dragEventHandler)
    {
        this.selectionHandler = selectionHandler;
        this.dragEventHandler = dragEventHandler;
    }

    public override void Enter() 
    {
        dragEventHandler.OnUnitDetectedInDragArea += selectionHandler.SelectUnits;
    }
    public override void Exit() 
    {
        dragEventHandler.OnUnitDetectedInDragArea -= selectionHandler.SelectUnits;
    }

    public override void Update()
    {
        dragEventHandler.HandleDragEvent();
    }

    public override void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
            selectionHandler.SelectUnit(Input.mousePosition, additive: Input.GetKey(KeyCode.LeftShift));

        if (Input.GetMouseButtonDown(1))
            selectionHandler.SelectTarget(Input.mousePosition);
    }
}
