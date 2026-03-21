using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private float _minDistanceToPlayer = 1.5f;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _rotationSpeed = 600f;

    [SerializeField] private NpcStates _currentState = NpcStates.Idle;
    private IdleStateVariations _idleState;
    private ActiveStateVariations _activeState;
    private ParticleSystem _deathParticle;

    private Transform _player;
    private Mover _mover;

    private float _minDistanceToTarget = 0.05f;
    private Vector3 _currentTarget;
    private Queue<Vector3> _patrolPoints;

    private readonly float _randomWalkTimer = 1;
    private float _currentRandomWalkTimer;
    private float _randomWalkingDistance;
    private Vector3 _normalizedRandomDirection;

    private void Start()
    {
        _currentRandomWalkTimer = _randomWalkTimer;
    }

    private void Update()
    {
        CheckDistance();

        switch (_currentState)
        {
            case NpcStates.Idle:
                DoIdleAction();
            break;

            case NpcStates.Active:
                DoActiveAction();
            break;

            default:
                DoIdleAction();
            break;
        }
    }

    private void CheckDistance()
    {
        Vector3 playerDirection = GetDirectionToPlayer();
        float minDistanceSqr = _minDistanceToPlayer * _minDistanceToPlayer;

        bool isClose = playerDirection.sqrMagnitude <= minDistanceSqr;
        _currentState = isClose ? NpcStates.Active : NpcStates.Idle;
    }

    public void InitializeValues(
        IdleStateVariations idleState, 
        ActiveStateVariations activeState, 
        Transform player, 
        Mover mover, 
        List<Transform> patrolTargets, 
        float randomWalkingDistance, 
        ParticleSystem deathParticle)
    {
        _idleState = idleState;
        _activeState = activeState;
        _player = player;
        _mover = mover;
        _randomWalkingDistance = randomWalkingDistance;
        _deathParticle = deathParticle;

        SetPatrolPoints(patrolTargets);
    }

    public void SetPatrolPoints(List<Transform> targets)
    {
        _patrolPoints = new Queue<Vector3>();

        foreach (Transform traget in targets)
            _patrolPoints.Enqueue(traget.position);

        SwitchTarget();
    }

    private Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0f;
        return direction;
    }
        

    private void DoIdleAction()
    {
        switch (_idleState)
        {
            case IdleStateVariations.DoingNothing:
            break;

            case IdleStateVariations.Patrolling:
                Patrolling();
            break;

            case IdleStateVariations.RandomWalking:
                RandomWalking();
            break;

            default:
            break;

        }
    }

    private void Patrolling()
    {
        Vector3 direction = _currentTarget - transform.position;
        float minDistanceSqr = _minDistanceToTarget * _minDistanceToTarget;

        if (direction.sqrMagnitude <= minDistanceSqr)
        {
            SwitchTarget();
            direction = _currentTarget - transform.position;
        }

        Vector3 normalizedDirection = direction.normalized;
        _mover.Move(gameObject, normalizedDirection, _speed, _rotationSpeed);
    }

    private void SwitchTarget()
    {
        _currentTarget = _patrolPoints.Dequeue();
        _patrolPoints.Enqueue(_currentTarget);
    }

    private void RandomWalking()
    {
        _currentRandomWalkTimer -= Time.deltaTime;

        if (_currentRandomWalkTimer <= 0)
        {
            _currentRandomWalkTimer = _randomWalkTimer;

            float randomValueX = Random.Range(0,_randomWalkingDistance);
            float randomValueZ = Random.Range(0,_randomWalkingDistance);
            Vector3 randomCoordinates = new Vector3 (randomValueX, transform.position.y, randomValueZ);
            Debug.Log(randomCoordinates);

            Vector3 randomDirection = randomCoordinates - transform.position;
            _normalizedRandomDirection = randomDirection.normalized;   
        }

        _mover.Move(gameObject, _normalizedRandomDirection, _speed, _rotationSpeed);
    }

    private void DoActiveAction()
    {
        switch (_activeState)
        {
            case ActiveStateVariations.RunningAway:
                RunningAway();
            break;

            case ActiveStateVariations.Chasing:
                Chasing();
            break;

            case ActiveStateVariations.Dying:
                Dying();
            break;

            default:
                Dying();
            break;
        }
    }

    private void RunningAway()
    {
        Vector3 direction = GetDirectionToPlayer();

        Vector3 normalizedDirection = direction.normalized;

        _mover.Move(gameObject, -normalizedDirection, _speed, _rotationSpeed);
    }

    private void Chasing()
    {
        Vector3 direction = GetDirectionToPlayer();
        float minDistanceSqr = _minDistanceToPlayer * _minDistanceToPlayer;

        if (direction.sqrMagnitude <= minDistanceSqr)
            return;

        Vector3 normalizedDirection = direction.normalized;

        _mover.Move(gameObject, normalizedDirection, _speed, _rotationSpeed);
    }

    private void Dying()
    {
        Instantiate(_deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
