using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerBaseState
{
    private readonly int FreeBlockBlendTree = Animator.StringToHash("FreeBlock Blend Tree");
    private readonly int TargetBlockBlendTree = Animator.StringToHash("TargetBlock Blend Tree");
    private readonly int FreeBlockHash = Animator.StringToHash("FreeBlock");
    private readonly int TargetFoward = Animator.StringToHash("TargetForward");
    private readonly int TargetRight = Animator.StringToHash("TargetRight");
    private const float AnimatorDampTime = 0.1f;


    public PlayerBlockState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _stateMachine.Health.Invunerable(true);

        if (_stateMachine.Targeter.currentTarget == null) { _stateMachine.Animator.CrossFadeInFixedTime(FreeBlockBlendTree, 0.1f); }
        else { _stateMachine.Animator.CrossFadeInFixedTime(TargetBlockBlendTree, 0.1f); }
    }

    public override void Tick(float deltaTime)
    {
        if (_stateMachine.InputReader.IsAttacking)
        {
            _stateMachine.SwitchState(new PlayerAttackingState(_stateMachine, 0));
            return;
        }
        if (!_stateMachine.InputReader.IsBlocking)
        {
            ReturnToLocomotion();
        }
        if (_stateMachine.Targeter.currentTarget == null)
        {
            Vector3 movement = CalculateMovementFree();

            Move(movement * _stateMachine.BlockMovementSpeed, deltaTime);

            if (_stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                _stateMachine.Animator.SetFloat(FreeBlockHash, 0, AnimatorDampTime, deltaTime);
                return;
            }
            _stateMachine.Animator.SetFloat(FreeBlockHash, 1, AnimatorDampTime, deltaTime);

            FaceMovementDirection(movement, deltaTime);
        }
        else
        {
            Vector3 movement = CalculateMovementTarget();
            Move(movement * _stateMachine.BlockMovementSpeed, deltaTime);

            FaceTarget();
            UpdateAnimatior(deltaTime);
        }
    }

    private void UpdateAnimatior(float deltaTime)
    {
        if (_stateMachine.InputReader.MovementValue.y == 0)
        {
            _stateMachine.Animator.SetFloat(TargetFoward, 0, 0.01f, deltaTime);
        }
        else
        {
            float value = _stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            _stateMachine.Animator.SetFloat(TargetFoward, value, 0.01f, deltaTime);
        }

        if (_stateMachine.InputReader.MovementValue.x == 0)
        {
            _stateMachine.Animator.SetFloat(TargetRight, 0, 0.01f, deltaTime);
        }
        else
        {
            float value = _stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            _stateMachine.Animator.SetFloat(TargetRight, value, 0.01f, deltaTime);
        }
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        _stateMachine.transform.rotation = Quaternion.Lerp(
            _stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * _stateMachine.RotationDamping
        );
    }

    private Vector3 CalculateMovementTarget()
    {
        Vector3 movement = new Vector3();

        movement += _stateMachine.transform.right * _stateMachine.InputReader.MovementValue.x;
        movement += _stateMachine.transform.forward * _stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private Vector3 CalculateMovementFree()
    {
        Vector3 forward = _stateMachine.MainCameraTransform.forward;
        Vector3 right = _stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * _stateMachine.InputReader.MovementValue.y + right * _stateMachine.InputReader.MovementValue.x;
    }

    public override void Exit()
    {
        _stateMachine.Health.Invunerable(false);
    }
}
