using UnityEngine;

public class Chasing : INpcStates
{
    public void Execute(Npc npc)
    {
        Vector3 direction = npc.GetDirectionToPlayer();

        if (direction.sqrMagnitude <= npc.MinDistanceToTargetSqr)
            return;

        Vector3 normalizedDirection = direction.normalized;

        npc.Move(normalizedDirection);
        npc.ProcessRotateTo(normalizedDirection);
    }
}
