using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class InputManager
{
    private Vector3 lastPosition;
    private Camera camera;

    private List<RectTransform> nonClickableAreas;

    public InputManager(Camera camera, RectTransform nonClickableArea)
    {
        this.camera = camera;
         
        nonClickableAreas = new List<RectTransform>();
        for (int i = 0; i < nonClickableArea.childCount; i++)
            nonClickableAreas.Add(nonClickableArea.GetChild(i) as RectTransform);
    }

    public bool GetMouseButton(int button)
    {
        return Input.GetMouseButton(button);
    }

    public bool GetMouseButtonDown(int button)
    {
        return Input.GetMouseButtonDown(button);
    }

    public bool GetMouseButtonUp(int button)
    {
        return Input.GetMouseButtonUp(button);
    }

    public bool GetKey(KeyCode keyCode)
    {
        return Input.GetKey(keyCode);
    }

    public bool GetKeyDown(KeyCode keyCode)
    {
        return Input.GetKeyDown(keyCode);
    }

    public Vector3 GetMousePositionOnCanvas()
    {
        return Input.mousePosition;
    }

    public Vector3 GetMousePositionOnGround()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, Layer.Ground.ToLayerMask()))
            lastPosition = hit.point;

        return lastPosition;
    }

    public bool IsMouseScrolled(out MouseScrollType type)
    {
        float scroll = Input.mouseScrollDelta.y;
        
        if (scroll < -0.9f)
        {
            type = MouseScrollType.Down;
            return true;
        }
        else if (scroll > 0.9f)
        {
            type = MouseScrollType.Up;
            return true;
        }
        else
        {
            type = MouseScrollType.None;
            return false;
        }
    }


    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public bool IsPointerOverUI(RectTransform transform)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(transform, Input.mousePosition);
    }

    public bool IsPointerInClickableArea()
    {
        foreach (var area in nonClickableAreas)
            if (IsPointerOverUI(area))
                return false;

        return true;
    }
}

public enum MouseScrollType { None = 0, Up = 1, Down = -1}