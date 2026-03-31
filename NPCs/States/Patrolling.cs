using System.Collections.Generic;
using UnityEngine;

public class Patrolling : INpcStates
{
    private readonly Queue<Vector3> _patrolPoints;
    private Vector3 _currentTarget;
    private readonly float _minDistanceSqr;

    public Patrolling(List<Transform> patrolTargets, float minDistanceToTarget)
    {
        _patrolPoints = new Queue<Vector3>();

        foreach(Transform target in patrolTargets)
            _patrolPoints.Enqueue(target.position);
        
        _currentTarget = _patrolPoints.Dequeue();
        _patrolPoints.Enqueue(_currentTarget);

        _minDistanceSqr = minDistanceToTarget * minDistanceToTarget;
    }

    public void Execute(Npc npc)
    {
        Vector3 direction = _currentTarget - npc.transform.position;

        if (direction.sqrMagnitude <= npc.MinDistanceToPlayerSqr)
        {
            SwitchTarget();
            direction = _currentTarget - npc.transform.position;
        }

        Vector3 normalizedDirection = direction.normalized;
        
        npc.Move(normalizedDirection);
        npc.ProcessRotateTo(normalizedDirection);
    }

    private void SwitchTarget()
    {
        _currentTarget = _patrolPoints.Dequeue();
        _patrolPoints.Enqueue(_currentTarget);
    }
}
