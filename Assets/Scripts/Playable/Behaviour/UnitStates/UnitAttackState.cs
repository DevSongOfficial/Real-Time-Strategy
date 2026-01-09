using System.Collections;
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
        if (blackBoard.target.Entity is IDamageable target)
            blackBoard.coroutineExecutor.Execute(AttackRoutine(target));
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        base.Update();
    }

    private IEnumerator AttackRoutine(IDamageable target)
    {
        animator.Play(blackBoard.BaseData.Combat.Animation, 0, 0f);
        blackBoard.attackCooldown = blackBoard.BaseData.Combat.AttackCooldown;

        yield return new WaitForSeconds(blackBoard.BaseData.Combat.WindupTime);
        target.GetDamaged(blackBoard.BaseData.Combat.AttackDamage);

        yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).IsName(blackBoard.BaseData.Combat.Animation) &&
           animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        stateMachine.ChangeState<UnitMoveState>();
    }
}