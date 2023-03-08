using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine _stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        _stateMachine.CharacterController.Move((motion + _stateMachine.ForceReciver.movement) * deltaTime);
    }

    protected void FacePlayer()
    {
        if (_stateMachine.Player == null) return;

        Vector3 lookPos = _stateMachine.Player.transform.position - _stateMachine.transform.position;
        lookPos.y = 0;

        _stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected bool IsInChaseRange()
    {
        float playerDistanceSqr = (_stateMachine.Player.transform.position - _stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= _stateMachine.PlayerDetectionRange * _stateMachine.PlayerDetectionRange;
    }

    protected bool IsInAttackRange()
    {
        float playerDistanceSqr = (_stateMachine.Player.transform.position - _stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= _stateMachine.AttackRange * _stateMachine.AttackRange;
    }
}
