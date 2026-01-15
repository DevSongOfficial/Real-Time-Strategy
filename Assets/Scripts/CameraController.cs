using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Camera Camera { get; private set; }


    [Header("Camera Move")]
    [SerializeField] [Range(0, 300)] float edgeSize      = 50f;
    [SerializeField] [Range(0, 10)]  float movementSpeed = 3f;

    [Header("Camera Zoom In & Out")]
    [SerializeField] private float zoomSpeed = 5f;
    [field: SerializeField] public float ZoomLevel = 4f;
    [Space]
    [Tooltip("Projection Size")]
    [SerializeField] private float defaultSize = 4f;
    [SerializeField] private float minSize = 2f;
    [SerializeField] private float maxSize = 7f;

    private InputManager inputManager;

    private void Awake()
    {
        Camera.orthographicSize = defaultSize;
    }

    public void Setup(InputManager inputManager)
    {
        this.inputManager = inputManager;
    }

    private void LateUpdate()
    {
        HandleCameraMovement();
        HandleZoom();
    }

    private void HandleZoom()
    {
        if (!inputManager.IsMouseScrolled(out MouseScrollType type)) return;

        var size = Camera.orthographicSize + zoomSpeed * -(int)type * Time.deltaTime;
        size = Mathf.Clamp(size, minSize, maxSize);

        Camera.orthographicSize = size;
    }

    private void HandleZoom_Perspective()
    {
        if (!inputManager.IsMouseScrolled(out MouseScrollType type)) return;

        var direction = Camera.transform.forward * (int)type;
        var positionToMove = transform.position + direction * zoomSpeed * Time.deltaTime;

        // if(positionToMove.z > maxZ && type == MouseScrollType.Up) return;
        // if (positionToMove.z < minZ && type == MouseScrollType.Down) return;

        transform.position = positionToMove;
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