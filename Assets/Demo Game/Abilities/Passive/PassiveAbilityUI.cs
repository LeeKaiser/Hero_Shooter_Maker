using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
