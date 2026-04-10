using UnityEngine;
using System.Collections.Generic;
using StarterAssets;
using System;

[System.Flags]
    public enum EffectCategory
    {
        None,
        Buff,
        Debuff,
        Ohter
    }

public class StatusEffectManager : MonoBehaviour
{
    [Header("Status Effect Stats")]
    [Tooltip("current status effects")]
    [SerializeField] List<StatusEffect> statusEffectList = new List<StatusEffect>();

    CharCore playerReference;

    [Tooltip("EffectTimeMultiplier for buffs")]
    public float EffectTimeMultiplierBuffs = 1;
    [Tooltip("EffectTimeMultiplier for debuffs")]
    public float EffectTimeMultiplierDebuffs = 1;

    public void Start()
    {
        playerReference = this.gameObject.GetComponent<CharCore>();
    }

    //get effect
    public void AddNewEffect(GameObject newEffect)
    {
        // Make a copy of the prefab and attach it to the player
        GameObject EffectObj = Instantiate(newEffect, transform);

        // Grab the Ability script on that prefab
        StatusEffect effect = EffectObj.GetComponent<StatusEffect>();
        if (effect == null)
        {
            Debug.LogError("The prefab does not have an status effect component!");
            return;
        }

        effect.SetAffectedPlayer(playerReference);
        effect.ApplyEffect();
        statusEffectList.Add(effect);
    }

    public void Update()
    {
        //delete inactive SE
        foreach (StatusEffect status in statusEffectList)
        {
            if (status.Stats.ExpireViaTime)
            {
                float DurationMult = 1;
                if (status.Stats.effectCategory == EffectCategory.Buff)
                {
                    DurationMult = EffectTimeMultiplierBuffs;
                }
                if (status.Stats.effectCategory == EffectCategory.Debuff)
                {
                    DurationMult = EffectTimeMultiplierDebuffs;
                }

                status.SpendDuration(Time.deltaTime / DurationMult);
                
            }
            
            if (!status.CurrentlyActive() && status.Stats.DeleteOnExpire)
            {
                RemoveStatus(status);
            }
        }
    }

    public void RemoveStatus(StatusEffect status)
    {
        statusEffectList.Remove(status);
        Destroy(status.gameObject);
    }
}
