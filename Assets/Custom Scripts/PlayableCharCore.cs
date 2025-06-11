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
    [Tooltip("base speed")]
    public float moveSpeedBase = 6f;
    [Tooltip("Current speed")]
    private float moveSpeedCurrent = 6f;
    [Tooltip("Speed Multiplier")]
    private float moveSpeedMult = 1f;
    [Tooltip("base Jump Height")]
    public float jumpHeightBase = 1.4f;
    [Tooltip("Current Jump Height")]
    private float jumpHeightCurrent = 1.4f;
    [Tooltip("Jump Height Multiplier")]
    private float jumpHeightMult = 1f;
    [Tooltip("base Gravity")]
    public float GravityBase = -15f;
    [Tooltip("Current Gravity")]
    private float GravityCurrent = -15f;
    [Tooltip("Jump Gravity")]
    private float GravityMult = 1f;


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
        moveSpeedCurrent = moveSpeedBase;
        jumpHeightCurrent = jumpHeightBase;
        GravityCurrent = GravityBase;

    }

    //add Player operations that must be done every tick
    void Update(){
        //set movement speed in third person controller equal to moveSpeedCurrent
        playerArmature.GetComponent<StarterAssets.ThirdPersonController>().SetMovementSpeed(moveSpeedCurrent);
        playerArmature.GetComponent<StarterAssets.ThirdPersonController>().SetGravity(GravityCurrent);
        playerArmature.GetComponent<StarterAssets.ThirdPersonController>().SetJumpHeight(jumpHeightCurrent);

        
    }


}
