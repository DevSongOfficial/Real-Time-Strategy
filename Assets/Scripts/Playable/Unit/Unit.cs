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
    [SerializeField] private GameObject selectionIndicator;

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
        selectionIndicator.SetActive(false);
        selectionIndicator.SetActive(true);
    }

    public override void OnDeselected()
    {
        selectionIndicator.SetActive(false);
    }
}