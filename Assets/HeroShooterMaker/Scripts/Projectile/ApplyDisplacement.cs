using UnityEngine;
using HeroShooterMaker.Character;
using HeroShooterMaker.StatusEffects;
using HeroShooterMakerDemo; //reference knockback status effect

namespace HeroShooterMaker.Projectile
{
    public class ApplyDisplacement : ApplyEffect
    {
        public Knockback DisplacementEffect;
        protected override void ActivateEffect(CharCore targetPlayer)
        {
            DisplacementEffect.sourcePosition = transform.position;
            targetPlayer.GetComponent<StatusEffectManager>().AddNewEffect(DisplacementEffect.gameObject, info.OwningPlayer);

        }
    }
}