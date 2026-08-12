using UnityEngine;
using HeroShooterMaker.CharacterEvents;
using HeroShooterMaker.Controls;
using HeroShooterMaker.EventBus;

//RegenPassive
//player regenerates health after avoiding taking damage or using abilities for few seconds
//example of a passive ability that stops itself based on events rather than being activated by it
namespace HeroShooterMaker.Abilities
{
    public class RegenPassive : Ability
    {
        public int regenAmount;
        protected override void Startup()
        {
            EventBus<PlayerTakeDamage>.Subscribe(PauseRecoveryDamage);
            EventBus<UseAbility>.Subscribe(PauseRecoveryAbility);
        }

        public void PauseRecoveryDamage(PlayerTakeDamage takeDamage)
        {
            if (takeDamage.PlayerIdentity != playerReference)
            {
                return;
            }

            StopRecovery();
        }

        public void PauseRecoveryAbility(UseAbility useAbil)
        {
            if (useAbil.PlayerIdentity != playerReference)
            {
                return;
            }

            StopRecovery();
        }

        void StopRecovery()
        {
            InterruptReload();
            ConsumeCharge(1);
            playerReference.MovementStyle = MovementStyles.MovementStyle.AlwaysFaceForward;
        }

        void Update()
        {
            if (CanActivate()){
                playerReference.HealHealth(regenAmount, playerReference);
                currentAbilityPause = 1 / Stats.UsePerSec;
                abilityIsPaused = true;
                playerReference.MovementStyle = MovementStyles.MovementStyle.FaceMovement;
            }
        }

        public override void Cleanup()
        {
            EventBus<PlayerTakeDamage>.Unsubscribe(PauseRecoveryDamage);
            EventBus<UseAbility>.Unsubscribe(PauseRecoveryAbility);
        }
    }
}