using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("Targeting Blend Tree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _stateMachine.InputReader.CancelEvent += OnCancel;

        _stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        if (_stateMachine.InputReader.IsAttacking)
        {
            _stateMachine.SwitchState(new PlayerAttackingState(_stateMachine, 0));
            return;
        }
        if (_stateMachine.InputReader.IsBlocking)
        {
            _stateMachine.SwitchState(new PlayerBlockState(_stateMachine));
            return;
        }
        if (_stateMachine.Targeter.currentTarget == null)
        {
            _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.TargetingMovementSpeed, deltaTime);

        FaceTarget();
        UpdateAnimatior(deltaTime);
    }

    private void OnCancel()
    {
        _stateMachine.Targeter.Cancel();
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement += _stateMachine.transform.right * _stateMachine.InputReader.MovementValue.x;
        movement += _stateMachine.transform.forward * _stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void UpdateAnimatior(float deltaTime)
    {
        if (_stateMachine.InputReader.MovementValue.y == 0)
        {
            _stateMachine.Animator.SetFloat(TargetingForwardHash, 0, 0.01f, deltaTime);
        }
        else
        {
            float value = _stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            _stateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.01f, deltaTime);
        }

        if (_stateMachine.InputReader.MovementValue.x == 0)
        {
            _stateMachine.Animator.SetFloat(TargetingRightHash, 0, 0.01f, deltaTime);
        }
        else
        {
            float value = _stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            _stateMachine.Animator.SetFloat(TargetingRightHash, value, 0.01f, deltaTime);
        }
    }

    public override void Exit()
    {
        _stateMachine.InputReader.CancelEvent -= OnCancel;
    }
}
