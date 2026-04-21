using UnityEngine;

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
