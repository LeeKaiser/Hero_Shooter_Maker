using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif


public class PlayableCharCore : MonoBehaviour
{
    //variables
    [Header("Hitpoints vars")]
    [Tooltip("base hp")]
    public int hitPointsBase ;

    [Tooltip("Current hp")]
    private int hitPointsCurrent;

    [Header("Damage modifier vars")]
    
    [Tooltip("Damage Taken Multiplier - multiply damage taken by this value.")]
    public float damageTakeMult = 1f;
    private float damageTakeMultBase = 1f;

    private float damageDealMultBase = 1f;
    [Tooltip("Damage Dealt Multiplier - multiply damage dealt to others by this value.")]
    public float damageDealMult = 1f;

    [Header("Movement vars")]
    [Tooltip("base forward speed")]
    public float forwardSpeedBase = 6f;
    private float forwardSpeedCurrent = 6f;
    private float forwardSpeedMult = 1f;

    [Tooltip("base backward speed")]
    public float backwardSpeedBase = 6f;
    private float backwardSpeedCurrent = 6f;
    private float backwardSpeedMult = 1f;

    [Tooltip("base strafe speed")]
    public float strafeSpeedBase = 6f;
    private float strafeSpeedCurrent = 6f;
    private float strafeSpeedMult = 1f;

    

    [Tooltip("base Jump Height")]
    public float jumpHeightBase = 1.4f;
    private float jumpHeightCurrent = 1.4f;
    private float jumpHeightMult = 1f;

    [Tooltip("base Gravity")]
    public float GravityBase = -15f;
    private float GravityCurrent = -15f;
    private float GravityMult = 1f;

    [Tooltip("If player faces movement or camera (true for movement, false for camera)")]
    public bool PlayerFaceMovement = true;


    //add ability / passive / inventory array in future

    [Header("Misc.")]
    [Tooltip("Third person controller script")]
    public GameObject playerArmature;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif

    //add operation for match load
    void Start(){
        hitPointsCurrent = hitPointsBase;
        damageTakeMult = damageTakeMultBase;
        damageDealMult = damageDealMultBase;
        forwardSpeedCurrent = forwardSpeedBase;
        strafeSpeedCurrent = strafeSpeedBase;
        backwardSpeedCurrent = backwardSpeedBase;
        jumpHeightCurrent = jumpHeightBase;
        GravityCurrent = GravityBase;

    }

    //add Player operations that must be done every tick
    void Update(){
        //set movement speed in third person controller equal to moveSpeedCurrent
        playerArmature.GetComponent<StarterAssets.ThirdPersonController>().SetForwardMovementSpeed(forwardSpeedCurrent);
        playerArmature.GetComponent<StarterAssets.ThirdPersonController>().SetStrafeMovementSpeed(strafeSpeedCurrent);
        playerArmature.GetComponent<StarterAssets.ThirdPersonController>().SetBackwardMovementSpeed(backwardSpeedCurrent);
        playerArmature.GetComponent<StarterAssets.ThirdPersonController>().SetGravity(GravityCurrent);
        playerArmature.GetComponent<StarterAssets.ThirdPersonController>().SetJumpHeight(jumpHeightCurrent);
        playerArmature.GetComponent<StarterAssets.ThirdPersonController>().setPlayerFaceMove(PlayerFaceMovement);
        
    }

    public void DealDamage(int damage)
    {
        hitPointsCurrent -= damage;
    }


}
