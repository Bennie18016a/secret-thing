using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    private List<Target> targets = new List<Target>();
    private Camera _mainCamera;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    public Target currentTarget { get; private set; }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            targets.Add(target);
            target.onDestroyed += RemoveTarget;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            targets.Remove(target);
            RemoveTarget(target);
        }
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) return false;

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (Target target in targets)
        {
            Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);

            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) { continue; }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestTargetDistance) { closestTarget = target; closestTargetDistance = toCenter.sqrMagnitude; }
        }

        if (closestTarget == null) { return false; }

        currentTarget = closestTarget;
        targetGroup.AddMember(currentTarget.transform, 1f, 2f);
        return true;
    }

    public void Cancel()
    {
        if (currentTarget != null) targetGroup.RemoveMember(currentTarget.transform);
        currentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if (currentTarget == target)
        {
            targetGroup.RemoveMember(target.transform);
            currentTarget = null;
        }

        target.onDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
