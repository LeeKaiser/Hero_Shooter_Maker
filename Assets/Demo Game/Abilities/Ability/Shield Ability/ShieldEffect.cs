using UnityEngine;

public class ShieldEffect : StatusEffect
{
    [Tooltip("shield multiplier")]
    public float ShieldMultiplier;

    public override void ApplyEffect()
    {
        Active = true;
        RemainingDuration = Stats.EffectDuration;
        AffectedPlayer.ModifyDamageTakeMult(ShieldMultiplier);
    }

    protected override void RemoveEffect()
    {
        //reverse the speed bonus
        Active = false;
        AffectedPlayer.ModifyDamageTakeMult(-ShieldMultiplier);
    }
}
