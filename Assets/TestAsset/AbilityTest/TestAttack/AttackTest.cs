using UnityEngine;
using PlayerEvents;

public class AttackTest : ActiveAbility
{
    [Header("Custom variables")]
    public GameObject attackPrefab;
    
    Transform attackPoint;
    Transform targetPoint;

    
    
    protected override void Startup(){
        attackPoint = playerReference.PlayerArmature.transform.Find("KeyPoint1").transform;
        targetPoint = playerReference.transform.Find("AimTarget").transform;
        EventBus<ActiveAbilityID>.Subscribe(executeAbility);
        SetUpInput();
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

        InterruptReload();
        
        //instantiate projectile
        GameObject attackObj = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
        attackObj.transform.LookAt(targetPoint);

        //set projectile settings
        ProjectileInfo atkInfo = attackObj.GetComponent<ProjectileInfo>();
        atkInfo.OwningPlayer = playerReference;
        atkInfo.AttackAllegience = playerReference.PlayerAllegience;

        //use a charge
        ConsumeCharge(1);

        //invoke used ability
        UseAbility usedAbilEvent = new UseAbility();
        usedAbilEvent.PlayerIdentity = playerReference;
        usedAbilEvent.UsedAbility = this;
        EventBus<UseAbility>.Invoke(usedAbilEvent);
    }


    public override void Cleanup()
    {
        EventBus<ActiveAbilityID>.Unsubscribe(executeAbility);
    }
}
