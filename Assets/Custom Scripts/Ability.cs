using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Ability: MonoBehaviour
{
    //charge represents a use of an ability (functional as ammo)
    //charge point is progress to complete a full charge up or reload.


    //variables
    [Header("Cooldown variables")]
    [Tooltip("maximum amount of charge that can be held")]
    public int MaxCharge = 1;
    private int CurrentCharge; //remaining  charge

    [Tooltip("Amount of charge to gain in order to complete a charge once")]
    public int ChargePointsRequired = 100;

    private int ChargePointsProgress; //current progress on getting new charge

    [Tooltip("amount of charge point gained per second ")]
    public int ChargePointsPerSec = 100;

    [Tooltip("amount of charge gained per full charge point")]
    public int ChargeGainPerFullRecharge = 1;

    [Tooltip("bool on if this is being recharged or not")]
    public bool RechargeInProgress = false;

    [Header("Input variables")]
    [Tooltip("all input that can be used to activate this ability")]
    public InputActionReference actionReference;

    private PlayerInputSystem m_Actions;                  // Source code representation of asset.
    private PlayerInputSystem.AbilitiesActions m_Abilities;     // Source code representation of action map.

    void Awake()
    {
        m_Actions = new PlayerInputSystem();              // Create asset object.
        m_Abilities = m_Actions.Abilities;                      // Extract action map object.
    }

    void OnDestroy()
    {
        m_Actions.Dispose();                              // Destroy asset object.
    }

    void OnEnable()
    {
        m_Abilities.Enable();                                // Enable all actions within map.
    }

    void OnDisable()
    {
        m_Abilities.Disable();                               // Disable all actions within map.
    }

    public virtual void Initialize()
    {
        if (actionReference != null)
            actionReference.action.performed += OnPerform;
    }

    protected abstract void OnPerform(InputAction.CallbackContext context);

}
