using UnityEngine;

/*
Status Effect
abstract parent class for status effects
*/
public abstract class StatusEffect : MonoBehaviour
{
    //variable - public
    [Tooltip("base stats of status effect")]
    public StatusEffectStats statusEffectStat;

    //variable - private
    //remaining duration
    protected float RemainingDuration;

    //player it is affecting
    protected PlayableCharCore AffectedPlayer;
    
    //is active
    protected bool Active = true; 

    private void Start()
    {
        RemainingDuration = statusEffectStat.EffectDuration;
    }
    
    //main effect
    public abstract void ApplyEffect();

    //inverse of effect that activates to remove the effect
    protected abstract void RemoveEffect();

    public void SpendDuration(float timePassed)
    {
        RemainingDuration -= timePassed;
        if (RemainingDuration <= 0)
        {
            RemoveEffect();
            Active = false;
        }

    }

    public bool CurrentlyActive()
    {
        return Active;
    }

    public void SetAffectedPlayer(PlayableCharCore player)
    {
        AffectedPlayer = player;
    }
}
