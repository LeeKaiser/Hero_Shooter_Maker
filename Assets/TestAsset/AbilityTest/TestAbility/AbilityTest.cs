using UnityEngine;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;

public class AbilityTest : Ability
{
    public ActiveAbilityID AbilID ;
    //test activation set up
    List<InputOptions.Input> activationInput =
    new List<InputOptions.Input>{
        InputOptions.Input.MoveLShift
    };
    public GameObject SpeedBoostPrefab;

    protected override void Startup()
    {
        AbilID = new ActiveAbilityID();
        manager.SetupInput(this, AbilID, activationInput);
        EventBus<ActiveAbilityID>.Subscribe(executeAbility);
    }

    public void executeAbility(ActiveAbilityID inputEventInfo)
    {
        if (inputEventInfo != AbilID)
        {
            return;
        }

        if (!CanActivate())
        {
            return;
        }

        if (currentCharge <= 0)
        {
            return;
        }

        InterruptReload();
        Debug.Log("Ability1 activated");
        playerReference.GetComponent<StatusEffectManager>().AddNewEffect(SpeedBoostPrefab);
        ConsumeCharge(1);
    }

    public override void Cleanup()
    {
        EventBus<ActiveAbilityID>.Unsubscribe(executeAbility);
    }
}
