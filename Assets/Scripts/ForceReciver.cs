using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReciver : MonoBehaviour
{
    public Vector3 movement => _impact + Vector3.up * verticalVelocity;
    [SerializeField] CharacterController cc;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float drag = 0.3f;

    private float verticalVelocity;
    private Vector3 _impact;
    private Vector3 _dampingVelocity;

    private void Update()
    {
        if (verticalVelocity < 0f && cc.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, drag);

        if (_impact.sqrMagnitude < 0.2f * 0.2f && agent != null)
        {
            _impact = Vector3.zero;
            agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        _impact += force;
        if (agent != null)
        {
            agent.enabled = false;
        }
    }
}
