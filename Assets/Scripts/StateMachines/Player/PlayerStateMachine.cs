using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReciver ForceReciver { get; private set; }
    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float BlockMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public GameObject TextMenu { get; private set; }
    [field: SerializeField] public bool CanMove { get; set; }
    [field: SerializeField] public bool Texting { get; set; }
    public Transform MainCameraTransform { get; private set; }

    private void Start()
    {
        MainCameraTransform = Camera.main.transform;
        CanMove = false;
        SwitchState(new PlayerFreeLookState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }
    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }
}
