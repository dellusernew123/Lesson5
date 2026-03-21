using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    [SerializeField] private GameObject _npcPrefab;
    [SerializeField] private Transform _player;
    [SerializeField] private Mover _mover;
    [SerializeField] private ParticleSystem _deathParticle;
    [SerializeField] private List<SpawnPoint> _spawnPoints;

    [SerializeField] private float _randomWalkingDistance = 10f;
    [SerializeField] private List<Transform> _patrolTargets;

    private void Start()
    {
        foreach(SpawnPoint spawnPoint in _spawnPoints)
        {
            CreateNpc(spawnPoint, spawnPoint.IdleStateVariations, spawnPoint.ActiveStateVariations);
        }
    }

    private void CreateNpc(SpawnPoint spawnPoint, IdleStateVariations idleStateVariations, ActiveStateVariations activeStateVariations)
    {
        Npc npc = Instantiate(_npcPrefab, spawnPoint.transform.position, Quaternion.identity).GetComponent<Npc>();
        
        npc.InitializeValues(
            spawnPoint.IdleStateVariations, 
            spawnPoint.ActiveStateVariations,
            _player, 
            _mover, 
            _patrolTargets, 
            _randomWalkingDistance, 
            _deathParticle
        );
    }
}
