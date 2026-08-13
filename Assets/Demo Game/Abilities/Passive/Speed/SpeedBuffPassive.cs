using UnityEngine;
using HeroShooterMaker.Abilities;

//SpeedBuffPassive
//example of a passive ability that affects player's movement
namespace HeroShooterMakerDemo
{
    public class SpeedBuffPassive : Ability
    {
        public float ExtraSpeedMult;

        protected override void Startup()
        {
            playerReference.ModifyForwardSpeed(ExtraSpeedMult);
        }

        public override void Cleanup()
        {
            playerReference.ModifyForwardSpeed(-ExtraSpeedMult);
        }
    }
}