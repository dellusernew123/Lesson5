using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    [SerializeField] private Npc _npc;
    [SerializeField] private Transform _player;
    [SerializeField] private Mover _mover;
    [SerializeField] private ParticleSystem _deathParticle;
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private float _minDistanceToTarget = 0.05f;

    [SerializeField] private float _randomWalkingDistance = 10f;
    [SerializeField] private List<Transform> _patrolTargets;

    private INpcStates _idleBehaviour;
    private INpcStates _activeBehaviour;

    private void Start()
    {
        foreach(SpawnPoint spawnPoint in _spawnPoints)
        {
            SetSpawnPointIdleBehaviour(spawnPoint);
            SetSpawnPointActiveBehaviour(spawnPoint);

            CreateNpc(spawnPoint, _idleBehaviour, _activeBehaviour);
        }
    }

    private void SetSpawnPointIdleBehaviour(SpawnPoint spawnPoint)
    {
        switch (spawnPoint.IdleStateVariations)
        {
            case StateVariations.DoingNothing:
                _idleBehaviour = new DoingNothing();
            break;

            case StateVariations.Patrolling:
                _idleBehaviour = new Patrolling(_patrolTargets, _minDistanceToTarget);
            break;

            case StateVariations.RandomWalking:
                _idleBehaviour = new RandomWalking(_randomWalkingDistance);
            break;

            default:
                _idleBehaviour = new DoingNothing();
            break;

        }
    }

    private void SetSpawnPointActiveBehaviour(SpawnPoint spawnPoint)
    {
        switch (spawnPoint.ActiveStateVariations)
        {
            case StateVariations.RunningAway:
                _activeBehaviour = new RunningAway();
            break;

            case StateVariations.Chasing:
                _activeBehaviour = new Chasing();
            break;

            case StateVariations.Dying:
                _activeBehaviour = new Dying(_deathParticle);
            break;

            default:
                _activeBehaviour = new Dying(_deathParticle);
            break;
        }
    }

    private void CreateNpc(SpawnPoint spawnPoint, INpcStates idleStateVariations, INpcStates activeStateVariations)
    {
        Npc npc = Instantiate(_npc, spawnPoint.transform.position, Quaternion.identity);
        
        npc.InitializeValues(
            _player, 
            _mover, 
            idleStateVariations,
            activeStateVariations
        );
    }
}
