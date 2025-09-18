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
    protected EntityData data;

    public abstract void OnSelected();
    public abstract void OnDeselected();

    public Transform GetTransform() { return transform; }
    public virtual void SetPosition(Vector3 position) { transform.position = position; }
    public void SetRotation(Quaternion rotation) {  transform.rotation = rotation; }

}