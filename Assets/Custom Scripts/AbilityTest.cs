using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;

public class AbilityTest : Ability
{
    

    protected override IEnumerator Execute()
    {
        if (CurrentCharge >= 1)
        {
            Debug.Log("used test ability");
            CurrentCharge -= 1;
            RechargeInProgress = true;
            yield return new WaitForSeconds(UseTime);
        }
        else
        {
            Debug.Log("Out of charges");
            yield return new WaitForSeconds(UseFailTime);
        }
    }
}
