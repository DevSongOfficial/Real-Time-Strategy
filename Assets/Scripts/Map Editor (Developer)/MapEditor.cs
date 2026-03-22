using BuildingSystem;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;
    [SerializeField] private MapSaveLoad SaveLoad;
    [SerializeField] private EntityDataDB EntityDataDB;
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

    // StateMachine & Modes
    private EditorStateMachine stateMachine;
    private SelectMode selectMode;
    private BuildMode buildMode;

    private List<Building> placedBuildings;

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


        buildMode = new BuildMode(GameManager.GetTeamContext(CurrentTeam), inputManager, placementPresenter, useEditorPlacement: true);
        buildMode.OnBuildingPlaced += RegisterBuilding;

        selectMode = new SelectMode(inputManager, placementPresenter, cameraController.Camera);
        selectMode.OnBuildingSelected += UnregisterBuilding;
        selectMode.OnBuildingSelected += (building) => AddBuilding(building.GetData());

        stateMachine = new EditorStateMachine(selectMode, buildMode);

        placedBuildings = new List<Building>();
    }

    void Start()
    {
        stateMachine.SetMode(selectMode);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        stateMachine.HandleInput();
    }

    private void RegisterBuilding(ITarget obj)
    {
        var building = obj as Building;
        placedBuildings.Add(building);

        stateMachine.SetMode(selectMode);
    }

    private void UnregisterBuilding(Building building)
    {
        placedBuildings.Remove(building);
    }

    // Buttons' Event Actions
    // Connected through UnitAction 
    #region For Buttons
    public void AddBuilding(BuildingData data)
    {
        stateMachine.SetMode(buildMode);

        placementPresenter.SelectBuilding(data);
    }

    public void SaveMapData()
    {
        var mapData = SaveLoad.CreateMapData(placedBuildings, null);
        SaveLoad.SaveMapData(mapData);
    }

    public void LoadMapData()
    {
        var mapData = SaveLoad.LoadMapDataFromFile();
        if (mapData == null) return;

        foreach (var record in mapData.buildings)
        {
            var data        = EntityDataDB.GetBuildinigData(record.id);
            var teamContext = GameManager.GetTeamContext((Team)record.teamId);
            var position    = new Vector3(record.cellX, 0.5f, record.cellY);

            placementPresenter.TryPlace(data, teamContext , position , out Building placed, spendResources: false);
        }
    }
    #endregion
}