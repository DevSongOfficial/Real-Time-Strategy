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

    // Unit container
    private UnitRegistry unitRegistry;

    // Mouse drag event.
    private DragEventHandler dragEventHandler;

    // Playbles selection.
    private SelectionHandler selectionHandler;
    private List<ISelectable> selectedUnits;

    // Input
    private InputManager inputManager;

    // Factory
    private BuildingFactory buildingFactory;
    private UnitFactory unitFactory;

    private UnitGenerator unitGenerator;
    private HealthBarGenerator healthBarGenerator;

    // Game Mode (Normal / Build)
    private ModeBase currentMode;
    private ModeBase normalMode;
    private ModeBase buildMode;

    private void Awake()
    {
        inputManager = new InputManager(mainCamera);

        unitRegistry = new UnitRegistry();
        selectedUnits = new List<ISelectable>();

        dragEventHandler    = new DragEventHandler(unitRegistry.GetTransforms(), mainCamera, canvas);
        selectionHandler    = new SelectionHandler(selectedUnits, mainCamera);

        buildingFactory     = new BuildingFactory();
        unitFactory         = new UnitFactory();

        healthBarGenerator = new HealthBarGenerator(canvas.transform, mainCamera);
        unitGenerator = new UnitGenerator(unitFactory, unitRegistry);
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