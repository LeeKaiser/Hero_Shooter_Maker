using UnityEngine;

[CreateAssetMenu(fileName = "CharStats", menuName = "Scriptable Objects/CharStats")]
public class CharStats : ScriptableObject
{
    //variables
    [Header("Hitpoints vars")]
    [Tooltip("base hp")]
    public int HitPointsBase ;

    [Header("Movement vars")]
    [Tooltip("base forward speed")]
    public float ForwardSpeedBase = 6f;
    //private float forwardSpeedCurrent = 6f;
    //private float forwardSpeedMult = 1f;

    [Tooltip("base backward speed")]
    public float BackwardSpeedBase = 6f;
    //private float backwardSpeedCurrent = 6f;
    //private float backwardSpeedMult = 1f;

    [Tooltip("base strafe speed")]
    public float StrafeSpeedBase = 6f;
    //private float strafeSpeedCurrent = 6f;
    //private float strafeSpeedMult = 1f;

    

    [Tooltip("base Jump Height")]
    public float JumpHeightBase = 1.4f;
    //private float jumpHeightCurrent = 1.4f;
    //private float jumpHeightMult = 1f;

    [Tooltip("base Gravity")]
    public float GravityBase = -15f;
   // private float GravityCurrent = -15f;
    //private float GravityMult = 1f;

    [Header("AI varialbe")]
    [Tooltip("index to access the priority of patrol point for this character")]
    public int PatrolPriorityIndex = 0;

}
