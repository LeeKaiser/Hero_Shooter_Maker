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

/*
Status Effect Manager
manages player's status effects
*/
public class StatusEffectManager : MonoBehaviour
{
    [Header("Status Effect Stats")]
    [Tooltip("current status effects")]
    [SerializeField] List<StatusEffect> statusEffectList = new List<StatusEffect>();

    PlayableCharCore playerRef;

    [Tooltip("EffectTimeMultiplier for buffs")]
    public float EffectTimeMultiplierBuffs = 1;
    [Tooltip("EffectTimeMultiplier for debuffs")]
    public float EffectTimeMultiplierDebuffs = 1;

    public void Start()
    {
        playerRef = this.gameObject.GetComponent<PlayableCharCore>();
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

        effect.SetAffectedPlayer(playerRef);
        effect.ApplyEffect();
        statusEffectList.Add(effect);
    }

    public void Update()
    {
        //delete inactive SE
        for (int i = statusEffectList.Count - 1; i >= 0; i--)
        {
            if (statusEffectList[i].statusEffectStat.ExpireViaTime)
            {
                float DurationMult = 1;
                if (statusEffectList[i].statusEffectStat.effectCategory == EffectCategory.Buff)
                {
                    DurationMult = EffectTimeMultiplierBuffs;
                }
                if (statusEffectList[i].statusEffectStat.effectCategory == EffectCategory.Debuff)
                {
                    DurationMult = EffectTimeMultiplierDebuffs;
                }

                statusEffectList[i].SpendDuration(Time.deltaTime / DurationMult);
                
            }
            
            

            if (!statusEffectList[i].CurrentlyActive())
            {
                statusEffectList.RemoveAt(i);
            }
        }
    }
}
