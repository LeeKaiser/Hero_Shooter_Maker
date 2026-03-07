using UnityEngine;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;

public class AttackTest : Ability
{
    public class AttackTestEventType : PlayerEventInfo
    {
        public AttackTestEventType(PlayableCharCore playerRef):base(playerRef){}
    }
    AttackTestEventType attackInput ;
    //test activation set up
    List<InputOptions.Input> activationInput =
    new List<InputOptions.Input>{
        InputOptions.Input.AtkL
    };
    public GameObject attackPrefab;
    public LayerMask EnemyMask;
    Transform attackPoint;
    Transform targetPoint;
    public float fireRate; //custom fire rate in shots per sec

    private float currentAttackPause = 0;
    
    protected override void Startup(){
        attackInput = new AttackTestEventType(playerRef);
        attackPoint = playerRef.transform.Find("PlayerArmature").transform.Find("KeyPoint1").transform;
        targetPoint = playerRef.transform.Find("TargetPoint").transform;
        playerRef.transform.Find("PlayerArmature").GetComponent<InputReader>().InputDict.Add(activationInput, attackInput);
        EventBus<AttackTestEventType>.Subscribe(executeAbility);
    }

    public void executeAbility(AttackTestEventType inputEventInfo)
    {
        //if event is sent by wrong player, do not activate ability
        if (inputEventInfo.PlayerIdentity != playerRef)
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
        Debug.Log("Attack1 activated");

        //limit fire rate
        currentAttackPause = 1 / fireRate;
        
        //instantiate projectile
        GameObject attackObj = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
        attackObj.transform.LookAt(targetPoint);

        //set projectile settings
        AttackInfo atkInfo = attackObj.GetComponent<AttackInfo>();
        atkInfo.owningPlayer = playerRef;
        atkInfo.attackAllegience = playerRef.playerAllegience;

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
        EventBus<AttackTestEventType>.Unsubscribe(executeAbility);
    }
}
