using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float _previousFrameTime;
    private bool _appliedForce;
    private Attack _attack;
    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        _attack = _stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        _stateMachine.WeaponDamage.SetDamage(_attack.AttackDamage, _attack.Knockback);
        _stateMachine.Animator.CrossFadeInFixedTime(_attack.AnimatonName, _attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedTime(_stateMachine.Animator);

        if (normalizedTime >= _previousFrameTime && normalizedTime < 1f)
        {
            if (normalizedTime >= _attack.ForceTime) { TryApplyForce(); }
            if (_stateMachine.InputReader.IsAttacking) { TryComboAttack(normalizedTime); }
        }
        else
        {
            if (_stateMachine.Targeter.currentTarget != null)
            {
                _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
            }
            else
            {
                _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
            }
        }

        _previousFrameTime = normalizedTime;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (_attack.CombatStateIndex == -1) { Debug.Log("No combo"); return; }
        if (normalizedTime < _attack.ComboAttackTime) { Debug.Log("Too early"); return; }

        _stateMachine.SwitchState(new PlayerAttackingState(_stateMachine, _attack.CombatStateIndex));
    }

    private void TryApplyForce()
    {
        if (_appliedForce) return;
        _stateMachine.ForceReciver.AddForce(_stateMachine.transform.forward * _attack.Force);
        _appliedForce = true;
    }



    public override void Exit()
    {

    }
}
