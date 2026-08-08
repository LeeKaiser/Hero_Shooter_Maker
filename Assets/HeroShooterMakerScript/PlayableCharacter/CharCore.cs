using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif
using PlayerEvents;

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
    [Tooltip("Ability Manager script")]
    public AbilityManager AbilityManage;
    [Tooltip("Player's armature")]
    public GameObject PlayerArmature;
    [Tooltip("Player's alive status (true if alive, false if dead)")]
    public bool IsAlive;
    
    private CharStats currentStats; //local copy of player's stats (can be edited)
    private int hitPointsCurrent; //current hp
    private float damageTakeMult = 1f; //damage taken multiplier
    private float damageDealMult = 1f; //damage dealt multiplier
    private float healingMult = 1f;
    
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
        AbilityManage = GetComponent<AbilityManager>();
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
    public int DealDamage(int damage, CharCore damageDealer)
    {
        if (hitPointsCurrent <= 0)
        {
            return 0;
        }
        int damageDealt = (int)(damage * damageTakeMult);
        
        hitPointsCurrent -= damageDealt;


        PlayerTakeDamage playerTakeDamageEvent = new PlayerTakeDamage();
        playerTakeDamageEvent.PlayerIdentity = this;
        playerTakeDamageEvent.DamageDealer = damageDealer;
        playerTakeDamageEvent.Damage = damageDealt;
        EventBus<PlayerTakeDamage>.Invoke(playerTakeDamageEvent);


        if (hitPointsCurrent <= 0)
        {
            Defeat(damageDealer);
            hitPointsCurrent = 0;
        }

        return damageDealt;
    }

    public int HealHealth(int healing, CharCore healer)
    {
        if (hitPointsCurrent == currentStats.HitPointsBase)
        {
            return 0;
        }
        int healthHealed = (int)(healing * healingMult);
        
        hitPointsCurrent += healthHealed;

        PlayerHealHealth PlayerHealHealthEvent = new PlayerHealHealth();
        PlayerHealHealthEvent.PlayerIdentity = this;
        PlayerHealHealthEvent.Healer = healer;
        PlayerHealHealthEvent.Healing = healthHealed;
        EventBus<PlayerHealHealth>.Invoke(PlayerHealHealthEvent);

        if (hitPointsCurrent > currentStats.HitPointsBase)
        {
            hitPointsCurrent = currentStats.HitPointsBase;
        }

        return healthHealed;
    }

    //Defeat
    //causes the character to die
    public void Defeat(CharCore killer)
    {
        IsAlive = false;
        PlayerDead playerDeadEvent = new PlayerDead();
        playerDeadEvent.PlayerIdentity = this;
        playerDeadEvent.PlayerKiller = killer;
        EventBus<PlayerDead>.Invoke(playerDeadEvent);
        PlayerArmature.SetActive(IsAlive);
    }

    //Spawn
    //if player is previously dead, spawns the player
    public void Spawn()
    {
        if (!IsAlive)
        {
            IsAlive = true;
            hitPointsCurrent = currentStats.HitPointsBase;
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
            PlayerMovement.Translate(spawnLocation);
        }
    }

    public void ModifyMaxHealth(int health){currentStats.HitPointsBase += health;}

    public void ModifyForwardSpeed(float speedMod){forwardSpeedMult += speedMod;}

    public void ModifyStrafeSpeed(float speedMod){strafeSpeedMult += speedMod;}

    public void ModifyBackwardSpeed(float speedMod){ backwardSpeedMult += speedMod;}

    public void ModifyGravityMult(float gravChange){GravityMult += gravChange;}
    public void ModifyDamageTakeMult(float damageMod){damageTakeMult += damageMod;}
    public void ModifyDamageDealMult(float damageMod){damageDealMult += damageMod;}

    public float GetDamageMult(){return damageDealMult;}

    public int GetHitPointsCurrent(){return hitPointsCurrent;}
    public int GetHitPointsBase() {return currentStats.HitPointsBase;}
    
}
