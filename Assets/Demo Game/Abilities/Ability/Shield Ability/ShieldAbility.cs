using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShieldAbility : ActiveAbility
{
    
    public GameObject ShieldPrefab;

    protected override void Startup()
    {
        EventBus<ActiveAbilityID>.Subscribe(executeAbility);
    }

    public void executeAbility(ActiveAbilityID inputEventInfo)
    {
        if (inputEventInfo != AbilityID)
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
        playerReference.GetComponent<StatusEffectManager>().AddNewEffect(ShieldPrefab);
        ConsumeCharge(1);
    }

    public override void Cleanup()
    {
        EventBus<ActiveAbilityID>.Unsubscribe(executeAbility);
    }
}
