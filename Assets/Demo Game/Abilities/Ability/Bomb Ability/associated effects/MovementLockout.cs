using UnityEngine;

public class MovementLockout : StatusEffect
{
    public override void ApplyEffect()
    {
        Active = true;
        RemainingDuration = Stats.EffectDuration;
        AffectedPlayer.PlayerMovement.SetHorizontalMovementPause(true);
        
        AIMovement aimovement = AffectedPlayer.PlayerArmature.GetComponent<AIMovement>();
        if (aimovement != null && aimovement.enabled)
        {
            aimovement.PauseNavmesh();
        }
        
    }

    protected override void RemoveEffect()
    {
        Active = false;
        AffectedPlayer.PlayerMovement.SetHorizontalMovementPause(false);
    }
}
