using UnityEngine;
using HeroShooterMaker.StatusEffects;

namespace HeroShooterMakerDemo
{

    public class PoisonEffect : StatusEffect
    {
        [Tooltip("damage dealt by poison per interval")]
        public int PoisonDamage;
        [Tooltip("Amount of interval per second")]
        public int HitPerSec;

        float interval;

        public override void ApplyEffect()
        {
            Active = true;
            RemainingDuration = Stats.EffectDuration;
            interval = 1f / HitPerSec;
        }

        void Update()
        {
            interval -= Time.deltaTime;
            if (interval <= 0)
            {
                interval = 1f / HitPerSec;
                AffectedPlayer.DealDamage(PoisonDamage, OwningPlayer);
            }
        }

        protected override void RemoveEffect()
        {
            Active = false;
        }
    }
}