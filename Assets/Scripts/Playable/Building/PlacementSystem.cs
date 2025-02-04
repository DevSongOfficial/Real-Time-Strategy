using CustomResourceManagement;
using UnityEditor;
using UnityEngine;

namespace BuildingSystem
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private Transform mouseIndicator;
        [SerializeField] private Transform cellIndicator;

        private InputManager inputManager;

        [SerializeField] private Grid grid;

        private void Awake()
        {
            inputManager = new InputManager(Camera.main);
        }

        private void Update()
        {
            var mousePosition = inputManager.GetSelectedMapPosition();

            // Set cell indicator position.
            var gridPosition = grid.WorldToCell(mousePosition);
            cellIndicator.position = grid.CellToWorld(gridPosition).WithY(0);

            // Set mouse indicator position.
            mouseIndicator.position = mousePosition;

            HandlePlacement();
        }


        private void HandlePlacement()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
                PlaceBuilding();
        }
        private void PlaceBuilding()
        {
            if (inputManager.IsPointerOverUI()) return;

            var prefab = ResourceLoader.GetResource<Transform>(Prefabs.Playable.Building.BuildingA);
            var building = Instantiate(prefab);
            building.position = cellIndicator.position;
        }
    }
}