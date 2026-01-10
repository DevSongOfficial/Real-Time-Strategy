using System.Collections.Generic;
using UnityEngine;
using CustomResourceManagement;
using BuildingSystem;

public sealed class Player : MonoBehaviour
{
    // Temporary for testing.
    [Header("Spawn(temp)")]
    [SerializeField, Range(0, 99)] private int numberOfUnitOnStart = 3;
    [SerializeField] private EntityData unitData;
    

    [Header("Scene Refs")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Canvas canvas;
    [SerializeField] private PlacementView placementView;
    [SerializeField] private Grid grid;
    [SerializeField] private Mesh quadMesh;

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
    private MoveMakerFactory moveMarkerFactory;

    // Grid System for building placement.
    private GridSystem gridSystem;

    private UnitGenerator unitGenerator;
    private HealthBarGenerator healthBarGenerator;

    // Game Mode (Normal / Build)
    private ModeBase currentMode;
    private ModeBase normalMode;
    private ModeBase buildMode;

    private void Awake()
    {
        inputManager = new InputManager(cameraController.Camera);
        cameraController.Setup(inputManager);

        entityRegistry = new EntityRegistry();

        selectionIndicatorFactory   = new SelectionIndicatorFactory();
        moveMarkerFactory            = new MoveMakerFactory();
        buildingFactory             = new BuildingFactory(selectionIndicatorFactory);
        unitFactory                 = new UnitFactory(selectionIndicatorFactory);

        dragEventHandler    = new DragEventHandler(entityRegistry.GetTransformsOfUnits(), cameraController.Camera, canvas, inputManager);
        selectionHandler    = new SelectionHandler(entityRegistry.GetSelectedEntities(), cameraController.Camera, moveMarkerFactory);

        healthBarGenerator = new HealthBarGenerator(canvas.transform, cameraController.Camera);
        unitGenerator = new UnitGenerator(unitFactory, entityRegistry);
        unitGenerator.OnUnitGenerated += healthBarGenerator.GenerateAndSetTargetUnit;

        placementView.SetUp(buildingFactory);
        placementView.ToggleUIPreview(false);

        gridSystem = new GridSystem(grid, quadMesh);

        normalMode = new NormalMode(inputManager, selectionHandler, dragEventHandler);
        buildMode = new BuildMode(inputManager, placementView, buildingFactory, gridSystem);
        SetMode(normalMode);
    }

    private void Start()
    {
        unitGenerator.Generate(unitData, numberOfUnitOnStart);
    }

    private void Update()
    {
        currentMode?.Update();
        currentMode?.HandleInput();

        if (inputManager.GetKeyDown(KeyCode.B))
        {
            SetMode(currentMode == normalMode ? buildMode : normalMode);
        }
    }

    private void SetMode(ModeBase newMode)
    {
        currentMode?.Exit();
        currentMode = newMode;
        currentMode.Enter();
    }
}