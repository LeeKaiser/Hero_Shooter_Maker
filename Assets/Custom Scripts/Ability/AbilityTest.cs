using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;

public class AbilityTest : Ability
{
    protected override IEnumerator Execute()
    {
        if (currentCharge > 0)
        {
            Debug.Log("Ability tapped or held.");
            currentCharge -= 1;
        }
        InterruptReload();
        yield return new WaitForSeconds(useTime);
    }

    protected override IEnumerator ExecuteReleased(float chargeRatio)
    {
        Debug.Log($"Ability released with charge: {chargeRatio:F2}");
        currentCharge -= 1;
        yield return null;
    }

    void Update()
    {
        if (currentCharge < maxCharge && !isActive)
        {
            rechargeInProgress = true;
        }
        RecoverChargePoint(Time.deltaTime); //recharge every tick if possible
    }
}
