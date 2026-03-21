using System;
using System.Runtime.InteropServices.WindowsRuntime;
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
    bool CanSelect();

    void ExecuteCommand(CommandData command);
    event Action<CommandData> OnCommandExecuted;
}

public interface ITransformProvider
{
    Transform GetTransform();
}

public abstract class Playable : MonoBehaviour, ISelectable, ITransformProvider
{
    [SerializeField] protected new Collider collider;
    [SerializeField] protected CoroutineExecutor coroutineExecutor;

    // Entity Data
    public EntityData GetData() => data;
    protected EntityData data;

    // Team
    protected TeamContext teamContext;
    public Team GetTeam() => teamContext.Team;

    // State Machine
    protected StateMachineBase stateMachine;
    protected BlackBoard blackBoard;

    // Command
    public virtual void ExecuteCommand(CommandData command) => OnCommandExecuted?.Invoke(command);
    public event Action<CommandData> OnCommandExecuted;


    public abstract void OnSelected();
    public abstract void OnDeselected();
    public abstract bool CanSelect();


    // TODO: I think it's better to put this variable in EntityData and write on my own. (not getting it through collider)
    public float GetPositionDeltaY() => collider.bounds.extents.y;
    public Transform GetTransform() { return transform; }
    public virtual void SetPosition(Vector3 position) { transform.position = position; }
    public void SetRotation(Quaternion rotation) {  transform.rotation = rotation; }

    protected virtual void Awake()
    {
        if (collider == null)
            collider = GetComponentInChildren<Collider>();

        if (coroutineExecutor != null)
            coroutineExecutor = GetComponent<CoroutineExecutor>();
    }
}