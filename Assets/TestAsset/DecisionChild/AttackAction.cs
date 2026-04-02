using UnityEngine;
using AbilityClassification;
using System.Collections.Generic;
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

            Debug.Log(targetEnemy);
            Vector3 nextDestination = targetEnemy.transform.position;

            Vector3 enemyToSelf =  Detection.GetCurrentContext().SelfSummary.SummarizedPlayer.transform.position - targetEnemy.transform.position;
            Quaternion randomRot = Quaternion.AngleAxis(Random.Range(-randomAngleTweak,randomAngleTweak),Vector3.up);
            nextDestination = nextDestination + (randomRot * enemyToSelf.normalized * (distanceFromEnemy + Random.Range(-randomDistanceTweak,randomDistanceTweak)));
            //Debug.Log(nextDestination);
            MoveTarget.position = nextDestination;
        }
    }
    public override void DetermineAim()
    {
        Vector3 targetPosition = targetEnemy.transform.position;
        float heightAdjustment = targetEnemy.GetComponent<CharacterController>().height * 0.8f;
        targetPosition.y += heightAdjustment;
        AimTarget.position = targetPosition;
    }
    public override void MakeInput()
    {
        //attempt to shoot a damage attack

        //get the attack from the self's ability manager
        AbilityManager abilManager = Detection.GetCurrentContext().SelfSummary.SummarizedPlayer.GetComponent<AbilityManager>();
        Ability abilToUse = null;
        float bestCooldown = 0;
        foreach (Ability abil in abilManager.GetAbilList())
        {
            //assumes that active abilities have active ability classification and has input tied to it
            if (abil.CurrentAbilClass.HasFlag(AbilityClass.Damage) && abil.CurrentAbilClass.HasFlag(AbilityClass.Damage))
            {
                float abilCooldown = abil.GetCurrentCharge() / abil.GetCurrentMaxCharge();
                if (abilCooldown > bestCooldown)
                {
                    abilToUse = abil;
                }
            }
            
        }
        
        //if there is an ability available, call the input for it
        //TODO: rework to use correct input type
        if (!(abilToUse == null))
        {
            
            InputCall.AddHoldInput(abilManager.AbiltyToInputDictionary[abilToUse].InputCombo);
            
        }
    }
}
