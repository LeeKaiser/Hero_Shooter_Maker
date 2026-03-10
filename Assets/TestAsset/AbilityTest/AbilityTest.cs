using UnityEngine;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;

public class AbilityTest : Ability
{
    public PlayerActiveAbilID AbilID ;
    //test activation set up
    List<InputOptions.Input> activationInput =
    new List<InputOptions.Input>{
        InputOptions.Input.MoveLShift
    };
    public GameObject SpeedBoostPrefab;

    protected override void Startup()
    {
        AbilID = new PlayerActiveAbilID();
        manager.SetupInput(this, AbilID, activationInput);
        EventBus<PlayerActiveAbilID>.Subscribe(executeAbility);
    }

    public void executeAbility(PlayerActiveAbilID inputEventInfo)
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
        playerRef.GetComponent<StatusEffectManager>().AddNewEffect(SpeedBoostPrefab);
        ConsumeCharge(1);
    }

    public override void Cleanup()
    {
        EventBus<PlayerActiveAbilID>.Unsubscribe(executeAbility);
    }
}
