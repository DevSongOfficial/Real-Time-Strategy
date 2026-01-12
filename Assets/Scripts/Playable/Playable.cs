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
    
    void OnSelected();
    void OnDeselected();

    // Command
    CommandSetData CommandSet { get; }
    void ExecuteCommand(Command command);
    event Action<Command> OnCommandExecuted;
}

public interface ITransformProvider
{
    Transform GetTransform();
}

public abstract class Playable : MonoBehaviour, ISelectable, ITransformProvider
{
    protected EntityData data;

    public Team GetTeam() => team;
    protected Team team;


    public abstract void OnSelected();
    public abstract void OnDeselected();

    public CommandSetData CommandSet => commandSet;
    [SerializeField] protected CommandSetData commandSet;
    public event Action<Command> OnCommandExecuted;
    public virtual void ExecuteCommand(Command command) => OnCommandExecuted?.Invoke(command);


    public Transform GetTransform() { return transform; }
    public virtual void SetPosition(Vector3 position) { transform.position = position; }
    public void SetRotation(Quaternion rotation) {  transform.rotation = rotation; }

}