using BuildingSystem;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;
    [Space]

    [SerializeField] private CameraController cameraController;
                     private InputManager inputManager;
    [Space]

    // Placement
    [SerializeField] private PlacementView placementView;
                     private PlacementPresenter placementPresenter;
    [SerializeField] private GridSystem gridSystem;
                     private BuildingFactory buildingFactory;
                     private UnitFactory unitFactory;

                     private BuildMode buildMode;

    // Team
    public Team CurrentTeam { get; private set; } = Team.None;
    public void ShiftTeam(Team team) => CurrentTeam = team;


    private void Awake()
    {
        inputManager = new InputManager(cameraController.Camera);
        cameraController.Setup(inputManager);

        buildingFactory = new BuildingFactory();
        unitFactory = new UnitFactory();

        placementView.SetUp(buildingFactory);
        placementPresenter = new PlacementPresenter(placementView, buildingFactory, gridSystem, inputManager);

        // TODO: need dedicated fsm & states.
        buildMode = new BuildMode(GameManager.GetTeamContext(CurrentTeam), inputManager, placementPresenter, useEditorPlacement: true);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buildMode.Update();
        buildMode.HandleInput();
    }

    public void AddBuilding(BuildingData data)
    {
        placementPresenter.SelectBuilding(data);
    }
}