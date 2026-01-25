using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    [SerializeField] private CameraController mainCameraController;
    [SerializeField] private int height = 10;

    private void LateUpdate()
    {
        Ray ray = new Ray(mainCameraController.transform.position, mainCameraController.Camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.yellow); // Ãß°¡

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Layer.Ground.ToLayerMask()))
        {
            Debug.Log(hit.point);
            transform.position = hit.point.WithY(height);
        }
    }
}
