using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _rotationSpeed = 600f;
    [SerializeField] private float _minDistanceToPlayer = 1.5f;
    [SerializeField] private float _minDistanceToTarget = 0.05f;

    private Transform _player;
    private Mover _mover;

    private INpcStates _idleBehaviour;
    private INpcStates _activeBehaviour;
    private INpcStates _currentBehaviour;

    public float MinDistanceToTargetSqr => _minDistanceToTarget * _minDistanceToTarget;
    public float MinDistanceToPlayerSqr => _minDistanceToPlayer * _minDistanceToPlayer;


    private void Update()
    {
        CheckDistance();
        _currentBehaviour.Execute(this);
    }

    private void CheckDistance()
    {
        Vector3 playerDirection = GetDirectionToPlayer();
        playerDirection.y = 0;
        float minDistanceSqr = _minDistanceToPlayer * _minDistanceToPlayer;

        bool isClose = playerDirection.sqrMagnitude <= minDistanceSqr;
        _currentBehaviour = isClose ? _activeBehaviour : _idleBehaviour;
    }

    public void InitializeValues(
        Transform player, 
        Mover mover,
        INpcStates idleBehaviour,
        INpcStates activeBehaviour)
    {
        _player = player;
        _mover = mover;
        _idleBehaviour = idleBehaviour;
        _activeBehaviour = activeBehaviour;
    }

    public Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0f;
        return direction;
    }

    public void Move(Vector3 direction)
    {
        _mover.Move(direction, _speed);
    }

    public void ProcessRotateTo(Vector3 direction)
    {
        _mover.ProcessRotateTo(direction, _rotationSpeed, gameObject);
    }
}
