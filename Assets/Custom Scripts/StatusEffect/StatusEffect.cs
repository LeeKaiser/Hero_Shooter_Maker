using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{

    public StatusEffectStats Stats;

    protected float RemainingDuration;

    protected CharCore AffectedPlayer;
    protected CharCore OwningPlayer;

    protected bool Active = true; //is active

    private void Start()
    {
        RemainingDuration = Stats.EffectDuration;
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

    public void SetDuration(float duration)
    {
        RemainingDuration = duration;
    }

    public float GetDuration(){return RemainingDuration;}

    public bool CurrentlyActive()
    {
        return Active;
    }

    public void SetAffectedPlayer(CharCore player, CharCore owningPlayer)
    {
        AffectedPlayer = player;
        OwningPlayer = owningPlayer;
    }
}
