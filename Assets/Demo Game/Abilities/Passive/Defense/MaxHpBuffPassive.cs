using UnityEngine;

//MaxHpBuffPassive
//example of a passive that changes player's stats
namespace HeroShooterMaker.Abilities
{
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
}