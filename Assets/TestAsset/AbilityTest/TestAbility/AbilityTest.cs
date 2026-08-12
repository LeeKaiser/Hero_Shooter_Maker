using UnityEngine;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;
using HeroShooterMaker.Controls;
using HeroShooterMaker.EventBus;

namespace HeroShooterMaker.Abilities
{
    public class AbilityTest : ActiveAbility
    {
        
        public GameObject SpeedBoostPrefab;

        protected override void Startup()
        {
            EventBus<ActiveAbilityID>.Subscribe(executeAbility);
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
            playerReference.GetComponent<StatusEffectManager>().AddNewEffect(SpeedBoostPrefab, playerReference);
            ConsumeCharge(1);
        }

        public override void Cleanup()
        {
            EventBus<ActiveAbilityID>.Unsubscribe(executeAbility);
        }
    }
}