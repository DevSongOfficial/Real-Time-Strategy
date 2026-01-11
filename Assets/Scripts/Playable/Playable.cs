using UnityEngine;

public enum Team
{
    None, // Neutral
    Red,
    Green,
    Blue,
}

public interface ISelectable // Selectable by mouse0 or keyboard or ... .
{
    void OnSelected();
    void OnDeselected();
    Team GetTeam();
    CommandSetData CommandSet { get; }
    
}

public interface ITransformProvider
{
    Transform GetTransform();
}

public abstract class Playable : MonoBehaviour, ISelectable, ITransformProvider
{
    protected EntityData data;
    protected Team team;

    [SerializeField] protected CommandSetData commandSet;

    public abstract void OnSelected();
    public abstract void OnDeselected();
    public Team GetTeam() => team;
    public CommandSetData CommandSet => commandSet;


    public Transform GetTransform() { return transform; }
    public virtual void SetPosition(Vector3 position) { transform.position = position; }
    public void SetRotation(Quaternion rotation) {  transform.rotation = rotation; }

}