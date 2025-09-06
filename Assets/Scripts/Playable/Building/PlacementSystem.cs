using CustomResourceManagement;
using UnityEngine;

namespace BuildingSystem
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private Transform mouseIndicator;
        [SerializeField] private Transform cellIndicator;

        [SerializeField] private Grid grid;

        public void EnablePreview(bool enable)
        {
            mouseIndicator?.gameObject.SetActive(enable);
            cellIndicator?.gameObject.SetActive(enable);
        }

        public void UpdatePreview(Vector3 mousePosition)
        {
            // Set mouse indicator position.
            mouseIndicator.position = mousePosition;

            // Set cell indicator position.
            var gridPosition = grid.WorldToCell(mousePosition);
            cellIndicator.position = grid.CellToWorld(gridPosition).WithY(0);
        }

        public void PlaceBuilding()
        {
            var prefab = ResourceLoader.GetResource<Transform>(Prefabs.Playable.Building.BuildingA);
            var building = Instantiate(prefab);
            building.position = cellIndicator.position;
        }
    }
}