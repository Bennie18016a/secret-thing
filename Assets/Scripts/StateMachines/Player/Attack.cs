using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attack
{
    [field: SerializeField] public string AnimatonName { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
    [field: SerializeField] public int CombatStateIndex { get; private set; } = -1;
    [field: SerializeField] public float ComboAttackTime { get; private set; }
    [field: SerializeField] public float ForceTime { get; private set; }
    [field: SerializeField] public float Force { get; private set; }
    [field: SerializeField] public float Knockback { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
}
