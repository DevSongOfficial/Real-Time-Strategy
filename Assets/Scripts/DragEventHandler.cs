using System;
using System.Collections.Generic;
using UnityEngine;
using CustomResourceManagement;

public sealed class DragEventHandler
{
    private Camera camera;
    private Canvas canvas;
    private IUnitSelector unitSelector;
    private RectTransform selectionBox;

    private Vector2 startPosition;
    private Vector2 endPosition;

    private List<ISelectable> unitsInSelectionBox;
    public event Action<List<ISelectable>> OnUnitDetectedInDragArea;

    public DragEventHandler(Camera camera, Canvas canvas, IUnitSelector unitSelector)
    {
        this.camera = camera;
        this.canvas = canvas;
        this.unitSelector = unitSelector;
        selectionBox = GameObject.Instantiate(ResourceLoader.GetResource<RectTransform>(Prefabs.UI.SelectionBox), canvas.transform);


        startPosition = Vector2.zero;
        endPosition = Vector2.zero;

        DrawSelectionBox();
    }

    // This function should be called every frame to handle drag input.
    public void HandleDragEvent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;

            DrawSelectionBox();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DetectUnitsInSelectionBox();

            startPosition = Vector2.zero;
            endPosition = Vector2.zero;

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


        unitsInSelectionBox = new List<ISelectable>();

        foreach (var unit in unitSelector.EnumerateAllUnits())
        {
            if (unit is not ITransformProvider transformProvider) return;

            var screenPosition = camera.WorldToScreenPoint(transformProvider.GetTransform().position);
            if (selectionRect.Contains(screenPosition))
            {
                unitsInSelectionBox.Add(unit);
            }
        }

        if (unitsInSelectionBox.Count > 0) 
            OnUnitDetectedInDragArea?.Invoke(unitsInSelectionBox);
    }
}