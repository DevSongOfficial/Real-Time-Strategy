using CustomResourceManagement;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem
{
    // View
    public class PlacementSystem : MonoBehaviour, IPlacementView
    {
        [SerializeField] private Transform mouseIndicator;
        [SerializeField] private Transform cellIndicator;
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

        public void TogglePreview(bool enable)
        {
            mouseIndicator?.gameObject.SetActive(enable);
            cellIndicator?.gameObject.SetActive(enable);
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

        public void Place(Transform prefab)
        {
            var building = Instantiate(prefab);
            building.position = CurrentCellWorldPosition;
        }
    }
}