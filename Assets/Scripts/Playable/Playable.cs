using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum Team
{
    None, // Neutral
    Red,
    Green,
    Blue,
}

public interface ISelectable // Selectable by mouse0 or keyboard or ... .
{
    Team GetTeam();
    EntityData GetData();
    
    // Selection
    void OnSelected();
    void OnDeselected();

    void ExecuteCommand(CommandData command);
    event Action<CommandData> OnCommandExecuted;
}

public interface ITransformProvider
{
    Transform GetTransform();
}

public abstract class Playable : MonoBehaviour, ISelectable, ITransformProvider
{
    // Entity Data
    public EntityData GetData() => data;
    protected EntityData data;

    // Team
    public Team GetTeam() => team;
    protected Team team;
    
    // State Machine
    protected StateMachineBase stateMachine;
    protected BlackBoard blackBoard;

    // Command
    public virtual void ExecuteCommand(CommandData command) => OnCommandExecuted?.Invoke(command);
    public event Action<CommandData> OnCommandExecuted;


    public abstract void OnSelected();
    public abstract void OnDeselected();


    public Transform GetTransform() { return transform; }
    public virtual void SetPosition(Vector3 position) { transform.position = position; }
    public void SetRotation(Quaternion rotation) {  transform.rotation = rotation; }
}