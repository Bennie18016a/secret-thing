using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {

        if (!IsInChaseRange())
        {
            _stateMachine.SwitchState(new EnemyIdleState(_stateMachine));
            return;
        }
        else if (IsInAttackRange())
        {
            _stateMachine.SwitchState(new EnemyAttackState(_stateMachine));
            return;
        }
        _stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);

        MoveToPlayer(deltaTime);
        FacePlayer();
    }

    private void MoveToPlayer(float deltaTime)
    {

        _stateMachine.NavMeshAgent.nextPosition = _stateMachine.transform.position;
        if (!_stateMachine.NavMeshAgent.isOnNavMesh) { return; }

        _stateMachine.NavMeshAgent.destination = _stateMachine.Player.transform.position;

        Move(_stateMachine.NavMeshAgent.desiredVelocity.normalized * _stateMachine.MovementSpeed, deltaTime);
    }
    public override void Exit()
    {
        _stateMachine.NavMeshAgent.ResetPath();
        _stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}
