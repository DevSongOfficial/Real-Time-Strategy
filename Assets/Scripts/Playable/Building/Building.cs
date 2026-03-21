using System;
using System.Collections;
using UnityEngine;

public class Building : Playable, ITarget<BuildingData>, IBuildingStateContext, IDamageable
{
    protected HealthSystem healthSystem;
    protected GameObject selectionIndicator;
    protected EntityProfilePanel profilePanel;

    public event Action<Building> OnDestroyed;
    public event Action<Building> OnDestructionRequested;


    protected new BuildingStateMachine stateMachine => base.stateMachine as BuildingStateMachine;
    protected new BuildingBlackBoard blackBoard;

    public bool IsPreview { get; private set; }

    public bool IsUnderConstruction => (stateMachine.CurrentState is BuildingUnderConstructionState);
    public IState CurrentState => stateMachine.CurrentState;

    private Vector2Int cellPosition;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public virtual Building SetUp(EntityData data, GameObject selectionIndicator, EntityProfilePanel profilePanel, TeamContext teamContext)
    {
        this.data = data;
        this.teamContext = teamContext;

        blackBoard = new BuildingBlackBoard(data, coroutineExecutor, teamContext);
        base.stateMachine = new BuildingStateMachine(this, blackBoard);

        healthSystem = new HealthSystem(data.MaxHealth);
        healthSystem.OnDie += StartDestruction;

        this.selectionIndicator = selectionIndicator;
        this.profilePanel = profilePanel;

        return this;
    }

    public void SetCellPosition(Vector2Int cellPosition)
    {
        this.cellPosition = cellPosition;
    }

    public Vector2Int GetCellPosition() => cellPosition;

    public override void ExecuteCommand(CommandData command)
    {
        base.ExecuteCommand(command);

        if(command is DemolishCommandData demolitionCommand)
            Demolish(demolitionCommand);
    }

    #region Selection
    public override void OnSelected()
    {
        selectionIndicator?.SetActive(false);
        selectionIndicator?.SetActive(true);

        profilePanel?.RegisterEntity(this);
    }

    public override void OnDeselected()
    {
        selectionIndicator?.SetActive(false);
        profilePanel?.UnregisterEntity();
    }

    public override bool CanSelect()
    {
        return true;
    }
    #endregion

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

    #region Destruction & Demolition
    public void Demolish(DemolishCommandData command)
    {
        teamContext.ResourceBank.AddResource(ResourceType.Gold, GetData().GoldRefund);
        teamContext.ResourceBank.AddResource(ResourceType.Wood, GetData().WoodRefund);

        StartDestruction(GetData().DemolitionTime);
    }

    private void StartDestruction() => StartDestruction(0);

    private void StartDestruction(float delaySeconds)
    {
        OnDestructionRequested?.Invoke(this);
        StartCoroutine(DestructionRoutine(delaySeconds));
    }

    private IEnumerator DestructionRoutine(float delaySeconds)
    {
        if (profilePanel != null && profilePanel.CurrentEntity == this as ISelectable)
            profilePanel.UnregisterEntity();

        yield return new WaitForSeconds(delaySeconds);

        OnDestroyed?.Invoke(this);
        GameObject.Destroy(gameObject);
    }
    #endregion


    #region Transform
    public override void SetPosition(Vector3 position)
    {
        transform.position = position.WithY(GetPositionDeltaY());
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

    public void GetDamaged(int damage)
    {
        healthSystem.GetDamaged(damage);
    }
    public bool IsAlive()
    {
        return healthSystem.CurrentHealth > 0;
    }
}