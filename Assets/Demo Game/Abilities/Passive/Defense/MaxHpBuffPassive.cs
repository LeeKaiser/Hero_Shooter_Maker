using UnityEngine;

public class MaxHpBuffPassive : Ability
{
    public int ExtraMaxHP;

    protected override void Startup()
    {
        playerReference.ModifyMaxHealth(ExtraMaxHP);
    }

    public override void Cleanup()
    {
        playerReference.ModifyMaxHealth(-ExtraMaxHP);
    }
}
