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
    [SerializeField] private GameManager GameManager;
    [Header("UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private EntityProfilePanel profilePanel;
    [SerializeField] private CommandPanel commandPanel;
    [SerializeField] private Transform healthBarContainer;
    [SerializeField] private RectTransform nonClickableAreas;
    [SerializeField] private PlacementView placementView;
    
    [Space]
    [SerializeField] private CameraController cameraController;

    [Header("Grid System")]
    [SerializeField] private GridSystem gridSystem;
    [Space]
    // GameObject following mouse cursor position
    [SerializeField] private Transform mouseIndicator_World;

    // Team
    [SerializeField] private Team playerTeam = Team.Green;
    private TeamContext myTeam => GameManager.GetTeamContext(playerTeam);
    
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
        buildingFactory      = new BuildingFactory(selectionIndicatorFactory, () => unitGenerator, profilePanel, rallyPointSetter);

        dragEventHandler    = new DragEventHandler(entityRegistry.GetTransformsOfUnits(), cameraController.Camera, canvas, inputManager);
        selectionHandler    = new SelectionHandler(entityRegistry.GetSelectedEntities(), cameraController.Camera, commandPanel, moveMarkerFactory, myTeam);

        // Placement
        placementView.SetUp(buildingFactory);
        placementView.ToggleUIPreview(false);
        placementPresenter  = new PlacementPresenter(placementView, buildingFactory, gridSystem, inputManager);
        placementPresenter.OnPlacementCanceled += (Vector3 finishedPosition) => stateMachine.RequestTransition(Mode.Normal);
        placementPresenter.OnPlacementRequested += (ITarget requestedBuilding) => stateMachine.RequestTransition(Mode.Normal);

        unitFactory                     = new UnitFactory(selectionIndicatorFactory, placementPresenter, profilePanel, gridSystem);
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
        commandPanel.Setup(stateMachine, myTeam);
        stateMachine.SetMode(normalMode);


        selectionHandler.OnSelectEntity += OnEntitySelected;
        selectionHandler.OnDeselectEntity += OnEntityDeselected;
        commandPanel.OnBuildingConstructionButtonClicked += (BuildingData data) => placementPresenter.SelectBuilding(data);
    }

    private void Start()
    {
        unitGenerator.GenerateWithRandomPosition(unitData, myTeam, numberOfUnitOnStart);
        //unitGenerator.GenerateWithRandomPosition(unitData, teamContext_Red, numberOfUnitOnStart);

        var temp_HQPosition = new Vector3(30, 0.5f, 30);
        if (placementPresenter.TryPlace(headquartersData, myTeam, new Vector3(30, 0.5f, 30), out Building HQ) == PlacementResult.Success)
        {
            myTeam.SetHeadQuarters(HQ as HeadQuarters);
        }

        var temp_minePosition = new Vector3(34, 0.5f, 34);
        placementPresenter.TryPlace(goldMineData, GameManager.GetTeamContext(Team.None),  temp_minePosition, out Building mine);
        placementPresenter.TryPlace(redTeamBuildingData, GameManager.GetTeamContext(Team.Red), temp_minePosition + Vector3.back * 4, out Building building);
        
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