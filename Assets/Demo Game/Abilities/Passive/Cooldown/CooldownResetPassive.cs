using UnityEngine;
using HeroShooterMaker.CharacterEvents;
using System.Collections.Generic;

namespace HeroShooterMaker.Abilities
{
    //CooldownResetPassive
    //example of a passive ability that activates when defeating an opponent
    //example of an ability that affects other abilities.
    public class CooldownResetPassive : Ability
    {
        public float RechargePercent;

        protected override void Startup()
        {
            EventBus<PlayerDead>.Subscribe(executeAbility);
        }

        public void executeAbility(PlayerDead deadInfo)
        {
            if (deadInfo.PlayerKiller != playerReference)
            {
                return;
            }

            List<Ability> abilList = manager.GetAbilList();
            foreach (Ability abil in abilList)
            {
                if (abil.CurrentAbilClass.HasFlag(AbilityClass.Active))
                {
                    float chargeAmount = (abil.Stats.MaxCharge / abil.Stats.ChargeGainPerFullRecharge) * 
                        abil.Stats.ChargePointsRequired * RechargePercent;
                    abil.RecoverChargePoint(chargeAmount);
                }
            }
        }

        public override void Cleanup()
        {
            EventBus<PlayerDead>.Unsubscribe(executeAbility);
        }
    }
}

