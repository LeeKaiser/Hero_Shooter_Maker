using UnityEngine;
using AbilityClassification;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "AIAction/Attack")]
public class AttackAction : AIAction
{
    /*
    public Transform movementDestination;
    public Transform aimTarget;
    public ObjectDetection objectDetection;
    public InputEventCaller inputCall;
    */

    GameObject targetEnemy = null;
    float distanceFromEnemy = 12f;
    float randomAngleTweak = 10f;
    float randomDistanceTweak = 2f;
    public override void DetermineMovement()
    {
        if (!(objectDetection.GetCurrentContext().knownEnemyList == null))
        {
            // identify weakest enemy
            if (targetEnemy == null)
            {
                float highestVuln = 0;
                foreach (KeyValuePair<CharCore, PlayerSummary> potentialTarget in objectDetection.GetCurrentContext().knownEnemyList)
                {
                    if (potentialTarget.Value.vulnValue >= highestVuln)
                    {
                        targetEnemy = potentialTarget.Key.playerArmature;
                        highestVuln = potentialTarget.Value.vulnValue;
                    }
                }
            }

            Debug.Log(targetEnemy);
            Vector3 nextDestination = targetEnemy.transform.position;

            Vector3 enemyToSelf =  objectDetection.GetCurrentContext().selfSummary.summarizedPlayer.transform.position - targetEnemy.transform.position;
            Quaternion randomRot = Quaternion.AngleAxis(Random.Range(-randomAngleTweak,randomAngleTweak),Vector3.up);
            nextDestination = nextDestination + (randomRot * enemyToSelf.normalized * (distanceFromEnemy + Random.Range(-randomDistanceTweak,randomDistanceTweak)));
            //Debug.Log(nextDestination);
            movementDestination.position = nextDestination;
        }
    }
    public override void DetermineAim()
    {
        Vector3 targetPosition = targetEnemy.transform.position;
        float heightAdjustment = targetEnemy.GetComponent<CharacterController>().height * 0.8f;
        targetPosition.y += heightAdjustment;
        aimTarget.position = targetPosition;
    }
    public override void MakeInput()
    {
        //attempt to shoot a damage attack

        //get the attack from the self's ability manager
        AbilityManager abilManager = objectDetection.GetCurrentContext().selfSummary.summarizedPlayer.GetComponent<AbilityManager>();
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
        if (!(abilToUse == null))
        {
            foreach (InputOptions.Input i in abilManager.abilToInput[abilToUse])
            {
                inputCall.AddInput(i);
            }
        }
    }
}
