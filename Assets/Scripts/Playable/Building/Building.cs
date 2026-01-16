using System;
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

    public bool CanExecuteCommand => (stateMachine.CurrentState != (stateMachine).ConstructionState);


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
    public float GetProgressRate()
    {
        return Mathf.Clamp(blackBoard.progressRate, 0, 1);
    }
    public string GetProgressLabelName()
    {
        switch (stateMachine.CurrentState)
        {
            case BuildingUnderConstructionState:
                return "Being constructed...";
            case BuildingUnitTrainState:
                return "Traning " + blackBoard.unitGenerationInfo.unitData.DisplayName + "...";
        }

        return string.Empty;
    }

    public Sprite GetTraningUnitSprite()
    {
        if(stateMachine.CurrentState is BuildingUnitTrainState)
            return blackBoard.unitGenerationInfo.unitData.ProfileSprite;

        return null;
    }
    #endregion

    #region Construction
    // TODO: refactor 
    public void StartConstruction(Action unitActionOnConsturctionFinished)
    {
        stateMachine.ConstructionState.OnFinished += unitActionOnConsturctionFinished;
        stateMachine.ChangeState<BuildingUnderConstructionState>();
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

    public BuildingData GetData()
    {
        return (BuildingData)data;
    }

    EntityData ITarget.GetData() => GetData();

    // This is basically for ghost building.
    public virtual void MakeRenderOnly()
    {
        enabled = false;
        gameObject.SetLayer(Layer.IgnoreCollision);
    }
}