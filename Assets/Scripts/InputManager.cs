using UnityEngine;
using UnityEngine.EventSystems;

public sealed class InputManager
{
    private Vector3 lastPosition;
    private Camera camera;

    private RectTransform nonClickableArea;

    public InputManager(Camera camera, RectTransform nonClickableArea)
    {
        this.camera = camera;
        this.nonClickableArea = nonClickableArea;
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

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
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

    public Vector3 GetSelectedMapPosition()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, Layer.Ground.ToLayerMask()))
            lastPosition = hit.point;

        return lastPosition;
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
        return !IsPointerOverUI(nonClickableArea);
    }
}

public enum MouseScrollType { None = 0, Up = 1, Down = -1}