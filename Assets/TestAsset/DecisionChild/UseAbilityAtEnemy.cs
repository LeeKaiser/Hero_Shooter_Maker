using UnityEngine;
using AbilityClassification;
using System.Collections.Generic;
using InputOptions;
using System;
[CreateAssetMenu(menuName = "AIAction/AbilityAtEnemy")]
public class UseAbilityAtEnemy : PatrolAction
{
    /*
    public Transform MoveTarget;
    public Transform AimTarget;
    public ObjectDetection Detection;
    public InputEventCaller InputCall;

    public ActiveAbility abilityToUse = null;
    protected InputUnit abilityInput = null;
    protected float inputHoldTime;
    public bool HoldingInput = false; 
    */

    public AbilityClass PerferedAbilityClass;

    
    protected float distanceFromEnemy = 12f;
    protected float randomAngleTweak = 10f;
    protected float randomDistanceTweak = 2f;

    
    public override void DetermineMovement()
    {
        if (!(Detection.GetCurrentContext().KnownEnemyList == null))
        {
            // identify weakest enemy
            float highestVuln = 0;
            foreach (KeyValuePair<CharCore, PlayerSummary> potentialTarget in Detection.GetCurrentContext().KnownEnemyList)
            {
                if (potentialTarget.Value.VulnerabilityValue >= highestVuln)
                {
                    targetPlayer = potentialTarget.Key.PlayerArmature;
                    highestVuln = potentialTarget.Value.VulnerabilityValue;
                }
            }
            

            //set distance based on ability to use
            if (abilityToUse != null)
            {
                distanceFromEnemy = abilityToUse.MinimumRange + ((abilityToUse.MaximumRange - abilityToUse.MinimumRange) / 2);
                randomDistanceTweak = abilityToUse.MaximumRange - abilityToUse.MinimumRange;
            }

            try
            {
                Vector3 nextDestination = targetPlayer.transform.position;

                Vector3 enemyToSelf =  playerArmature.transform.position - targetPlayer.transform.position;
                Quaternion randomRot = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomAngleTweak,randomAngleTweak),Vector3.up);
                nextDestination = nextDestination + (randomRot * enemyToSelf.normalized * (distanceFromEnemy + UnityEngine.Random.Range(-randomDistanceTweak,randomDistanceTweak)));
                MoveTarget.position = nextDestination;
                Movement.MoveToLocation();
            }
            //Debug.Log(nextDestination);
            catch (Exception e)
            {
                base.DetermineMovement();
                Debug.Log(e);
            }
            
        }
        else
        {
            base.DetermineMovement();
        }
    }
    public override void DetermineAim()
    {
        try
        {
            Vector3 targetPosition = targetPlayer.transform.position;
            float heightAdjustment = targetPlayer.GetComponent<CharacterController>().height * 0.8f;
            targetPosition.y += heightAdjustment;
            AimTarget.position = targetPosition;
        }

        catch(Exception e)
        {
            base.DetermineAim();
            Debug.Log(e);
        }
        
    }
    public override void DetermineInput()
    {
        if (abilityToUse == null)
        {
            //attempt to shoot a damage attack

            //get the attack from the self's ability manager
            AbilityManager abilManager = Detection.GetCurrentContext().SelfSummary.SummarizedPlayer.GetComponent<AbilityManager>();
            //abilToUse = null;
            float bestCooldown = 0;
            foreach (Ability abil in abilManager.GetAbilList())
            {
                //assumes that active abilities have active ability classification and has input tied to it
                if (abil.CurrentAbilClass.HasFlag(AbilityClass.Active) && abil.CurrentAbilClass.HasFlag(PerferedAbilityClass))
                {
                    float abilCooldown = abil.GetCurrentCharge() / abil.GetCurrentMaxCharge();
                    if (abilCooldown > bestCooldown && abilManager.AbiltyToInputDictionary.ContainsKey(abil))
                    {
                        
                        abilityToUse = (ActiveAbility)abil;
                        abilityInput = abilManager.AbiltyToInputDictionary[abil];
                        switch (abilityInput.ComboInputType)
                        {
                            case InputType.Hold:
                                inputHoldTime = 99f;
                                break;
                            case InputType.Release:
                                inputHoldTime = abilityToUse.Stats.UsePerSec;
                                break;
                            default:
                                inputHoldTime = 0.2f;
                                break;
                        }
                    }
                }
                
            }
        }
        
    }

    
}
