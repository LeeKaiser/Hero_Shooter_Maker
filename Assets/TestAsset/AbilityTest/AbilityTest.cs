using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;
using AbilityInputEvents;

public class AbilityTest : Ability
{
    public GameObject SpeedBoostPrefab;

    protected override void Startup(){
        EventBus<PlayerStartAbility1>.Subscribe(executeAbility);
    }

    public void executeAbility(PlayerStartAbility1 inputEventInfo)
    {
        if (inputEventInfo.PlayerIdentity != userRef)
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
        userRef.GetComponent<StatusEffectManager>().AddNewEffect(SpeedBoostPrefab);
        ConsumeCharge(1);
    }

    public override void Cleanup()
    {
        EventBus<PlayerStartAbility1>.Unsubscribe(executeAbility);
    }
}
