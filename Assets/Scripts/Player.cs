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
    [SerializeField] private EntityData unitData;
    

    [Header("Scene Refs")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private EntityProfilePanel profilePanel;
    [SerializeField] private CommandPanel commandPanel;
    [SerializeField] private Transform healthBarContainer;
    [SerializeField] private RectTransform nonClickableArea;
    [SerializeField] private PlacementView placementView;
    [Space]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Grid grid;
    [SerializeField] private Mesh quadMesh;
    [SerializeField] private Transform mouseIndicator_World;

    // Team
    public static Team Team = Team.Green;

    // Entity Regsitry includes diffrent type of entity containers.
    private EntityRegistry entityRegistry;

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
        inputManager = new InputManager(cameraController.Camera, nonClickableArea);
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


        unitFactory                     = new UnitFactory(selectionHandler, selectionIndicatorFactory, placementPresenter, profilePanel);
        unitGenerator                   = new UnitGenerator(unitFactory, entityRegistry);
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
        unitGenerator.RandomGenerate(unitData, numberOfUnitOnStart);
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