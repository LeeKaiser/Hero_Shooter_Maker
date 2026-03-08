using UnityEngine;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;

public class AbilityTest : Ability
{
    PlayerActiveAbilID attackInput ;
    //test activation set up
    List<InputOptions.Input> activationInput =
    new List<InputOptions.Input>{
        InputOptions.Input.MoveLShift
    };
    public GameObject SpeedBoostPrefab;

    protected override void Startup()
    {
        attackInput = new PlayerActiveAbilID();
        playerRef.transform.Find("PlayerArmature").GetComponent<InputReader>().InputDict.Add(activationInput, attackInput);
        EventBus<PlayerActiveAbilID>.Subscribe(executeAbility);
    }

    public void executeAbility(PlayerActiveAbilID inputEventInfo)
    {
        if (inputEventInfo != attackInput)
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
