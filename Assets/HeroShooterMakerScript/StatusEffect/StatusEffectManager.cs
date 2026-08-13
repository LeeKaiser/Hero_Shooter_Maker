using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using HeroShooterMaker.Character;

namespace HeroShooterMaker.StatusEffects
{
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
        public void AddNewEffect(GameObject newEffect, CharCore owningPlayer)
        {
            //filter out non stacking effect
            StatusEffect effect = newEffect.GetComponent<StatusEffect>();
            if (!effect.Stats.DoesStack)
            {
                //check if list already has a status effect of same type
                StatusEffect existing = statusEffectList.FirstOrDefault(e => e.GetType().IsAssignableFrom(effect.GetType()));
                if (existing != null)
                {
                    //if duration of new is longer than remaining duration, reset duration
                    if (existing.GetDuration() < effect.Stats.EffectDuration)
                    {
                        existing.SetDuration(effect.Stats.EffectDuration);
                        return;
                    }
                }
            }

            // Make a copy of the prefab and attach it to the player
            GameObject EffectObj = Instantiate(newEffect, playerReference.PlayerArmature.transform.position, playerReference.PlayerArmature.transform.rotation, playerReference.PlayerArmature.transform);
            EffectObj.transform.position += Vector3.up * (playerReference.PlayerArmature.GetComponent<CharacterController>().height / 2);
            // Grab the Ability script on that prefab
            effect = EffectObj.GetComponent<StatusEffect>();
            if (effect == null)
            {
                Debug.LogError("The prefab does not have an status effect component!");
                return;
            }

            effect.SetAffectedPlayer(playerReference, owningPlayer);
            effect.ApplyEffect();

            statusEffectList.Add(effect);
        }

        public void Update()
        {
            //delete inactive SE
            List<StatusEffect> effectToDelete = new List<StatusEffect>();
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
                    effectToDelete.Add(status);
                }
            }
            foreach (StatusEffect status in effectToDelete)
            {
                RemoveStatus(status);
            }
            effectToDelete.Clear();
        }

        public void RemoveStatus(StatusEffect status)
        {
            statusEffectList.Remove(status);
            Destroy(status.gameObject);
        }
    }
    
    [System.Flags]
    public enum EffectCategory
    {
        None,
        Buff,
        Debuff,
        Ohter
    }
}