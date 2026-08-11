using UnityEngine;

//SpeedBuffPassive
//example of a passive ability that affects player's movement
namespace HeroShooterMaker.Abilities
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