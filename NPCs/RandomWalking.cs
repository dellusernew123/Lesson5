using UnityEngine;

public class RandomWalking : INpcStates
{
    private readonly float _randomWalkTimer = 1;
    private float _currentRandomWalkTimer = 1;
    private readonly float _randomWalkingDistance;
    private Vector3 _normalizedRandomDirection;

    public RandomWalking(float randomWalkingDistance)
    {
        _randomWalkingDistance = randomWalkingDistance;
    }

    private void Start()
    {
        _currentRandomWalkTimer = _randomWalkTimer;
    }

    public void Execute(Npc npc)
    {
        _currentRandomWalkTimer -= Time.deltaTime;

        if (_currentRandomWalkTimer <= 0)
        {
            _currentRandomWalkTimer = _randomWalkTimer;

            float randomValueX = Random.Range(0, _randomWalkingDistance);
            float randomValueZ = Random.Range(0, _randomWalkingDistance);
            Vector3 randomCoordinates = new Vector3 (randomValueX, npc.transform.position.y, randomValueZ);
            Debug.Log(randomCoordinates);

            Vector3 randomDirection = randomCoordinates - npc.transform.position;
            _normalizedRandomDirection = randomDirection.normalized;   
        }

        npc.Move(_normalizedRandomDirection);
        npc.ProcessRotateTo(_normalizedRandomDirection);
    }
}
