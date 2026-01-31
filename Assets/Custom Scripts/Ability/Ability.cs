using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System.Collections;
using AbilityClassification;

public abstract class Ability: MonoBehaviour
{
    //charge represents a use of an ability (functional as ammo)
    //charge point is progress to complete a full charge up or reload.
    [Header("ability stats")]
    [Tooltip("ability stats")]
    public AbilityStats abilityStat;
    [Tooltip("reference to user")]
    public GameObject UserRef;
    //variables
    protected int currentCharge ; //remaining  charge
    protected int currentMaxCharge ; // current maximum charge

    protected float chargePointsProgress; //current progress on getting new charge

    protected float chargePointMultiplier = 1; //amount of multiplier to the charge rate

    protected bool rechargeInProgress = false;

    public AbilityManager Manager; //reference to ability Manager
    protected bool isActive = false; // 

    public AbilityUI AbilUIRef; //reference to ability's UI

    public AbilityClass CurrentAbilClass;

    void Awake()
    {
        currentCharge = abilityStat.maxCharge;
        currentMaxCharge = abilityStat.maxCharge;
        GetComponentInParent<AbilityManager>().AddAbility(this.gameObject);
        
    }

    public virtual void Initialize(AbilityManager owningManager, GameObject playerReference)
    {
        this.Manager = owningManager;
        this.UserRef = playerReference;
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
        return !isActive && Manager.CanUseAbility(this) && currentCharge >= 1;
    }

    //when ability is missing any charge, set recharge in progress to true
    public void ActivateReload()
    {
        if (currentCharge < abilityStat.maxCharge && !isActive)
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
            while (chargePointsProgress >= abilityStat.chargePointsRequired)
            {
                //give a charge
                if (currentCharge < currentMaxCharge)
                {
                    currentCharge += abilityStat.chargeGainPerFullRecharge;
                }
                //subtract charge points required from charge point progress 
                chargePointsProgress -= abilityStat.chargePointsRequired;
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
        float newCharge = abilityStat.chargePointsPerSec * TimeElapsed;
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
}
