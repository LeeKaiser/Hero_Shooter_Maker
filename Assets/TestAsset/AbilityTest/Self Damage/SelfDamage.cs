using UnityEngine;

namespace HeroShooterMaker.Abilities
{
    public class SelfDamage : ActiveAbility
    {
        [Header("Custom variables")]
        public int damage ;
        
        protected override void Startup(){
            EventBus<ActiveAbilityID>.Subscribe(executeAbility);
            SetUpInput();
        }

        public void executeAbility(ActiveAbilityID inputEventInfo)
        {
            //if event is sent by wrong player, do not activate ability
            if (inputEventInfo != AbilityID)
            {
                return;
            }
            //if ability cannot be activated, do not activate ability
            if (!CanActivate())
            {
                return;
            }
            //if no charge remaining, do not activate ability
            if (currentCharge <= 0)
            {
                return;
            }

            InterruptReload();
            
            playerReference.DealDamage(damage, playerReference);

            //use a charge
            ConsumeCharge(1);
        }


        public override void Cleanup()
        {
            EventBus<ActiveAbilityID>.Unsubscribe(executeAbility);
        }
    }
}