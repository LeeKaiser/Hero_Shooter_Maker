using UnityEngine;
using TMPro;
using UnityEngine.UI;
using HeroShooterMaker.Abilities;

//PassiveAbilityUI
//user interface for passive ability in the demo
namespace HeroShooterMakerDemo
{
    public class PassiveAbilityUI : AbilityUI
    {
        public TMP_Text abilityName;

        public override void Initialize()
        {
            if (AbilityReference == null)
            {
                return;
            }
            abilityName.text = AbilityReference.Stats.AbilityName;
            
        }

        public override void UpdateUI(){}

    }
}