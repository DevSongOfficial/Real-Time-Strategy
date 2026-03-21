using UnityEngine;

public class MapEditor : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CameraController cameraController;

    // Team
    public Team CurrentTeam { get; private set; } = Team.None;
    public void ShiftTeam(Team team)
    {
        CurrentTeam = team;
    }

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
        if (inputManager.GetMouseButtonDown(0))
        {
            TeamContext teamContext = gameManager.GetTeamContext(CurrentTeam);
            //presenter.TryPlaceForEditor(teamContext);
        }
    }
}