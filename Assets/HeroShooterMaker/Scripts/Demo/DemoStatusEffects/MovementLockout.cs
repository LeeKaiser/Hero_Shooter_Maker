using UnityEngine;
using HeroShooterMaker.AI;
using HeroShooterMaker.StatusEffects;

namespace HeroShooterMakerDemo
{

    public class MovementLockout : StatusEffect
    {
        public override void ApplyEffect()
        {
            Active = true;
            RemainingDuration = Stats.EffectDuration;
            AffectedPlayer.PlayerMovement.SetHorizontalMovementPause(true);

            AIMovement aimovement = AffectedPlayer.PlayerArmature.GetComponent<AIMovement>();


        }

        protected override void RemoveEffect()
        {
            Active = false;
            AffectedPlayer.PlayerMovement.SetHorizontalMovementPause(false);
        }
    }
}