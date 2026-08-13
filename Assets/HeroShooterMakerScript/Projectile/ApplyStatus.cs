using UnityEngine;
using HeroShooterMaker.Character;
using HeroShooterMaker.StatusEffects;

namespace HeroShooterMaker.Projectile
{
    public class ApplyStatus : ApplyEffect
    {
        public GameObject Effect;
        protected override void ActivateEffect(CharCore targetPlayer)
        {
            targetPlayer.GetComponent<StatusEffectManager>().AddNewEffect(Effect, info.OwningPlayer);
        }
    }
}