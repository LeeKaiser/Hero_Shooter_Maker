using UnityEngine;
using PlayerEvents;

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
    }

    void Update()
    {
        if (CanActivate()){
            playerReference.HealHealth(regenAmount, playerReference);
            currentAbilityPause = 1 / Stats.UsePerSec;
            abilityIsPaused = true;
        }
    }

    public override void Cleanup()
    {
        EventBus<PlayerTakeDamage>.Unsubscribe(PauseRecoveryDamage);
        EventBus<UseAbility>.Unsubscribe(PauseRecoveryAbility);
    }
}
