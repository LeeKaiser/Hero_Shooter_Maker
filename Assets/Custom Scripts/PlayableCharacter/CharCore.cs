using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif
using StarterAssets;

//rename to CharCore in the future
public class CharCore : MonoBehaviour
{
    //variables
    [Header("Player stats")]
    [Tooltip("playable character base stats")]
    public CharStats Stats;
    
    [Tooltip("Team the player belongs to")]
    public TeamManager PlayerAllegience; //set to team object 
    [Tooltip("The way player moves")]
    public MovementStyles.MovementStyle MovementStyle;

    [Header("Misc.")]
    [Tooltip("Third person controller script")]
    public ThirdPersonController PlayerMovement;
    [Tooltip("Player's armature")]
    public GameObject PlayerArmature;
    [Tooltip("Player's alive status (true if alive, false if dead)")]
    public bool IsAlive;
    
    private CharStats currentStats; //local copy of player's stats (can be edited)
    private int hitPointsCurrent; //current hp
    private float damageTakeMult = 1f; //damage taken multiplier
    private float damageDealMult = 1f; //damage dealt multiplier
    
    //forward movement adjustment
    private float forwardSpeedMult = 1f;
    //backward movement adjustment
    private float backwardSpeedMult = 1f;
    //strafe movement adjustment
    private float strafeSpeedMult = 1f;
    //jump height adjustment
    private float jumpHeightMult = 1f;
    //gravity adjustment
    private float GravityMult = 1f;

    

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif

    //add operation for match load
    void Awake()
    {
        PlayerArmature = transform.Find("PlayerArmature").gameObject;
        PlayerMovement = PlayerArmature.GetComponent<ThirdPersonController>();
    }

    void Start(){
        currentStats = Instantiate(Stats);
        hitPointsCurrent = currentStats.HitPointsBase;
        
    }

    //add Player operations that must be done every tick
    void Update(){
        //set movement speed in third person controller equal to moveSpeedCurrent
        PlayerMovement.SetForwardMovementSpeed(GetForwardSpeed());
        PlayerMovement.SetStrafeMovementSpeed(GetStrafeSpeed());
        PlayerMovement.SetBackwardMovementSpeed(GetBackwardSpeed());
        PlayerMovement.SetGravity(GetGravity());
        PlayerMovement.SetJumpHeight(GetJumpHeight());
        PlayerMovement.setPlayerMovementStyle(MovementStyle);
        
    }

    public float GetForwardSpeed(){return currentStats.ForwardSpeedBase * forwardSpeedMult;}
    public float GetBackwardSpeed(){return currentStats.BackwardSpeedBase * backwardSpeedMult;}
    public float GetStrafeSpeed(){return currentStats.StrafeSpeedBase * strafeSpeedMult;}
    public float GetGravity(){return currentStats.GravityBase * GravityMult;}
    public float GetJumpHeight(){return currentStats.JumpHeightBase * jumpHeightMult;}

    //Deal Damage
    //causes the character to take damage
    public int DealDamage(int damage)
    {
        int DamageDealt = (int)(damage * damageTakeMult);
        hitPointsCurrent -= DamageDealt;
        if (hitPointsCurrent <= 0)
        {
            Defeat();
        }

        return DamageDealt;
    }

    //Defeat
    //causes the character to die
    public void Defeat()
    {
        IsAlive = false;
        PlayerArmature.SetActive(IsAlive);
    }

    //Spawn
    //if player is previously dead, spawns the player
    public void Spawn()
    {
        if (!IsAlive)
        {
            IsAlive = true;
            PlayerArmature.SetActive(IsAlive);
        }
    }
    //Spawn
    //if player is previously dead, spawns the player at a location
    public void Spawn(Vector3 spawnLocation)
    {
        if (!IsAlive)
        {
            Spawn();
            PlayerArmature.transform.position = spawnLocation;
        }
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
    public int GetHitPointsBase() {return currentStats.HitPointsBase;}
}
