using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        GameObject.Destroy(_stateMachine.gameObject);
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
