using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;
using AbilityInputEvents;

public class AttackTest : Ability
{
    //public GameObject AttackPrefab;
    
    public GameObject attackPrefab;
    public LayerMask EnemyMask;
    Transform attackPoint;
    Transform targetPoint;
    
    protected override void Startup(){
        EventBus<PlayerStartAttack1>.Subscribe(executeAbility);
        attackPoint = userRef.transform.Find("PlayerArmature").transform.Find("KeyPoint1").transform;
        targetPoint = userRef.transform.Find("TargetPoint").transform;
    }

    public void executeAbility(PlayerStartAttack1 inputEventInfo)
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
        Debug.Log("Attack1 activated");
        
        GameObject attackObj = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
        attackObj.transform.LookAt(targetPoint);

        AttackInfo atkInfo = attackObj.GetComponent<AttackInfo>();
        atkInfo.owningPlayer = userRef;
        atkInfo.attackAllegience = userRef.GetComponent<PlayableCharCore>().playerAllegience;

        ConsumeCharge(1);
    }

    


    public override void Cleanup()
    {
        EventBus<PlayerStartAttack1>.Unsubscribe(executeAbility);
    }
}
