using UnityEngine;

public class Building : Playable, ITarget
{
    // State Machine
    protected StateMachineBase stateMachine;
    protected BlackBoard blackBoard;

    protected HealthSystem healthSystem;


    public virtual Building SetUp(EntityData data)
    {
        this.data = data;

        blackBoard = new BlackBoard(data);
        stateMachine = new BuildingStateMachine(blackBoard);

        healthSystem = new HealthSystem(data.MaxHealth);

        return this;
    }

    public override void OnSelected()
    {
        Debug.Log(name + " has selected");
    }

    public override void OnDeselected()
    {
        Debug.Log(name + " has unselected");
    }

    public Vector3 GetPosition()
    {
        throw new System.NotImplementedException();
    }

    public IHealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}