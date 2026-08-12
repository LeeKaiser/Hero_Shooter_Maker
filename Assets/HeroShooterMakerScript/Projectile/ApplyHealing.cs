using UnityEngine;
using HeroShooterMaker.Character;
public class ApplyHealing : ApplyEffect
{
    [Tooltip("amount of healing applied")]
    public int BaseHealing;
    
    protected override void ActivateEffect(CharCore targetPlayer)
    {
        int healthHealed = (int) (BaseHealing /** info.OwningPlayer.GetDamageMult()*/);
        // heal ally
        healthHealed = targetPlayer.HealHealth(healthHealed, info.OwningPlayer);
    }
}
