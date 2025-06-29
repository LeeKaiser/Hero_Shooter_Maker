using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System.Collections;

public abstract class Ability: MonoBehaviour
{
    //charge represents a use of an ability (functional as ammo)
    //charge point is progress to complete a full charge up or reload.


    //variables
    [Header("Cooldown variables")]
    [Tooltip("maximum amount of charge that can be held")]
    public int maxCharge = 1;
    public int currentCharge = 1; //remaining  charge

    [Tooltip("Amount of charge to gain in order to complete a charge once")]
    public float chargePointsRequired = 100;

    public float chargePointsProgress; //current progress on getting new charge

    [Tooltip("amount of charge point gained per second ")]
    public float chargePointsPerSec = 100;

    protected float chargePointMultiplier = 1; //amount of multiplier to the charge rate

    [Tooltip("amount of charge gained per full charge point")]
    public int chargeGainPerFullRecharge = 1;

    [Tooltip("bool on if this is being recharged or not")]
    public bool rechargeInProgress = false;

    [Header("Input variables")]
    [Tooltip("all input that can be used to activate this ability")]
    public InputActionReference actionReference;

    protected InputAction boundAction;

    protected AbilityManager manager;
    protected bool isActive = false;

    [Header("use time related variables")]
    [Tooltip("if using ability disables use of other abilities")]
    public bool canInterruptOthers = false;
    [Tooltip("amount of time ability use is disabled for when using the ability")]
    public float useTime = 0.2f;
    [Tooltip("amount of time ability use is disabled for when attempting to use ability and it fails")]
    public float useFailTime = 0.0f;

    private float holdStartTime;
    private Coroutine holdLoop;
    private bool isCharging = false;

    [Header("charge variables")]
    [Tooltip("amount of time to charge up ability")]
    public float maxChargeTime;

    [Header("hold fire variables")]
    [Tooltip("amount of time ability is used per second")]
    public float usePerSec;
    
    [System.Flags]
    public enum InputResponseMode
    {
        None = 0,
        Tap = 1 << 0,
        Hold = 1 << 1,
        Release = 1 << 2
    }

    public InputResponseMode inputMode = InputResponseMode.Tap | InputResponseMode.Release;

    void Start()
    {
        currentCharge = maxCharge;
    }

    public virtual void Initialize(PlayerInput playerInput, AbilityManager owningManager)
    {
        manager = owningManager;

        if (actionReference == null || actionReference.action == null)
        {
            Debug.LogError($"{gameObject.name}: No InputActionReference assigned.");
            return;
        }

        // Get the runtime action instance from PlayerInput
        boundAction = playerInput.actions.FindAction(actionReference.action.name);

        if (boundAction == null)
        {
            Debug.LogError($"{gameObject.name}: Action '{actionReference.action.name}' not found in PlayerInput.");
            return;
        }

        boundAction.started += OnInputStarted;
        boundAction.performed += OnInputHeld;
        boundAction.canceled += OnInputCanceled;

        Debug.Log($"{gameObject.name} bound to input: {boundAction.name}");
    }

    //method that activates when user first presses the input
    protected virtual void OnInputStarted(InputAction.CallbackContext context)
    {
        if (!CanActivate()) return;

        if (context.interaction is HoldInteraction)
        {
            Debug.Log("Hold started → begin auto-fire and charge timer");

            if (inputMode.HasFlag(InputResponseMode.Release) )
            {
                isCharging = true;
                holdStartTime = Time.time;
            }

            if (holdLoop == null && inputMode.HasFlag(InputResponseMode.Hold) )
            {
                holdLoop = StartCoroutine(RepeatWhileHeld());
            }
        }

        
    }

    //method that activates when user holds the input
    protected virtual void OnInputHeld(InputAction.CallbackContext context)
    {
        if (!CanActivate()) return;

        if (context.interaction is TapInteraction && inputMode.HasFlag(InputResponseMode.Tap))
        {
            Debug.Log("Tap performed → single fire");
            StartCoroutine(ActivateAbility());
        }
    }

    //method that activates when user lets go of the input
    protected virtual void OnInputCanceled(InputAction.CallbackContext context)
    {
        if (!CanActivate()) return;

        if (isCharging && inputMode.HasFlag(InputResponseMode.Release))
        {
            Debug.Log("Hold released → fire charged shot");

            isCharging = false;

            float heldTime = Time.time - holdStartTime;
            float chargeRatio = Mathf.Clamp01(heldTime / maxChargeTime);

            StartCoroutine(ActivateReleasedAbility(chargeRatio));
        }

        if (holdLoop != null)
        {
            StopCoroutine(holdLoop);
            holdLoop = null;
        }
    }

    protected virtual IEnumerator RepeatWhileHeld()
    {
        while (CanActivate())
        {
            
            Debug.Log("Auto-firing while held");
            yield return Execute(); // normal attack
            
            float unloadTime = 1 / usePerSec;

            yield return new WaitForSeconds(unloadTime);
        }
        holdLoop = null;
        yield return null;
    }

    protected virtual IEnumerator ActivateAbility()
    {
        isActive = true;
        manager.NotifyAbilityStarted(this);

        yield return Execute(); // ability logic

        isActive = false;
        manager.NotifyAbilityEnded(this);
    }

    protected virtual IEnumerator ActivateReleasedAbility(float chargeRatio)
    {
        isActive = true;
        manager.NotifyAbilityStarted(this);

        yield return ExecuteReleased(chargeRatio); // override this in subclasses

        isActive = false;
        manager.NotifyAbilityEnded(this);
    }

    // tap or held logic
    protected abstract IEnumerator Execute();

    // charged then released logic
    protected abstract IEnumerator ExecuteReleased(float chargeRatio);

    protected virtual bool CanActivate()
    {
        return !isActive && manager.CanUseAbility(this) && currentCharge >= 1;
    }



    // # Ability recharge code
    //recover ability charge point
    public void RecoverChargePoint(float TimeElapsed){

        if (rechargeInProgress)
        {
            chargePointsProgress += chargePointsPerSec * TimeElapsed * chargePointMultiplier;
            while (chargePointsProgress >= chargePointsRequired)
            {
                //give a charge
                if (currentCharge < maxCharge)
                {
                    currentCharge += chargeGainPerFullRecharge;
                }
                //subtract charge points required from charge point progress 
                chargePointsProgress -= chargePointsRequired;
                //if fully charged, reset charge point progress to 0
                if (currentCharge >= maxCharge)
                {
                    currentCharge = maxCharge;
                    chargePointsProgress = 0;
                    rechargeInProgress = false;
                }
            }
        }
    }

    public void GiveChargePointDirect(float ChargePtAdd )
    {
        RecoverChargePoint(ChargePtAdd / chargePointsPerSec);
    }

    public void InterruptReload()
    {
        if (rechargeInProgress)
        {
            chargePointsProgress = 0;
        }
    }

    public virtual void Cleanup()
    {
        if (actionReference != null)
            boundAction.started -= OnInputStarted;
            boundAction.performed -= OnInputHeld;
            boundAction.canceled -= OnInputCanceled;
    }

}
