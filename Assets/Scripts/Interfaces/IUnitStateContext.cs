using UnityEngine;

// For injection into unit states 
// TODO: Split this if it's better
public interface IUnitStateContext
{
    // Transform
    public Vector3 GetPosition();
    public void LookAt(Vector3 target);

    // NavMeshAgent
    public void SetDestination(Vector3 destination);
    public void ClearDestination();
    public float GetRemainingDistance();
    public bool HasArrived();

    // Animator
    public void PlayAnimation(string stateName, int layer, float normalizedTime);
    public void CrossFadeAnimation(string stateName, float normalizedTransitionDuration, int layer);
    public bool IsAnimationInProgress(string stateName, int layer = 0);
}
