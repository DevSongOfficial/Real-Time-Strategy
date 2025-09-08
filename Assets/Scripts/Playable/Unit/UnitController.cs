using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomResourceManagement;
using BuildingSystem;

// todo: Need to seprate class
public sealed class UnitController : MonoBehaviour
{
    // Temporary for testing.
    [Header("Spawn(temp)")]
    [SerializeField, Range(0, 99)] private int numberOfUnitOnStart = 3;
    private void GenerateUnits(int numberOfUnit)
    {
        for (int i = 0; i < numberOfUnit; i++)
        {
            // Generate units.
            var prefab_Unit = ResourceLoader.GetResource<Unit>(Prefabs.Playable.Unit.Unit_1);
            var randomPosition = new Vector3(Random.Range(26, 35), 2, Random.Range(20, 36));
            var newUnit = Instantiate(prefab_Unit, randomPosition, Quaternion.identity);
            allUnits.Add(newUnit);

            // Generate health bar per unit.
            var prefab_HealthBar = ResourceLoader.GetResource<HealthTracker>(Prefabs.UI.HealthTracker);
            var newHealthBar = Instantiate(prefab_HealthBar, canvas.transform);
            newHealthBar.SetUp(mainCamera, new Target(newUnit));
        }
    }

    [Header("Scene Refs")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private PlacementSystem placementSystem; 


    // Temporary for testing.
    private List<ISelectable> allUnits;

    // Mouse drag event.
    private DragEventHandler dragEventHandler;

    // Playbles selection.
    private SelectionHandler selectionHandler;
    private List<ISelectable> selectedUnits;


    // Input
    private InputManager inputManager;

    // Game Mode (Normal / Build)
    private ModeBase currentMode;
    private ModeBase normalMode;
    private ModeBase buildMode;

    private void Awake()
    {
        selectedUnits = new List<ISelectable>();
        allUnits = new List<ISelectable>();

        dragEventHandler    = new DragEventHandler(allUnits.FilterByType<ISelectable, ITransformProvider>(), mainCamera, canvas);
        selectionHandler    = new SelectionHandler(selectedUnits, mainCamera);

        inputManager = new InputManager(mainCamera);

        
        
        normalMode = new NormalMode(selectionHandler, dragEventHandler);
        buildMode = new BuildMode(inputManager, placementSystem);


        placementSystem.TogglePreview(false); 
        SetMode(normalMode);
    }

    private void Start()
    {
        GenerateUnits(numberOfUnitOnStart);
    }

    private void Update()
    {
        currentMode?.Update();
        currentMode?.HandleInput();

        if (Input.GetKeyDown(KeyCode.B))
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