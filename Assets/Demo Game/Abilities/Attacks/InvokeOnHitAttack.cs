using UnityEngine;
using HeroShooterMaker.EventBus;

//invokes an event when hitting attack projectile
//assumption: this script is on projectiles that the developer wants to be considered "attack".
public class InvokeOnHitAttack : ApplyEffect
{
    
    protected override void ActivateEffect(CharCore targetPlayer)
    {
        HitTarget target = new HitTarget();
        target.PlayerIdentity = info.OwningPlayer;
        target.TargetPlayer = targetPlayer;
        target.onHit = this;
        EventBus<HitTarget>.Invoke(target);
    }

}

public struct HitTarget
{
    public CharCore PlayerIdentity;
    public CharCore TargetPlayer;
    public InvokeOnHitAttack onHit;

}