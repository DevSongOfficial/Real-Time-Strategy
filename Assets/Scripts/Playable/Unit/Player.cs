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
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private PlacementView placementView; 

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

    private UnitGenerator unitGenerator;
    private HealthBarGenerator healthBarGenerator;

    // Game Mode (Normal / Build)
    private ModeBase currentMode;
    private ModeBase normalMode;
    private ModeBase buildMode;

    private void Awake()
    {
        inputManager = new InputManager(mainCamera);

        entityRegistry = new EntityRegistry();

        dragEventHandler    = new DragEventHandler(entityRegistry.GetTransformsOfUnits(), mainCamera, canvas, inputManager);
        selectionHandler    = new SelectionHandler(entityRegistry.GetSelectedEntities(), mainCamera);

        selectionIndicatorFactory   = new SelectionIndicatorFactory();
        buildingFactory             = new BuildingFactory(selectionIndicatorFactory);
        unitFactory                 = new UnitFactory(selectionIndicatorFactory);

        healthBarGenerator = new HealthBarGenerator(canvas.transform, mainCamera);
        unitGenerator = new UnitGenerator(unitFactory, entityRegistry);
        unitGenerator.OnUnitGenerated += healthBarGenerator.GenerateAndSetTargetUnit;

        placementView.SetUp(buildingFactory);
        placementView.ToggleUIPreview(false); 

        normalMode = new NormalMode(inputManager, selectionHandler, dragEventHandler);
        buildMode = new BuildMode(inputManager, placementView, buildingFactory);
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