using UnityEngine;

public class MapEditor : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;

    private InputManager inputManager;

    private void Awake()
    {
        inputManager = new InputManager(cameraController.Camera);
        cameraController.Setup(inputManager);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}