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

public abstract class Playable : MonoBehaviour, ISelectable
{
    // Team that the object belongs to.
    [field: SerializeField] private Team Team {  get; set; }

    public abstract void OnSelected();
    public abstract void OnDeselected();


    private void SetUp(Team team)
    {
        Team = team;
    }
}