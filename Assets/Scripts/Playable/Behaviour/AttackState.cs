using UnityEngine;

public class AttackState : UnitStateBase
{
    public AttackState(UnitStateMachine stateMachine, BlackBoard blackBoard)
        : base(stateMachine, blackBoard) { }

    public override void Enter()
    {
        TryAttack();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        
    }

    private void TryAttack()
    {
        if (blackBoard.target.Entity is IDamageable target)
            target.GetDamaged(blackBoard.data.AttackDamage);

        stateMachine.ChangeState(stateMachine.IdleState);
    }
}
