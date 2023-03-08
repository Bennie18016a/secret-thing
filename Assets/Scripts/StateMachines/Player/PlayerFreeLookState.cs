using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("Freelook Blend Tree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("Freelook Speed");
    private const float AnimatorDampTime = 0.1f;
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _stateMachine.InputReader.MessageEvent += PlaceMessage;
        _stateMachine.InputReader.InteractEvent += Interact;
        _stateMachine.InputReader.TargetEvent += OnTarget;

        _stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, 0.1f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Tick(float deltaTime)
    {
        if (!_stateMachine.CanMove) { return; }

        Vector3 movement = CalculateMovement();

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

        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);

        if (_stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            _stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        _stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

        FaceMovementDirection(movement, deltaTime);
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        _stateMachine.transform.rotation = Quaternion.Lerp(
            _stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * _stateMachine.RotationDamping
        );
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = _stateMachine.MainCameraTransform.forward;
        Vector3 right = _stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * _stateMachine.InputReader.MovementValue.y + right * _stateMachine.InputReader.MovementValue.x;
    }

    private void PlaceMessage()
    {
        if (_stateMachine.Texting) return;

        _stateMachine.Texting = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _stateMachine.CanMove = false;

        _stateMachine.TextMenu.SetActive(true);
    }

    private void Interact()
    {
        GameObject nearestInteractable = null;
        foreach (GameObject interactable in GameObject.FindGameObjectsWithTag("Interact"))
        {
            if (Vector3.Distance(_stateMachine.transform.position, interactable.transform.position) < 1.1
            &&
            Vector3.Distance(_stateMachine.transform.position, interactable.transform.position) > 0.1)
            {
                nearestInteractable = interactable;
            }
        }

        if (nearestInteractable == null) { return; }

        switch (nearestInteractable.name)
        {
            case ("Beacon"):
                nearestInteractable.GetComponent<CogBeacon>().Interact();
                break;
        }
    }

    private void OnTarget()
    {
        if (!_stateMachine.Targeter.SelectTarget()) return;
        _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
    }

    public override void Exit()
    {
        _stateMachine.InputReader.MessageEvent -= PlaceMessage;
        _stateMachine.InputReader.InteractEvent -= Interact;
        _stateMachine.InputReader.TargetEvent -= OnTarget;
    }
}
