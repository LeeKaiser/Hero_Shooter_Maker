using UnityEngine;

public class SpeedBuffPassive : Ability
{
    public float ExtraSpeedMult;

    protected override void Startup()
    {
        playerReference.ModifyForwardSpeed(ExtraSpeedMult);
    }

    public override void Cleanup()
    {
        playerReference.ModifyForwardSpeed(-ExtraSpeedMult);
    }
}
