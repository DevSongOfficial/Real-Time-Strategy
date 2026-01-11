using System;
using System.Collections.Generic;
using UnityEngine;
using CustomResourceManagement;

public sealed class DragEventHandler
{
    private InputManager inputManager;
    private Camera camera;
    private RectTransform selectionBox;

    private bool isDragging;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private IEnumerable<ITransformProvider> units;
    private List<ITransformProvider> unitsInSelectionBox;
    public event Action<IEnumerable<ISelectable>> OnUnitDetectedInDragArea;

    // Only units can be dragged, not buildings.
    public DragEventHandler(IEnumerable<ITransformProvider> units , Camera camera, Canvas canvas, InputManager inputManager)
    {
        this.units = units;
        this.inputManager = inputManager;

        this.camera = camera;
        selectionBox = GameObject.Instantiate(ResourceLoader.GetResource<RectTransform>(Prefabs.UI.SelectionBox), canvas.transform);

        startPosition = Vector2.zero;
        endPosition = Vector2.zero;

        DrawSelectionBox();
    }

    // This function should be called every frame to handle drag input.
    public void HandleDragEvent()
    {
        if (inputManager.GetMouseButtonDown(0) && inputManager.IsPointerInClickableArea())
        {
            startPosition = inputManager.GetMousePosition();
            isDragging = true;
        }

        if (!isDragging) return;

        if (inputManager.GetMouseButton(0))
        {
            endPosition = inputManager.GetMousePosition();

            DrawSelectionBox();
        }

        if (inputManager.GetMouseButtonUp(0))
        {
            DetectUnitsInSelectionBox();

            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            isDragging = false;

            DrawSelectionBox();
        }
    }

    private void DrawSelectionBox()
    {
        selectionBox.position = (startPosition + endPosition) / 2; ;
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(startPosition.x - endPosition.x), Mathf.Abs(startPosition.y - endPosition.y)); ;
    }

    private void DetectUnitsInSelectionBox()
    {
        // Rect for using contains function to detect gameObjects inside.
        Rect selectionRect = new Rect(
            Mathf.Min(startPosition.x, endPosition.x),
            Mathf.Min(startPosition.y, endPosition.y),
            Mathf.Abs(endPosition.x - startPosition.x),
            Mathf.Abs(endPosition.y - startPosition.y)
        );


        unitsInSelectionBox = new List<ITransformProvider>();

        foreach (var unit in units)
        {
            var screenPosition = camera.WorldToScreenPoint(unit.GetTransform().position);
            if (selectionRect.Contains(screenPosition))
            {
                unitsInSelectionBox.Add(unit);
            }
        }

        if (unitsInSelectionBox.Count > 0) 
            OnUnitDetectedInDragArea?.Invoke(unitsInSelectionBox.FilterByType<ITransformProvider, ISelectable>());
    }
}