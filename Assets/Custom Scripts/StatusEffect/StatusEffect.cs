using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{

    public StatusEffectStats statusEffectStat;

    protected float RemainingDuration;

    protected PlayableCharCore AffectedPlayer;

    protected bool Active = true; //is active

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
