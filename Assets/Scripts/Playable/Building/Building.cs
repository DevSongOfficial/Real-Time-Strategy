using System;
using Unity.VisualScripting;
using UnityEngine;

public class Building : Playable, ITarget
{
    // State Machine
    protected StateMachineBase stateMachine;
    protected BlackBoard blackBoard;

    protected HealthSystem healthSystem;

    [SerializeField] private new Collider collider;

    protected GameObject selectionIndicator;

    // TODO: I think it's better to put this variable in EntityData and write on my own. (not getting it through collider)
    public float PositionDeltaY { get; private set; }

    // Better not use Start().
    protected virtual void Awake()
    {
        if(collider == null)
            collider = GetComponentInChildren<Collider>();

        PositionDeltaY = collider.bounds.extents.y;
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public virtual Building SetUp(EntityData data, GameObject selectionIndicator)
    {
        this.data = data;

        blackBoard = new BlackBoard(data);
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

    public EntityData GetData()
    {
        return data;
    }
}