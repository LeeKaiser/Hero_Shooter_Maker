using UnityEngine;
using HeroShooterMaker.EventBus;
using HeroShooterMaker.Abilities;
using HeroShooterMaker.Projectile;


//HealerPassive
//makes the attack heal teammates when hitting them
//example of a passive ability that heals a certain player
namespace HeroShooterMakerDemo
{
    public class HealerPassive : Ability
    {
        public float HealPercentage;

        protected override void Startup()
        {
            EventBus<HitTarget>.Subscribe(executeAbility);
        }

        public void executeAbility(HitTarget hitTarget)
        {
            if (hitTarget.PlayerIdentity != playerReference)
            {
                return;
            }
            if (hitTarget.TargetPlayer.PlayerAllegience != playerReference.PlayerAllegience)
            {
                return;
            }


            //heal target player
            int healAmount = (int)(hitTarget.onHit.GetComponent<ApplyDamage>().BaseDamage * HealPercentage);
            hitTarget.TargetPlayer.HealHealth(healAmount,playerReference);
            
        }

        public override void Cleanup()
        {
            EventBus<HitTarget>.Unsubscribe(executeAbility);
        }
    }
}