using UnityEngine;
using StarterAssets;




[CreateAssetMenu(fileName = "StatusEffectStats", menuName = "Scriptable Objects/StatusEffectStats")]
public class StatusEffectStats : ScriptableObject
{
    [Header("Status Effect Stats")]
    [Tooltip("duration")]
    public float EffectDuration;

    [Tooltip("duration is time based. true if it is time based, false if duration uses other system")]
    public bool ExpireViaTime = true;

    public EffectCategory effectCategory;

}
