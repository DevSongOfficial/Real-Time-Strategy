using CustomResourceManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BuildingSystem
{
    // UI viewer for placement system
    public sealed class PlacementView : MonoBehaviour, IPlacementView
    {
        [SerializeField] private Transform mouseIndicator;


        [Header("UI(Temp)")]
        [SerializeField] private Image buttonPanel;
        [SerializeField] private EntityCreateButton button1;
        [SerializeField] private EntityCreateButton button2;
        [SerializeField] private EntityCreateButton button3;

        // Events
        public event System.Action<BuildingData> OnBuildingSelected;

        private IBuildingPreviewFactory factory;
        private Building buildingPreview; // Building that is created temporarily for preview.

        private Vector3 currentCellWorldPosition;

        public void SetUp(IBuildingPreviewFactory factory)
        {
            this.factory = factory;
        }

        private void Awake()
        {
            button1.OnButtonClicked += (EntityData data) => OnBuildingSelected?.Invoke((BuildingData)data);
            button2.OnButtonClicked += (EntityData data) => OnBuildingSelected?.Invoke((BuildingData)data);
            button3.OnButtonClicked += (EntityData data) => OnBuildingSelected?.Invoke((BarracksData)data);
        }

        public void ToggleButtonPanel(bool enable)
        {
            buttonPanel.gameObject.SetActive(enable);
        }

        public void ToggleUIPreview(bool enable)
        {
            mouseIndicator?.gameObject.SetActive(enable);
        }

        public void ToggleBuildingPreview(bool enable, BuildingData selectedBuilding = null)
        {
            if (enable && selectedBuilding != null)
                buildingPreview = factory.CreateGhost(selectedBuilding);
            else
            {
                factory.DestroyGhost(buildingPreview);
                buildingPreview = null;
            }
        }

        public void SetMouseIndicatorPosition(Vector3 position)
        {
            mouseIndicator.position = position;
        }

        public void SetCellPosition(Vector3 position)
        {
            currentCellWorldPosition = position;
        }

        public void SetBuildingPreviewPosition(Vector3 position)
        {
            buildingPreview.SetPosition(position);
        }
    }
}