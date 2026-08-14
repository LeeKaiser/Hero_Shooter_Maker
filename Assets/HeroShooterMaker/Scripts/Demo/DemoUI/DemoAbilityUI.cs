using UnityEngine;
using TMPro;
using UnityEngine.UI;

//DemoAbilityUI
//user interface for active ability in the demo
namespace HeroShooterMaker.Abilities
{
    public class DemoAbilityUI : AbilityUI
    {
        public TMP_Text abilityName;
        public TMP_Text chargeRemaining;
        public Slider chargeProgress;

        public override void Initialize()
        {
            if (AbilityReference == null)
            {
                return;
            }
            abilityName.text = AbilityReference.Stats.AbilityName;
            chargeRemaining.text = AbilityReference.GetCurrentCharge() + "";
            chargeProgress.maxValue = AbilityReference.Stats.ChargePointsRequired;
            chargeProgress.value = AbilityReference.GetChargePointProgress();
            
        }

        public override void UpdateUI()
        {
            chargeRemaining.text = AbilityReference.GetCurrentCharge() + "";
            chargeProgress.value = AbilityReference.GetChargePointProgress();
        }

    }
}