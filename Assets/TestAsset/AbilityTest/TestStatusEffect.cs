using UnityEngine;

public class TestStatusEffect : StatusEffect
{
    [Tooltip("amount of speed boost")]
    public float SpeedMultiplierTest;

    public override void ApplyEffect()
    {
        Active = true;
        RemainingDuration = statusEffectStat.EffectDuration;
        AffectedPlayer.ModifyForwardSpeed(SpeedMultiplierTest);
        AffectedPlayer.ModifyStrafeSpeed(SpeedMultiplierTest);
        AffectedPlayer.ModifyBackwardSpeed(SpeedMultiplierTest);
    }

    protected override void RemoveEffect()
    {
        //reverse the speed bonus
        Active = false;
        AffectedPlayer.ModifyForwardSpeed(-SpeedMultiplierTest);
        AffectedPlayer.ModifyStrafeSpeed(-SpeedMultiplierTest);
        AffectedPlayer.ModifyBackwardSpeed(-SpeedMultiplierTest);
    }
}
