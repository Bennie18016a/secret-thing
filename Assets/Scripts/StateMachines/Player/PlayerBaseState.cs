using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine _stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
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

    protected void FaceTarget()
    {
        if (_stateMachine.Targeter.currentTarget == null) return;

        Vector3 lookPos = _stateMachine.Targeter.currentTarget.transform.position - _stateMachine.transform.position;
        lookPos.y = 0;

        _stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected void ReturnToLocomotion()
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
}
