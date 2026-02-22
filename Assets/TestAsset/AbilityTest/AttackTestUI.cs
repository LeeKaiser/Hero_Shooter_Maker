using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttackTestUI : AbilityUI
{
    public TMP_Text abilityName;
    public TMP_Text chargeRemaining;
    public TMP_Text maxCharge;
    public Slider chargeProgress;

    public override void Initialize()
    {
        if (abilityRef == null)
        {
            return;
        }
        abilityName.text = abilityRef.abilityStat.abilityName;
        chargeRemaining.text = abilityRef.GetCurrentCharge() + "";
        maxCharge.text = abilityRef.abilityStat.maxCharge + "";
        chargeProgress.maxValue = abilityRef.abilityStat.chargePointsRequired;
        chargeProgress.value = abilityRef.GetChargePointProgress();
        
    }

    public override void UpdateUI()
    {
        chargeRemaining.text = abilityRef.GetCurrentCharge() + "";
        chargeProgress.value = abilityRef.GetChargePointProgress();
    }
}
