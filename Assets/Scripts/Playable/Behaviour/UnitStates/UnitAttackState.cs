using System.Collections;
using UnityEngine;

public class UnitAttackState : UnitStateBase
{
    private IUnitStateContext stateContext;

    public UnitAttackState(UnitStateMachine stateMachine, BlackBoard blackBoard, IUnitStateContext stateContext)
        : base(stateMachine, blackBoard) 
    {
        this.stateContext = stateContext;
    }

    public override void Enter()
    {
        if (blackBoard.Target.Entity is IDamageable target)
        {
            stateContext.LookAt(blackBoard.Target.GetPosition());
            blackBoard.coroutineExecutor.Execute(AttackRoutine(target));
        }
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
        stateContext.PlayAnimation(blackBoard.BaseData.Combat.AttackAnimation, 0, 0f);
        blackBoard.attackCooldown = blackBoard.BaseData.Combat.AttackCooldown;

        yield return new WaitForSeconds(blackBoard.BaseData.Combat.WindupTime);
        target.GetDamaged(blackBoard.BaseData.Combat.AttackDamage);

        yield return null;

        while (stateContext.IsAnimationInProgress(blackBoard.BaseData.Combat.AttackAnimation, 0))
            yield return null;

        if (IsTargetAlive(target))
            stateMachine.ChangeState<UnitMoveState>();
        else
            stateMachine.ChangeState<UnitIdleState>();
    }

    private bool IsTargetAlive(IDamageable target)
    {
        return (target as Unit).GetHealthSystem().CurrentHealth > 0;
    }
}