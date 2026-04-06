using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AttackTest : ActiveAbility
{
    [Header("Custom variables")]
    public GameObject attackPrefab;
    
    Transform attackPoint;
    Transform targetPoint;

    private float currentAttackPause = 0;
    
    protected override void Startup(){
        attackPoint = playerReference.PlayerArmature.transform.Find("KeyPoint1").transform;
        targetPoint = playerReference.transform.Find("AimTarget").transform;
        EventBus<ActiveAbilityID>.Subscribe(executeAbility);
    }

    public void executeAbility(ActiveAbilityID inputEventInfo)
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
        //if no charge remaining, do not activate ability
        if (currentCharge <= 0)
        {
            return;
        }
        //if attacked is currently limited by attackrate, 
        if (currentAttackPause > 0)
        {
            return;
        }

        InterruptReload();

        //limit fire rate
        currentAttackPause = 1 / Stats.UsePerSec;
        
        //instantiate projectile
        GameObject attackObj = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
        attackObj.transform.LookAt(targetPoint);

        //set projectile settings
        ProjectileInfo atkInfo = attackObj.GetComponent<ProjectileInfo>();
        atkInfo.OwningPlayer = playerReference;
        atkInfo.AttackAllegience = playerReference.PlayerAllegience;

        //use a charge
        ConsumeCharge(1);
    }

    void Update()
    {
        if (currentAttackPause > 0)
        {
            currentAttackPause -= Time.deltaTime;
        }
        
    }


    public override void Cleanup()
    {
        EventBus<ActiveAbilityID>.Unsubscribe(executeAbility);
    }
}
