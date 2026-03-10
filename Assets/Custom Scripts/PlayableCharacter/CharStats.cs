using UnityEngine;

[CreateAssetMenu(fileName = "PlayableCharacterStats", menuName = "Scriptable Objects/PlayableCharacterStats")]
public class PlayableCharacterStats : ScriptableObject
{
    //variables
    [Header("Hitpoints vars")]
    [Tooltip("base hp")]
    public int hitPointsBase ;

    //[Tooltip("Current hp")]
    //private int hitPointsCurrent;

    [Header("Damage modifier vars")]
    [Tooltip("Damage Taken Multiplier - multiply damage taken by this value.")]
    //public float damageTakeMult = 1f;
    public float damageTakeMultBase = 1f;
    [Tooltip("Damage Dealt Multiplier - multiply damage dealt to others by this value.")]
    public float damageDealMultBase = 1f;
    
    //public float damageDealMult = 1f;

    [Header("Movement vars")]
    [Tooltip("base forward speed")]
    public float forwardSpeedBase = 6f;
    //private float forwardSpeedCurrent = 6f;
    //private float forwardSpeedMult = 1f;

    [Tooltip("base backward speed")]
    public float backwardSpeedBase = 6f;
    //private float backwardSpeedCurrent = 6f;
    //private float backwardSpeedMult = 1f;

    [Tooltip("base strafe speed")]
    public float strafeSpeedBase = 6f;
    //private float strafeSpeedCurrent = 6f;
    //private float strafeSpeedMult = 1f;

    

    [Tooltip("base Jump Height")]
    public float jumpHeightBase = 1.4f;
    //private float jumpHeightCurrent = 1.4f;
    //private float jumpHeightMult = 1f;

    [Tooltip("base Gravity")]
    public float GravityBase = -15f;
   // private float GravityCurrent = -15f;
    //private float GravityMult = 1f;

}
