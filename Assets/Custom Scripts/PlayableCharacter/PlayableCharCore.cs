using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif
using StarterAssets;


public class PlayableCharCore : MonoBehaviour
{
    //variables
    [Header("Player stats")]
    [Tooltip("playable character base stats")]
    public PlayableCharacterStats PlayerStats;

    public TeamManager playerAllegience; //set to team object 

    private int hitPointsCurrent; //current hp
    private int hitPointsCurrentMax; //current max health
    private float damageTakeMult = 1f; //damage taken multiplier
    private float damageDealMult = 1f; //damage dealt multiplier

    //forward movement adjustment
    private float forwardSpeedCurrent = 6f;
    private float forwardSpeedMult = 1f;
    //backward movement adjustment
    private float backwardSpeedCurrent = 6f;
    private float backwardSpeedMult = 1f;
    //strafe movement adjustment
    private float strafeSpeedCurrent = 6f;
    private float strafeSpeedMult = 1f;
    //jump height adjustment
    private float jumpHeightCurrent = 1.4f;
    private float jumpHeightMult = 1f;
    //gravity adjustment
    private float GravityCurrent = -15f;
    private float GravityMult = 1f;

    [Tooltip("If player faces movement or camera (true for movement, false for camera)")]
    public MovementStyles.MovementStyle movementStyle;

    [Header("Misc.")]
    [Tooltip("Third person controller script")]
    public ThirdPersonController playerMovement;

    public Transform PlayerArmature;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif

    //add operation for match load
    void Start(){
        hitPointsCurrent = PlayerStats.hitPointsBase;
        hitPointsCurrentMax = PlayerStats.hitPointsBase;
        damageTakeMult = PlayerStats.damageTakeMultBase;
        damageDealMult = PlayerStats.damageDealMultBase;
        forwardSpeedCurrent = PlayerStats.forwardSpeedBase;
        strafeSpeedCurrent = PlayerStats.strafeSpeedBase;
        backwardSpeedCurrent = PlayerStats.backwardSpeedBase;
        jumpHeightCurrent = PlayerStats.jumpHeightBase;
        GravityCurrent = PlayerStats.GravityBase;

    }

    //add Player operations that must be done every tick
    void Update(){
        //set movement speed in third person controller equal to moveSpeedCurrent
        playerMovement.SetForwardMovementSpeed(GetForwardSpeed());
        playerMovement.SetStrafeMovementSpeed(GetStrafeSpeed());
        playerMovement.SetBackwardMovementSpeed(GetBackwardSpeed());
        playerMovement.SetGravity(GetGravity());
        playerMovement.SetJumpHeight(GetJumpHeight());
        playerMovement.setPlayerMovementStyle(movementStyle);
        
    }

    public float GetForwardSpeed(){return forwardSpeedCurrent * forwardSpeedMult;}
    public float GetBackwardSpeed(){return backwardSpeedCurrent * backwardSpeedMult;}
    public float GetStrafeSpeed(){return strafeSpeedCurrent * strafeSpeedMult;}
    public float GetGravity(){return GravityCurrent * GravityMult;}
    public float GetJumpHeight(){return jumpHeightCurrent * jumpHeightMult;}

    public int DealDamage(int damage)
    {
        int DamageDealt = (int)(damage * damageTakeMult);
        hitPointsCurrent -= DamageDealt;

        return DamageDealt;
    }

    public void ModifyForwardSpeed(float speedMod)
    {
        forwardSpeedMult += speedMod;
    }

    public void ModifyStrafeSpeed(float speedMod)
    {
        strafeSpeedMult += speedMod;
    }

    public void ModifyBackwardSpeed(float speedMod)
    {
        backwardSpeedMult += speedMod;
    }

    public float GetDamageMult(){return damageDealMult;}

    public int GetHitPointsCurrent(){return hitPointsCurrent;}
    public int GethitPointsCurrentMax() {return hitPointsCurrentMax;}
}
