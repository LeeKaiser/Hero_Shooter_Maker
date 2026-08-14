using UnityEngine;
using HeroShooterMaker.Character;

namespace HeroShooterMaker.Projectile
{
    public class ApplyDamage : ApplyEffect
    {
        [Tooltip("damage it deals")]
        public int BaseDamage;
        protected override void ActivateEffect(CharCore targetPlayer)
        {
            int damageDealt = (int)(BaseDamage * info.OwningPlayer.GetDamageMult());
            // deal damage to enemy player
            damageDealt = targetPlayer.DealDamage(damageDealt, info.OwningPlayer);
        }

    }
}

