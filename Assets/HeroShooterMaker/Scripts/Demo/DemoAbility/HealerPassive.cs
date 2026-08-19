using UnityEngine;
using HeroShooterMaker.EventBus;
using HeroShooterMaker.Abilities;
using HeroShooterMaker.Projectile;
using System.Collections.Generic;


//HealerPassive
//makes the attack heal teammates when hitting them
//example of a passive ability that heals a certain player
namespace HeroShooterMakerDemo
{
    public class HealerPassive : Ability
    {
        public float HealPercentage;

        public List<Ability> AbilityToChange;

        protected override void Startup()
        {
            EventBus<HitTarget>.Subscribe(executeAbility);

            //if there is an attack, give it support save and support boost classification. 
            //Assumes the attack is not added after the passive is added
            //Assumes the attack are named properately
            foreach(Ability abil in manager.GetAbilList())
            {
                foreach(Ability x in AbilityToChange)
                {
                    if (abil.Stats.AbilityName == x.Stats.AbilityName)
                    {
                        Debug.Log("Found ability to change");
                        abil.CurrentAbilClass |= AbilityClass.SupportSave;
                        abil.CurrentAbilClass |= AbilityClass.SupportBoost;
                    }
                }
            }
        }

        public void executeAbility(HitTarget hitTarget)
        {
            if (hitTarget.PlayerIdentity != playerReference)
            {
                return;
            }
            if (hitTarget.TargetPlayer.PlayerAllegience != playerReference.PlayerAllegience)
            {
                return;
            }


            //heal target player
            int healAmount = (int)(hitTarget.onHit.GetComponent<ApplyDamage>().BaseDamage * HealPercentage);
            hitTarget.TargetPlayer.HealHealth(healAmount,playerReference);
            
        }

        void Update()
        {
            
        }

        public override void Cleanup()
        {
            EventBus<HitTarget>.Unsubscribe(executeAbility);

            //if there is an attack, give it support save and support boost classification
            foreach(Ability abil in manager.GetAbilList())
            {
                foreach(Ability x in AbilityToChange)
                {
                    if (abil.Stats.AbilityName == x.Stats.AbilityName)
                    {
                        abil.CurrentAbilClass &= ~AbilityClass.SupportSave;
                        abil.CurrentAbilClass &= ~AbilityClass.SupportBoost;
                    }
                }
            }
        }
    }
}