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

        private IBuildingPreviewFactory factory;
        private Building buildingPreview; // Building that is created temporarily for preview.

        public void SetUp(IBuildingPreviewFactory factory)
        {
            this.factory = factory;
        }

        public void ToggleUIPreview(bool enable)
        {
            mouseIndicator?.gameObject.SetActive(enable);
        }

        public void ToggleBuildingPreview(bool enable, BuildingData selectedBuilding = null)
        {
            if (enable && selectedBuilding != null)
            {
                buildingPreview = factory.CreateGhost(selectedBuilding);
                return;
            }

            if (buildingPreview != null) 
            { 
                factory.DestroyGhost(buildingPreview);
                buildingPreview = null;
            }
        }

        public void SetMouseIndicatorPosition(Vector3 position)
        {
            mouseIndicator.position = position;
        }

        public void SetBuildingPreviewPosition(Vector3 position)
        {
            buildingPreview.SetPosition(position);
        }
    }
}