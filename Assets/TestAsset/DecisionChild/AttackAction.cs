using UnityEngine;
using AbilityClassification;
using System.Collections.Generic;
using InputOptions;
[CreateAssetMenu(menuName = "AIAction/Attack")]
public class AttackAction : AIAction
{
    /*
    public Transform MoveTarget;
    public Transform AimTarget;
    public ObjectDetection Detection;
    public InputEventCaller InputCall;
    */

    GameObject targetEnemy = null;
    float distanceFromEnemy = 12f;
    float randomAngleTweak = 10f;
    float randomDistanceTweak = 2f;

    
    public override void DetermineMovement()
    {
        if (!(Detection.GetCurrentContext().KnownEnemyList == null))
        {
            // identify weakest enemy
            if (targetEnemy == null)
            {
                float highestVuln = 0;
                foreach (KeyValuePair<CharCore, PlayerSummary> potentialTarget in Detection.GetCurrentContext().KnownEnemyList)
                {
                    if (potentialTarget.Value.VulnerabilityValue >= highestVuln)
                    {
                        targetEnemy = potentialTarget.Key.PlayerArmature;
                        highestVuln = potentialTarget.Value.VulnerabilityValue;
                    }
                }
            }

            Vector3 nextDestination = targetEnemy.transform.position;

            Vector3 enemyToSelf =  Detection.GetCurrentContext().SelfSummary.SummarizedPlayer.transform.position - targetEnemy.transform.position;
            Quaternion randomRot = Quaternion.AngleAxis(Random.Range(-randomAngleTweak,randomAngleTweak),Vector3.up);
            nextDestination = nextDestination + (randomRot * enemyToSelf.normalized * (distanceFromEnemy + Random.Range(-randomDistanceTweak,randomDistanceTweak)));
            //Debug.Log(nextDestination);
            MoveTarget.position = nextDestination;
            Movement.MoveToLocation();
        }
    }
    public override void DetermineAim()
    {
        Vector3 targetPosition = targetEnemy.transform.position;
        float heightAdjustment = targetEnemy.GetComponent<CharacterController>().height * 0.8f;
        targetPosition.y += heightAdjustment;
        AimTarget.position = targetPosition;
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
                if (abil.CurrentAbilClass.HasFlag(AbilityClass.Active) && abil.CurrentAbilClass.HasFlag(AbilityClass.Damage))
                {
                    float abilCooldown = abil.GetCurrentCharge() / abil.GetCurrentMaxCharge();
                    if (abilCooldown > bestCooldown && abilManager.AbiltyToInputDictionary.ContainsKey(abil))
                    {
                        
                        abilityToUse = abil;
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
