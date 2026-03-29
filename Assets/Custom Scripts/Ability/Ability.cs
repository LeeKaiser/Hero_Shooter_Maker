using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System.Collections;
using AbilityClassification;

/*
Ability
abstract parent class for all abilities
*/
public abstract class Ability: MonoBehaviour
{
    //charge represents a use of an ability (functional as ammo)
    //charge point is progress to complete a full charge up or reload.
    [Header("Ability operation")]
    [Tooltip("ability stats")]
    public AbilityStats Stats;
    
    [Header("AI Relevant Information")]
    [Tooltip("ability classification")]
    public AbilityClass CurrentAbilClass;
    
    protected CharCore playerReference;
    //variables
    protected int currentCharge ; //remaining  charge
    protected int currentMaxCharge ; // current maximum charge

    protected float chargePointsProgress; //current progress on getting new charge

    protected float chargePointMultiplier = 1; //amount of multiplier to the charge rate

    protected bool rechargeInProgress = false;

    protected AbilityManager manager; //reference to ability manager
    protected bool isActive = false; // 

    protected AbilityUI AbilityUIReference; //reference to ability's UI

    void Start()
    {
        currentCharge = Stats.MaxCharge;
        currentMaxCharge = Stats.MaxCharge;
        GetComponentInParent<AbilityManager>().AddAbility(this.gameObject);
        
    }

    public virtual void Initialize(AbilityManager owningManager, CharCore playerCharCore)
    {
        this.manager = owningManager;
        this.playerReference = playerCharCore;
        Startup();
    }

    protected void ConsumeCharge(int chargeConsumed)
    {
        currentCharge -= chargeConsumed;
        if (currentCharge < 0)
        {
            currentCharge = 0;
        }
    }

    protected virtual bool CanActivate()
    {
        return !isActive && manager.CanUseAbility(this) && currentCharge >= 1;
    }

    //when ability is missing any charge, set recharge in progress to true
    public void ActivateReload()
    {
        if (currentCharge < Stats.MaxCharge && !isActive)
        {
            rechargeInProgress = true;
        }
    }

    // # Ability recharge code
    //recover ability charge point
    public void RecoverChargePoint(float chargeAdded){

        if (rechargeInProgress)
        {
            //add to charge point's progress
            chargePointsProgress += chargeAdded * chargePointMultiplier;
            //converts charge points progress to charge
            while (chargePointsProgress >= Stats.ChargePointsRequired)
            {
                //give a charge
                if (currentCharge < currentMaxCharge)
                {
                    currentCharge += Stats.ChargeGainPerFullRecharge;
                }
                //subtract charge points required from charge point progress 
                chargePointsProgress -= Stats.ChargePointsRequired;
                //if fully charged, reset charge point progress to 0
                if (currentCharge >= currentMaxCharge)
                {
                    currentCharge = currentMaxCharge;
                    chargePointsProgress = 0;
                    rechargeInProgress = false;
                }
            }
        }
    }

    //for abilities that reload over time, call this method every update
    public void ReloadOverTime(float TimeElapsed )
    {
        float newCharge = Stats.ChargePointsPerSec * TimeElapsed;
        RecoverChargePoint(newCharge);
    }

    public void InterruptReload()
    {
        if (rechargeInProgress)
        {
            chargePointsProgress = 0;
        }
    }

    public abstract void Cleanup();

    protected abstract void Startup();

    public float GetCurrentCharge() { return currentCharge;}
    public float GetChargePointProgress() { return chargePointsProgress;}

    public float GetCurrentMaxCharge() {return currentMaxCharge;}

    public AbilityUI GetAbilityUI(){ return AbilityUIReference;}
    public void SetAbilityUI(AbilityUI ui){AbilityUIReference = ui;}
}
