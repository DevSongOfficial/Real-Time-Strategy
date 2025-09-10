using UnityEngine;
using UnityEngine.EventSystems;

namespace BuildingSystem
{
    public sealed class InputManager
    {
        private Vector3 lastPosition;
        private Camera camera;

        public InputManager(Camera camera)
        {
            this.camera = camera;
        }

        public bool GetMouseButton(int button)
        {
            return Input.GetMouseButton(button);
        }

        public bool GetMouseButtonDown(int button)
        {
            return Input.GetMouseButtonDown(button);
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode);
        }

        public Vector3 GetSelectedMapPosition()
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, Layer.Ground.ToLayerMask()))
                lastPosition = hitInfo.point;

            return lastPosition;
        }

        public bool IsPointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}