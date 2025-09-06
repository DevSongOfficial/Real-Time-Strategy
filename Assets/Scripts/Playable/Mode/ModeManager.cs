using UnityEngine;

public abstract class ModeBase
{
    public abstract void Enter();
    public abstract void Update();                
    public abstract void HandleInput();         
    public abstract void Exit();
}