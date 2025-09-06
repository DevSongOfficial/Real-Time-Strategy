using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] [Range(0, 300)] float edgeSize      = 50f;
    [SerializeField] [Range(0, 10)]  float movementSpeed = 3f;

    private void Update()
    {
        HandleCameraMovement();
    }

    private void HandleCameraMovement()
    {
        Vector3 direction = Vector3.zero;

        if (Input.mousePosition.x > Screen.width - edgeSize)
        {
            direction += transform.right;
        }
        if (Input.mousePosition.x < edgeSize)
        {
            direction -= transform.right;
        }
        if (Input.mousePosition.y > Screen.height - edgeSize)
        {
            direction += transform.forward;
        }
        if (Input.mousePosition.y < edgeSize)
        {
            direction -= transform.forward;
        }

        if (direction != Vector3.zero)
        {
            direction.Normalize();
            transform.position += direction * movementSpeed * Time.deltaTime;
        }
    }
}