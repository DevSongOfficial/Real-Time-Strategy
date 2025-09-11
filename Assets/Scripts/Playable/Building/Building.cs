using UnityEngine;

public class Building : Playable
{
    // State Machine
    protected StateMachine stateMachine;
    protected BlackBoard blackBoard;

    private HealthSystem healthSystem;


    public Building SetUp(EntityData data)
    {
        this.data = data;

        blackBoard = new BlackBoard(data);
        stateMachine = new StateMachine();

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
}