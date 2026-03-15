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
    [SerializeField] private BuildingData redTeamBuildingData;


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
    private TeamContext myTeam => teamContext_Green;
    private TeamContext teamContext_Green;
    private TeamContext teamContext_Red;
    private TeamContext teamContext_Blue;
    private TeamContext teamContext_Neutral;
    
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

    private void InitializeTeams()
    {
        // Green (my team)
        {
            var team = Team.Green;
            var resourceBank = new ResourceBank(resourceView);
            var capacitySlots = new UnitCapacitySlots(maxUnitCapacityOnStart, resourceView);
            teamContext_Green = new TeamContext(team, resourceBank, capacitySlots);
        }
        // Red 
        {
            var team = Team.Red;
            var resourceBank = new ResourceBank(resourceView);
            var capacitySlots = new UnitCapacitySlots(maxUnitCapacityOnStart, resourceView);
            teamContext_Red = new TeamContext(team, resourceBank, capacitySlots);
        }
        // Blue 
        {
            var team = Team.Blue;
            var resourceBank = new ResourceBank(resourceView);
            var capacitySlots = new UnitCapacitySlots(maxUnitCapacityOnStart, resourceView);
            teamContext_Blue = new TeamContext(team, resourceBank, capacitySlots);
        }
        // Neutral 
        {
            var team = Team.None;
            var resourceBank = new ResourceBank(resourceView);
            var capacitySlots = new UnitCapacitySlots(maxUnitCapacityOnStart, resourceView);
            teamContext_Neutral = new TeamContext(team, resourceBank, capacitySlots);
        }
    }

    private void Awake()
    {
        InitializeTeams();

        inputManager = new InputManager(cameraController.Camera, nonClickableAreas);
        cameraController.Setup(inputManager);

        entityRegistry              = new EntityRegistry();
        selectionIndicatorFactory   = new SelectionIndicatorFactory();
        healthBarGenerator          = new HealthBarGenerator(healthBarContainer, cameraController);
        
        rallyPointSetter = new RallyPointSetter(rallyPointIndicator, mouseIndicator_World);
        moveMarkerFactory    = new MoveMarkerFactory();
        buildingFactory      = new BuildingFactory(() => unitGenerator, selectionHandler, selectionIndicatorFactory, profilePanel, rallyPointSetter);

        dragEventHandler    = new DragEventHandler(entityRegistry.GetTransformsOfUnits(), cameraController.Camera, canvas, inputManager);
        selectionHandler    = new SelectionHandler(entityRegistry.GetSelectedEntities(), cameraController.Camera, commandPanel, moveMarkerFactory, teamContext_Green);

        // Placement
        placementView.SetUp(buildingFactory);
        placementView.ToggleUIPreview(false);
        gridSystem          = new GridSystem(grid, quadMesh);
        placementPresenter  = new PlacementPresenter(placementView, commandPanel, buildingFactory, gridSystem, inputManager);
        placementPresenter.OnPlacementCanceled += (Vector3 finishedPosition) => stateMachine.RequestTransition(Mode.Normal);
        placementPresenter.OnPlacementRequested += (ITarget requestedBuilding) => stateMachine.RequestTransition(Mode.Normal);

        unitFactory                     = new UnitFactory(selectionHandler, selectionIndicatorFactory, placementPresenter, profilePanel);
        unitGenerator                   = new UnitGenerator(unitFactory, entityRegistry, myTeam.CapacitySlots);
        unitGenerator.OnUnitGenerated   += healthBarGenerator.GenerateAndSetTargetUnit;
        
        unitGenerator.OnUnitDeathRequested                      += selectionHandler.DeselectEntity;
        placementPresenter.OnBuildingDeconstructionRequested    += selectionHandler.DeselectEntity; 
        unitGenerator.OnUnitDestroyed                           += healthBarGenerator.UnsetTargetUnit;
        

        // FSM
        var normalMode              = new NormalMode(inputManager, selectionHandler, dragEventHandler);
        var buildMode               = new BuildMode(myTeam, inputManager, placementPresenter);
        var rallyPointSetMode       = new SetRallyPointMode(inputManager, rallyPointSetter);
        var selectTargetMode        = new SelectTargetMode(inputManager, selectionHandler, mouseIndicator_World);
        
        stateMachine                = new PlayerStateMachine(normalMode, buildMode, rallyPointSetMode, selectTargetMode);
        commandPanel.Setup(stateMachine, teamContext_Green);
        stateMachine.SetMode(normalMode);


        selectionHandler.OnSelectEntity += OnEntitySelected;
        selectionHandler.OnDeselectEntity += OnEntityDeselected;
        commandPanel.OnBuildingConstructionButtonClicked += (BuildingData data) => placementPresenter.SelectBuilding(data);
    }

    private void Start()
    {
        unitGenerator.GenerateWithRandomPosition(unitData, myTeam, numberOfUnitOnStart);
        unitGenerator.GenerateWithRandomPosition(unitData, teamContext_Red, numberOfUnitOnStart);

        var temp_HQPosition = new Vector3(30, 0.5f, 30);
        if (placementPresenter.TryPlace(headquartersData, myTeam, new Vector3(30, 0.5f, 30), out Building HQ) == PlacementResult.Success)
        {
            myTeam.SetHeadQuarters(HQ as HeadQuarters);
        }

        var temp_minePosition = new Vector3(34, 0.5f, 34);
        placementPresenter.TryPlace(goldMineData, teamContext_Neutral,  temp_minePosition, out Building mine);
        placementPresenter.TryPlace(redTeamBuildingData, teamContext_Red, temp_minePosition + Vector3.back * 4, out Building building);
        
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