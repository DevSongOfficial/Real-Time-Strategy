using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Building : Playable, ITarget<BuildingData>, IBuildingStateContext
{
    [SerializeField] protected CoroutineExecutor coroutineExecutor;
    [SerializeField] private new Collider collider;
    protected HealthSystem healthSystem;
    protected GameObject selectionIndicator;
    protected EntityProfilePanel profilePanel;

    protected new BuildingStateMachine stateMachine => base.stateMachine as BuildingStateMachine;
    protected new BuildingBlackBoard blackBoard;

    public bool IsPreview { get; private set; }

    public bool IsUnderConstruction => (stateMachine.CurrentState is BuildingUnderConstructionState);
    public IState CurrentState => stateMachine.CurrentState;


    // TODO: I think it's better to put this variable in EntityData and write on my own. (not getting it through collider)
    public float PositionDeltaY { get; private set; }

    // Better not use Start().
    protected virtual void Awake()
    {
        if (coroutineExecutor != null)
            coroutineExecutor = GetComponent<CoroutineExecutor>();

        if (collider == null)
            collider = GetComponentInChildren<Collider>();

        PositionDeltaY = collider.bounds.extents.y;
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public virtual Building SetUp(EntityData data, GameObject selectionIndicator, EntityProfilePanel profilePanel, Team team)
    {
        this.data = data;
        this.team = team;

        blackBoard = new BuildingBlackBoard(data, coroutineExecutor, team);
        base.stateMachine = new BuildingStateMachine(this, blackBoard);

        healthSystem = new HealthSystem(data.MaxHealth);

        this.selectionIndicator = selectionIndicator;
        this.profilePanel = profilePanel;

        return this;
    }

    public override void OnSelected()
    {
        selectionIndicator.SetActive(false);
        selectionIndicator.SetActive(true);

        profilePanel.RegisterEntity(this);
    }

    public override void OnDeselected()
    {
        selectionIndicator.SetActive(false);
        profilePanel.UnregisterEntity();
    }

    #region State & Progress
    public virtual float GetProgressRate()
    {
        return Mathf.Clamp(blackBoard.progressRate, 0, 1);
    }
    public virtual string GetProgressLabelName()
    {
        if(stateMachine.CurrentState is BuildingUnderConstructionState)
            return "Being constructed...";

        return string.Empty;
    }
    #endregion

    #region Construction
    // TODO: refactor 
    public void StartConstruction(Action unitActionOnConsturctionFinished)
    {
        stateMachine.ConstructionState.ResumeConstruction(unitActionOnConsturctionFinished);
    }
    // TODO: refactor 
    public void PauseConstruction()
    {
        stateMachine.ConstructionState.PauseConstruction();
    }
    #endregion

    #region Transform
    public override void SetPosition(Vector3 position)
    {
        transform.position = position.WithY(PositionDeltaY);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    #endregion
    
    public virtual IUnitGenerator GetUnitGenerator()
    {
        return null;
    }

    public IHealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    public new BuildingData GetData()
    {
        return data as BuildingData;
    }

    EntityData ITarget.GetData() => GetData();

    // This is basically for ghost building.
    public virtual void SetToPreview()
    {
        IsPreview = true;
        enabled = false;
        gameObject.SetLayer(Layer.IgnoreCollision);
    }

    protected enum SpawnPositionType { TopLeft, TopRight, BottomLeft, BottomRight, Random }
    protected Vector3 CalculateSpawnPosition(SpawnPositionType type)
    {
        int xSign = 0; 
        int ySign = 0;

        switch (type)
        {
            case SpawnPositionType.TopLeft:
                xSign = -1;
                ySign = 1;
                break;
            case SpawnPositionType.TopRight:
                xSign = 1;
                ySign = 1;
                break;
            case SpawnPositionType.BottomLeft:
                xSign = -1;
                ySign = -1;
                break;
            case SpawnPositionType.BottomRight:
                xSign = 1;
                ySign = -1;
                break;
            case SpawnPositionType.Random:
                xSign = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
                ySign = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
                break;
        }

        int x = Mathf.CeilToInt((GetData().CellSize.x / 2f)) * xSign;
        int y = Mathf.CeilToInt((GetData().CellSize.y / 2f)) * ySign;

        return transform.position + new Vector3(x, 0, y);
    }
}