using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
            blackBoard.coroutineExecutor.ExecuteCoroutine(AttackRoutine(target));
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
        animator.Play("Melee Attack", 0, 0f);
        target.GetDamaged(blackBoard.data.AttackDamage);
        blackBoard.attackCooldown = blackBoard.data.AttackDalay;

        yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Attack") &&
           animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        stateMachine.ChangeState<UnitMoveState>();
    }
}
