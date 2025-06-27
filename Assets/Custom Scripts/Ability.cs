using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public abstract class Ability: MonoBehaviour
{
    //charge represents a use of an ability (functional as ammo)
    //charge point is progress to complete a full charge up or reload.


    //variables
    [Header("Cooldown variables")]
    [Tooltip("maximum amount of charge that can be held")]
    public int MaxCharge = 1;
    protected int CurrentCharge = 1; //remaining  charge

    [Tooltip("Amount of charge to gain in order to complete a charge once")]
    public float ChargePointsRequired = 100;

    protected float ChargePointsProgress; //current progress on getting new charge

    [Tooltip("amount of charge point gained per second ")]
    public float ChargePointsPerSec = 100;

    [Tooltip("amount of charge gained per full charge point")]
    public int ChargeGainPerFullRecharge = 1;

    [Tooltip("bool on if this is being recharged or not")]
    public bool RechargeInProgress = false;

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
    public float UseTime = 0.2f;
    [Tooltip("amount of time ability use is disabled for when attempting to use ability and it fails")]
    public float UseFailTime = 0.0f;

    void Awake(){}

    void OnDestroy(){}

    void OnEnable(){}

    void OnDisable(){}

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

        boundAction.performed += OnInputPerformed;

        Debug.Log($"{gameObject.name} bound to input: {boundAction.name}");
    }

    protected virtual void OnInputPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("input made");
        if (CanActivate())
            StartCoroutine(ActivateAbility());
    }

    protected virtual bool CanActivate()
    {
        return !isActive && manager.CanUseAbility(this);
    }

    protected virtual IEnumerator ActivateAbility()
    {
        isActive = true;
        manager.NotifyAbilityStarted(this);

        yield return Execute();

        isActive = false;
        manager.NotifyAbilityEnded(this);
    }

    protected abstract IEnumerator Execute();

    //recover ability charge point
    public void RecoverChargePoint(float TimeElapsed){
        
        if (RechargeInProgress)
        {
            ChargePointsProgress = ChargePointsPerSec * TimeElapsed;
            while (ChargePointsProgress >= ChargePointsRequired)
            {
                //give a charge
                if (CurrentCharge < MaxCharge)
                {
                    CurrentCharge += ChargeGainPerFullRecharge;
                }
                //subtract charge points required from charge point progress 
                ChargePointsProgress -= ChargePointsRequired;
                //if fully charged, reset charge point progress to 0
                if (CurrentCharge >= MaxCharge)
                {
                    CurrentCharge = MaxCharge;
                    ChargePointsProgress = 0;
                    RechargeInProgress = false;
                }
            }
        }
    }

    public void GiveChargePointDirect(float ChargePtAdd )
    {
        RecoverChargePoint(ChargePtAdd / ChargePointsPerSec);
    }

    public virtual void Cleanup()
    {
        if (actionReference != null)
            actionReference.action.performed -= OnInputPerformed;
    }

}
