using BuildingSystem;
using CustomResourceManagement;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public sealed class Player : MonoBehaviour
{
    // Temporary for testing.
    [Header("Spawn(temp)")]
    [SerializeField, Range(0, 99)] private int numberOfUnitOnStart = 3;
    [SerializeField] private UnitData unitData;
    [SerializeField] private ResourceProviderData goldMineData;


    [Header("Scene Refs")]
    [SerializeField] private BarracksData headquartersData;
    [Space]
    [SerializeField] private Canvas canvas;
    [SerializeField] private EntityProfilePanel profilePanel;
    [SerializeField] private CommandPanel commandPanel;
    [SerializeField] private Transform healthBarContainer;
    [SerializeField] private RectTransform nonClickableAreas;
    [SerializeField] private PlacementView placementView;
    [Header("Resource & Capacity")]
    [SerializeField] private ResourceView resourceView;
    [SerializeField] private int maxUnitCapacityOnStart;
    [Space]
    [SerializeField] private CameraController cameraController;
    
    [Header("Grid System")]
    [SerializeField] private Grid grid;
    [SerializeField] private Mesh quadMesh;

    // GameObject following mouse cursor position
    [SerializeField] private Transform mouseIndicator_World;

    // Team
    public readonly static Team Team = Team.Green;
    public static HeadQuarters HQ { get;  private set; }

    // Entity Regsitry includes diffrent type of entity containers.
    private EntityRegistry entityRegistry;

    // Manages resources such as Gold, Wood.
    public static ResourceBank ResourceBank => resourceBank; // TEMP: Each team must have a single resourcebank
    private static ResourceBank resourceBank;
    private UnitCapacitySlots capacitySlots;

    // Mouse drag event.
    private DragEventHandler dragEventHandler;

    // Playbles selection.
    private SelectionHandler selectionHandler;

    // Input
    private InputManager inputManager;

    // Factory
    private BuildingFactory buildingFactory;
    private UnitFactory unitFactory;
    private SelectionIndicatorFactory selectionIndicatorFactory;
    private MoveMarkerFactory moveMarkerFactory;

    private PlacementPresenter  placementPresenter;
    
    // Grid System for building placement.
    private GridSystem gridSystem;

    private UnitGenerator unitGenerator;
    private HealthBarGenerator healthBarGenerator;

    // FSM 
    private PlayerStateMachine stateMachine;

    // Spawning position controller for unit-generatable buildings.
    private RallyPointSetter rallyPointSetter;
    [SerializeField] private Transform rallyPointIndicator;

    private void Awake()
    {
        inputManager = new InputManager(cameraController.Camera, nonClickableAreas);
        cameraController.Setup(inputManager);

        entityRegistry              = new EntityRegistry();
        selectionIndicatorFactory   = new SelectionIndicatorFactory();
        healthBarGenerator          = new HealthBarGenerator(healthBarContainer, cameraController);
        
        rallyPointSetter = new RallyPointSetter(rallyPointIndicator, mouseIndicator_World);
        moveMarkerFactory    = new MoveMarkerFactory();
        buildingFactory      = new BuildingFactory(() => unitGenerator, selectionHandler, selectionIndicatorFactory, profilePanel, rallyPointSetter);

        dragEventHandler    = new DragEventHandler(entityRegistry.GetTransformsOfUnits(), cameraController.Camera, canvas, inputManager);
        selectionHandler    = new SelectionHandler(entityRegistry.GetSelectedEntities(), cameraController.Camera, commandPanel, moveMarkerFactory);

        placementView.SetUp(buildingFactory);
        placementView.ToggleUIPreview(false);
        gridSystem          = new GridSystem(grid, quadMesh);
        placementPresenter  = new PlacementPresenter(placementView, commandPanel, buildingFactory, gridSystem, inputManager);
        placementPresenter.OnPlacementCanceled += (Vector3 finishedPosition) => stateMachine.RequestTransition(Mode.Normal);
        placementPresenter.OnPlacementRequested += (ITarget requestedBuilding) => stateMachine.RequestTransition(Mode.Normal);

        resourceBank = new ResourceBank(resourceView);
        capacitySlots = new UnitCapacitySlots(maxUnitCapacityOnStart, resourceView);

        unitFactory                     = new UnitFactory(selectionHandler, selectionIndicatorFactory, placementPresenter, profilePanel);
        unitGenerator                   = new UnitGenerator(unitFactory, entityRegistry, capacitySlots);
        unitGenerator.OnUnitGenerated   += healthBarGenerator.GenerateAndSetTargetUnit;

        // FSM
        var normalMode              = new NormalMode(inputManager, selectionHandler, dragEventHandler);
        var buildMode               = new BuildMode(inputManager, placementPresenter);
        var rallyPointSetMode       = new SetRallyPointMode(inputManager, rallyPointSetter);
        var selectTargetMode        = new SelectTargetMode(inputManager, selectionHandler, mouseIndicator_World);
        
        stateMachine                = new PlayerStateMachine(normalMode, buildMode, rallyPointSetMode, selectTargetMode);
        commandPanel.Setup(stateMachine);
        stateMachine.SetMode(normalMode);


        selectionHandler.OnSelectEntity += OnEntitySelected;
        selectionHandler.OnDeselectEntity += OnEntityDeselected;
        commandPanel.OnBuildingConstructionButtonClicked += (BuildingData data) => placementPresenter.SelectBuilding(data);
    }

    private void Start()
    {
        unitGenerator.GenerateWithRandomPosition(unitData, Team.Green, numberOfUnitOnStart);
        unitGenerator.GenerateWithRandomPosition(unitData, Team.Red, numberOfUnitOnStart);

        var temp_HQPosition = new Vector3(30, 0.5f, 30);
        if (placementPresenter.TryPlace(headquartersData, new Vector3(30, 0.5f, 30), Team, out Building HQ))
        {
            Player.HQ = HQ as HeadQuarters;
        }

        var temp_minePosition = new Vector3(34, 0.5f, 34);
        placementPresenter.TryPlace(goldMineData, temp_minePosition, Team, out Building mine);
    }

    private void Update()
    {
        stateMachine.Update();
        stateMachine.HandleInput();
    }

    private void LateUpdate()
    {
        UpdateMouseIndicatorPosition();
    }

    private void OnEntitySelected(ISelectable entity)
    {
        entity.OnSelected();
        commandPanel.OnEntitySelected(entity);
    }

    private void OnEntityDeselected(ISelectable entity)
    {
        entity.OnDeselected();
        commandPanel.DisableAllButtons();
        rallyPointSetter.HideRallyPositionIndicator();
    }

    // Mouse Indicator
    private void UpdateMouseIndicatorPosition()
    {
        var mousePosition = inputManager.GetMousePositionOnCanvas();
        Ray ray = cameraController.Camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Layer.Ground.ToLayerMask()))
            mouseIndicator_World.position = hit.point;
    }
}