using CustomResourceManagement;
using UnityEngine;
using UnityEngine.AI;

public interface IMovable
{
    void MoveTo(Vector3 destination);
}

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Playable, IMovable
{
    [SerializeField] private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 destination)
    {
        agent?.SetDestination(destination);
    }

    public override void OnSelected()
    {
    }

    public override void OnDeselected()
    {
    }
}