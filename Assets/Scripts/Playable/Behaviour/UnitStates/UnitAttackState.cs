using UnityEngine;

public class UnitAttackState : UnitStateBase
{
    private Animator animator;
    public UnitAttackState(UnitStateMachine stateMachine, BlackBoard blackBoard, Animator animator)
        : base(stateMachine, blackBoard) 
    {
        this.animator = animator;

    }

    public override void Enter()
    {
        TryAttack();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        base.Update();
    }

    private void TryAttack()
    {
        if (blackBoard.target.Entity is IDamageable target)
        {
            animator.Play("Melee Attack", 0, 0f);

            target.GetDamaged(blackBoard.data.AttackDamage);
            blackBoard.attackCooldown = blackBoard.data.AttackDalay;
        }

        stateMachine.ChangeState<UnitMoveState>();
    }
}
