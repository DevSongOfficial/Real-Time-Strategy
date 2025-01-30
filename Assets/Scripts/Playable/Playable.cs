using UnityEngine;

public enum Team
{
    Red,
    Green,
    Blue,
}

public interface ISelectable
{
    void OnSelected();
    void OnDeselected();
}

public interface ITransformProvider
{
    Transform GetTransform();
}

public abstract class Playable : MonoBehaviour, ISelectable, ITransformProvider
{
    public abstract void OnSelected();
    public abstract void OnDeselected();

    public Transform GetTransform() { return transform; }

    [field: SerializeField] protected EntityData data;
}