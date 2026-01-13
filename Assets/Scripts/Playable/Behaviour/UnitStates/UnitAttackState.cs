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
        if (blackBoard.target.Entity is IDamageable target)
        {
            stateContext.LookAt(blackBoard.target.GetPosition());
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
        stateContext.PlayAnimation(blackBoard.BaseData.Combat.Animation, 0, 0f);
        blackBoard.attackCooldown = blackBoard.BaseData.Combat.AttackCooldown;

        yield return new WaitForSeconds(blackBoard.BaseData.Combat.WindupTime);
        target.GetDamaged(blackBoard.BaseData.Combat.AttackDamage);

        yield return null;

        while (stateContext.IsAnimationInProgress(blackBoard.BaseData.Combat.Animation, 0))
        {
            yield return null;
        }

        stateMachine.ChangeState<UnitMoveState>();
    }
}