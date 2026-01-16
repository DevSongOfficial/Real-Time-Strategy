using UnityEngine;

// For injection into unit states 
// TODO: Split this if it's better
public interface IUnitStateContext
{
    // Transform
    public Vector3 GetPosition();
    public void LookAt(Vector3 target);
    public int CaculateContactDistance(ITarget target);
    public int CaculateContactDistance(Target target);


    // NavMeshAgent
    public void SetDestination(Vector3 destination);
    public void ClearDestination();
    public float GetRemainingDistance();
    public bool HasArrived(float tolerance = 0.1f);

    // Animator
    public void PlayAnimation(string stateName, int layer, float normalizedTime);
    public void CrossFadeAnimation(string stateName, float normalizedTransitionDuration, int layer);
    public bool IsAnimationInProgress(string stateName, int layer = 0);
}

public interface IBuildingStateContext
{
    // Transform 
    public Vector3 GetPosition();

    IUnitGenerator GetUnitGenerator();
}
