using UnityEngine;

public abstract class ModeBase
{
    public abstract void Enter();
    public abstract void Update();                
    public abstract void HandleInput();         
    public abstract void Exit();

    protected IModeTransitionRequester transitionRequester;
    public void Setup(IModeTransitionRequester transitionRequester)
    {
        this.transitionRequester = transitionRequester;
    }
}