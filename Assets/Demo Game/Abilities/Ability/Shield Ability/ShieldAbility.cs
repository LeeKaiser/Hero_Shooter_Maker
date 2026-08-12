using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using HeroShooterMaker.CharacterEvents;
using HeroShooterMaker.Controls;

//ShieldAbility
//example of an ability that applies a status effect to self
namespace HeroShooterMaker.Abilities
{
    public class ShieldAbility : ActiveAbility
    {
        
        public GameObject ShieldPrefab;

        protected override void Startup()
        {
            EventBus<ActiveAbilityID>.Subscribe(executeAbility);
            SetUpInput();
        }

        public void executeAbility(ActiveAbilityID inputEventInfo)
        {
            if (inputEventInfo != AbilityID)
            {
                return;
            }

            if (!CanActivate())
            {
                return;
            }

            if (currentCharge <= 0)
            {
                return;
            }

            InterruptReload();
            playerReference.GetComponent<StatusEffectManager>().AddNewEffect(ShieldPrefab, playerReference);
            ConsumeCharge(1);

            //invoke used ability
            UseAbility usedAbilEvent = new UseAbility();
            usedAbilEvent.PlayerIdentity = playerReference;
            usedAbilEvent.UsedAbility = this;
            EventBus<UseAbility>.Invoke(usedAbilEvent);
        }

        public override void Cleanup()
        {
            EventBus<ActiveAbilityID>.Unsubscribe(executeAbility);
        }
    }
}