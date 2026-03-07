using UnityEngine;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;

public class AbilityTest : Ability
{
    public class AbilTestEventType : PlayerEventInfo
    {
        public AbilTestEventType(PlayableCharCore playerRef) : base(playerRef){}
    }
    
    AbilTestEventType attackInput ;
    //test activation set up
    List<InputOptions.Input> activationInput =
    new List<InputOptions.Input>{
        InputOptions.Input.MoveLShift
    };
    public GameObject SpeedBoostPrefab;

    protected override void Startup()
    {
        attackInput = new AbilTestEventType(playerRef);
        playerRef.transform.Find("PlayerArmature").GetComponent<InputReader>().InputDict.Add(activationInput, attackInput);
        EventBus<AbilTestEventType>.Subscribe(executeAbility);
    }

    public void executeAbility(AbilTestEventType inputEventInfo)
    {
        if (inputEventInfo.PlayerIdentity != playerRef)
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
        EventBus<AbilTestEventType>.Unsubscribe(executeAbility);
    }
}
