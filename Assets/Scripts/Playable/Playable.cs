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
    // Team that the object belongs to.
    [field: SerializeField] private Team Team {  get; set; }

    public abstract void OnSelected();
    public abstract void OnDeselected();

    public Transform GetTransform() { return transform; }


    private void SetUp(Team team)
    {
        Team = team;
    }

}