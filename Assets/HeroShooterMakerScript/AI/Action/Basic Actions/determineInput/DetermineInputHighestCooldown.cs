using UnityEngine;
using AbilityClassification;
using System.Collections.Generic;
using InputOptions;
using System;

[CreateAssetMenu(fileName = "InputHighestCd", menuName = "AIAction/Input/InputHighestCd")]
public class DetermineInputHighestCooldown : DetermineInput
{
    public override void ExecuteDetermineInput(AIAction action)
    {
        if (action.abilityToUse == null)
        {
            //attempt to shoot a damage attack

            //get the attack from the self's ability manager
            AbilityManager abilManager = action.Detection.GetCurrentContext().SelfSummary.SummarizedPlayer.GetComponent<AbilityManager>();
            //abilToUse = null;
            float bestCooldown = 0;
            foreach (Ability abil in abilManager.GetAbilList())
            {
                //assumes that active abilities have active ability classification and has input tied to it
                if (abil.CurrentAbilClass.HasFlag(AbilityClass.Active) && abil.CurrentAbilClass.HasFlag(PerferedAbilityClass))
                {
                    float abilCooldown = abil.GetCurrentCharge() / abil.GetCurrentMaxCharge();
                    if (abilCooldown > bestCooldown && abilManager.AbiltyToInputDictionary.ContainsKey(abil))
                    {
                        
                        action.abilityToUse = (ActiveAbility)abil;
                        action.abilityInput = abilManager.AbiltyToInputDictionary[abil];
                        switch (action.abilityInput.ComboInputType)
                        {
                            case InputType.Hold:
                                action.inputHoldTime = 99f;
                                break;
                            case InputType.Release:
                                action.inputHoldTime = action.abilityToUse.Stats.UsePerSec;
                                break;
                            default:
                                action.inputHoldTime = 0.2f;
                                break;
                        }
                    }
                }
                
            }
        }
    }
}
