using UnityEngine;
using HeroShooterMaker.Character;

public class ApplyDisplacement : ApplyEffect
{
    public Knockback DisplacementEffect;
    protected override void ActivateEffect(CharCore targetPlayer)
    {
        DisplacementEffect.sourcePosition = transform.position;
        targetPlayer.GetComponent<StatusEffectManager>().AddNewEffect(DisplacementEffect.gameObject, info.OwningPlayer);

    }
}
