using UnityEngine;

//PoisonPassive
//Example of passive ability that causes a certain ability to apply a debuff
namespace HeroShooterMaker.Abilities
{
    public class PoisonPassive : Ability
    {
        public GameObject PoisonEffect;

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
            if (hitTarget.TargetPlayer.PlayerAllegience == playerReference.PlayerAllegience)
            {
                return;
            }


            //poison target player
            StatusEffectManager enemyEffectManager = hitTarget.TargetPlayer.GetComponent<StatusEffectManager>();

            enemyEffectManager.AddNewEffect(PoisonEffect, playerReference);
            
        }

        public override void Cleanup()
        {
            EventBus<HitTarget>.Unsubscribe(executeAbility);
        }
    }
}