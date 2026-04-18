using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using StarterAssets;

public class DashAbility : ActiveAbility
{
    [Header("CustomVariables")]
    [Tooltip("distance traveled by the dash")]
    public float DashDistance;
    [Tooltip("Distance moved per second")]
    public float DashSpeed;
    [Tooltip("if set to true, player dashes in movement direction, if set to false, dash to look direction")]
    public bool DashMoveDirection = false;

    Transform targetPoint;
    ThirdPersonController playerMovement;
    float durationRemaining;
    private float currentAttackPause = 0;
    protected override void Startup(){
        targetPoint = playerReference.transform.Find("AimTarget").transform;
        EventBus<ActiveAbilityID>.Subscribe(ExecuteAbility);
        playerMovement = playerReference.PlayerMovement;
    }

    public void ExecuteAbility(ActiveAbilityID inputEventInfo)
    {
        //if event is sent by wrong player, do not activate ability
        if (inputEventInfo != AbilityID)
        {
            return;
        }
        //if ability cannot be activated, do not activate ability
        if (!CanActivate())
        {
            return;
        }
        //if attacked is currently limited by attackrate, 
        if (currentAttackPause > 0)
        {
            return;
        }

        manager.NotifyAbilityStarted(this);
        ConsumeCharge(1);
        
        //make player dash towards target direction
        Vector3 direction = (targetPoint.position - playerReference.PlayerArmature.transform.position).normalized;
        if (DashMoveDirection)
        {
            direction = playerMovement.GetCurrentDirection();
        }
        
        durationRemaining = DashDistance / DashSpeed;
        playerMovement.ApplyExternalForce(direction, DashSpeed);
        playerMovement.SetVerticalMovementPause(true);
        playerMovement.SetHorizontalMovementPause(true);
        playerReference.SetGravityMult(-1);

        //limit fire rate
        currentAttackPause = 1 / Stats.UsePerSec;
    }

    // Update is called once per frame
    void Update()
    {
        if (durationRemaining > 0)
        {
            durationRemaining -= Time.deltaTime;
            if (durationRemaining <= 0)
            {
                playerMovement.SetVerticalMovementPause(false);
                playerMovement.SetHorizontalMovementPause(false);
                playerMovement.ApplyExternalForce(Vector3.zero, 0);
                playerMovement.ResetCharacterVelocity();
                playerReference.SetGravityMult(1);
                manager.NotifyAbilityEnded(this);
            }
        }
        if (currentAttackPause > 0)
        {
            currentAttackPause -= Time.deltaTime;
        }
    }

    public override void Cleanup()
    {
        EventBus<ActiveAbilityID>.Unsubscribe(ExecuteAbility);
    }
}
