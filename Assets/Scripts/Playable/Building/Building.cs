using System;
using Unity.VisualScripting;
using UnityEngine;

public class Building : Playable, ITarget<BuildingData>
{
    [SerializeField] protected CoroutineExecutor coroutineExecutor;
    [SerializeField] private new Collider collider;
    protected HealthSystem healthSystem;

    protected GameObject selectionIndicator;

    // Construction
    public bool IsConstructing { get; private set; }
    private float leftConstructionTime;
    public event Action OnConstructionStarted;
    public event Action OnConstructionFinished;

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

    public virtual Building SetUp(EntityData data, GameObject selectionIndicator, Team team)
    {
        this.data = data;
        this.team = team;

        blackBoard = new BlackBoard(data, coroutineExecutor, team);
        stateMachine = new BuildingStateMachine(blackBoard);

        healthSystem = new HealthSystem(data.MaxHealth);

        this.selectionIndicator = selectionIndicator;

        return this;
    }

    public override void OnSelected()
    {
        selectionIndicator.SetActive(false);
        selectionIndicator.SetActive(true);
    }

    public override void OnDeselected()
    {
        selectionIndicator.SetActive(false);
    }

    // TODO: put these construction methods in building states
    public void StartConstruction()
    {
        IsConstructing = true;
        leftConstructionTime = GetData().ConsructionTime;
        OnConstructionStarted?.Invoke();
    }

    public void TickConstruction()
    {
        leftConstructionTime -= Time.deltaTime;
        if (leftConstructionTime <= 0)
        {
            IsConstructing = false;
            OnConstructionFinished?.Invoke();
        }
    }

    public override void SetPosition(Vector3 position)
    {
        transform.position = position.WithY(PositionDeltaY);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
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