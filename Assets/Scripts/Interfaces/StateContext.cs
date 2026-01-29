using UnityEngine;

// For injection into unit states 
// TODO: Split this if it's better
public interface IUnitStateContext
{
    // Transform
    public Vector3 GetPosition();
    void LookAt(Vector3 target);
    int CaculateContactDistance(ITarget target);
    int CaculateContactDistance(Target target);


    // NavMeshAgent
    void SetDestination(Vector3 destination);
    void ClearDestination();
    float GetRemainingDistance();
    bool HasArrived(float tolerance = 0.1f);

    // Animator
    void PlayAnimation(string stateName, int layer, float normalizedTime);
    void CrossFadeAnimation(string stateName, float normalizedTransitionDuration, int layer);
    bool IsAnimationInProgress(string stateName, int layer = 0);

    // Resource
    void CarryResource(ResourceType type, int amount);
    int DepositResource(ResourceType type);
    bool IsCarryingResources();
}

public interface IBuildingStateContext
{
    // Transform 
    public Vector3 GetPosition();

    IUnitGenerator GetUnitGenerator();
}
