using UnityEngine;

public class RunningAway : INpcStates
{
    public void Execute(Npc npc)
    {
        Vector3 direction = npc.GetDirectionToPlayer();

        Vector3 normalizedDirection = direction.normalized;

        npc.Move(-normalizedDirection);
        npc.ProcessRotateTo(normalizedDirection);
    }
}
