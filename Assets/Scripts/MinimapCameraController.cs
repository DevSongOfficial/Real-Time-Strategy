using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    [SerializeField] private CameraController mainCameraController;
    [SerializeField] private int height = 10;

    private void LateUpdate()
    {
        Ray ray = new Ray(mainCameraController.transform.position, mainCameraController.Camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Layer.Ground.ToLayerMask()))
        {
            transform.position = hit.point.WithY(height);
        }
    }
}
