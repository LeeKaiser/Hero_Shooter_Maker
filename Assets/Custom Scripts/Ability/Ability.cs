using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System.Collections;
using AbilityClassification;

/*
Ability
abstract parent class for abiltiies
*/
public abstract class Ability: MonoBehaviour
{
    //variables - public
    [Tooltip("ability stats")]
    public AbilityStats abilityStat;
    [Tooltip("reference to ability's UI")]
    public AbilityUI abilUIRef;
    [Tooltip("ability's class")]
    public AbilityClass CurrentAbilClass;

    //variables - private
    //reference to character information
    protected PlayableCharCore playerRef;
    //charge represents a use of an ability (functional as ammo)
    protected int currentCharge ; //remaining  charge
    protected int currentMaxCharge ; // current maximum charge
    
    //charge point is progress to complete a full charge up or reload.
    protected float chargePointsProgress; //current progress on getting new charge
    protected float chargePointMultiplier = 1; //amount of multiplier to the charge rate
    protected bool rechargeInProgress = false; //true if currently recharging
    
    //reference to ability manager
    protected AbilityManager manager; 
    //bool representing if the ability is currently active
    protected bool isActive = false;

    //Methods
    //called when ability is created
    void Start()
    {
        currentCharge = abilityStat.maxCharge;
        currentMaxCharge = abilityStat.maxCharge;
        GetComponentInParent<AbilityManager>().AddAbility(this.gameObject);
        
    }
    //initialize some ability's information (manager and player reference)
    public virtual void Initialize(AbilityManager owningManager, PlayableCharCore playerReference)
    {
        this.manager = owningManager;
        this.playerRef = playerReference;
        Startup();
    }
    //called when a charge is used. decreasing current charge. does not go below 0
    protected void ConsumeCharge(int chargeConsumed)
    {
        currentCharge -= chargeConsumed;
        if (currentCharge < 0)
        {
            currentCharge = 0;
        }
    }
    //checks if it is valid for ability to activate
    protected virtual bool CanActivate()
    {
        return !isActive && manager.CanUseAbility(this) && currentCharge >= 1;
    }

    //when ability is missing any charge, set recharge in progress to true
    public void ActivateReload()
    {
        if (currentCharge < abilityStat.maxCharge && !isActive)
        {
            rechargeInProgress = true;
        }
    }

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
