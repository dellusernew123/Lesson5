using UnityEngine;

public class Dying : INpcStates
{
    private ParticleSystem _deathParticle;
    public Dying(ParticleSystem deathParticle)
    {
        _deathParticle = deathParticle;
    }

    public void Execute(Npc npc)
    {
        Object.Instantiate(_deathParticle, npc.transform.position, Quaternion.identity);
        Object.Destroy(npc.gameObject);
    }
}
