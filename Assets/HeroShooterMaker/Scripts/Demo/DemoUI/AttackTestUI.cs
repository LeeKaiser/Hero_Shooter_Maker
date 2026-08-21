using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Attack Test UI
//example use of AbilityUI class
namespace HeroShooterMaker.Abilities
{
    public class AttackTestUI : AbilityUI
    {
        public TMP_Text abilityName;
        public TMP_Text chargeRemaining;
        public TMP_Text maxCharge;
        public Slider chargeProgress;

        

        public override void Initialize()
        {
            if (AbilityReference == null)
            {
                return;
            }
            abilityName.text = AbilityReference.Stats.AbilityName;
            chargeRemaining.text = AbilityReference.GetCurrentCharge() + "";
            maxCharge.text = AbilityReference.Stats.MaxCharge + "";
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
