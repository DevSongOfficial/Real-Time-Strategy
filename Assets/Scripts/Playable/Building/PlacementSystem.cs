using CustomResourceManagement;
using UnityEngine;
using UnityEngine.UI;
using static CustomResourceManagement.Prefabs.Playable;

namespace BuildingSystem
{
    // View
    public class PlacementSystem : MonoBehaviour, IPlacementView
    {
        [SerializeField] private Transform mouseIndicator;
        [SerializeField] private Transform cellIndicator;
        private Transform buildingIndicator;
        [SerializeField] private Grid grid;

        [Header("UI(Temp)")]
        [SerializeField] private Image buttonPanel;
        [SerializeField] private Button button1;
        [SerializeField] private Button button2;

        // Events
        public event System.Action<Transform> OnPrefabSelected;

        public Vector3 CurrentCellWorldPosition { get; private set; }
        public Grid Grid => grid;

        private void Awake()
        {
            button1.onClick.AddListener(() =>
            OnPrefabSelected?.Invoke(ResourceLoader.GetResource<Transform>(Prefabs.Playable.Building.BuildingA)));

            button2.onClick.AddListener(() =>
            OnPrefabSelected?.Invoke(ResourceLoader.GetResource<Transform>(Prefabs.Playable.Building.BuildingB)));
        }

        public void ToggleButtonPanel(bool enable)
        {
            buttonPanel.gameObject.SetActive(enable);
        }

        public void TogglePreview(bool enable, Transform prefabToPreview = null)
        {
            mouseIndicator?.gameObject.SetActive(enable);
            cellIndicator?.gameObject.SetActive(enable);
            

            if(enable && prefabToPreview != null)
            {
                // todo: This prefab's collider or other components must be unactivated. (so far it doesn't have a component)
                buildingIndicator = Instantiate(prefabToPreview);
                buildingIndicator.position = CurrentCellWorldPosition;
            }
            else if (buildingIndicator != null)
            {
               Destroy(buildingIndicator.gameObject);
            }
        }

        public void SetMouseIndicatorPosition(Vector3 position)
        {
            mouseIndicator.position = position;
        }

        public void SetCellPosition(Vector3 position)
        {
            CurrentCellWorldPosition = position;
            cellIndicator.position = CurrentCellWorldPosition;
        }

        public void SetBuildingIndicatorPosition(Vector3 position)
        {
            buildingIndicator.position = position;
        }

        public void Place(Transform prefab)
        {
            var building = Instantiate(prefab);
            building.position = CurrentCellWorldPosition;
        }
    }
}