using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private const float TarnsitionDuration = 0.1f;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        FacePlayer();
        _stateMachine.WeaponDamage.SetDamage(_stateMachine.AttackDamage, _stateMachine.Knockback);
        _stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TarnsitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(_stateMachine.Animator) >= 1f)
        {
            _stateMachine.SwitchState(new EnemyChasingState(_stateMachine));
        }
    }

    public override void Exit()
    {

    }
}
