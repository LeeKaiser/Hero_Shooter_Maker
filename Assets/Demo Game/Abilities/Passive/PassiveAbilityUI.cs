using UnityEngine;
using TMPro;
using UnityEngine.UI;

//PassiveAbilityUI
//user interface for passive ability in the demo
namespace HeroShooterMaker.Abilities
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