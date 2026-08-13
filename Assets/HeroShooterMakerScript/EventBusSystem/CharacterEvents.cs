using UnityEngine;
using HeroShooterMaker.Abilities;
using HeroShooterMaker.Character;

//CharacterEvents
//structs used for transfering information based on events related to characters
namespace HeroShooterMaker.CharacterEvents
{
    //struct for calling events when the player dies
    public struct PlayerDead
    {
        public CharCore PlayerIdentity;
        public CharCore PlayerKiller;
    }

    //struct for when player takes damage
    public struct PlayerTakeDamage
    {
        public int Damage;
        public CharCore PlayerIdentity;
        public CharCore DamageDealer;
    }

    public struct PlayerHealHealth
    {
        public int Healing;
        public CharCore PlayerIdentity;
        public CharCore Healer;
    }

    public struct AddNewAbility
    {
        public CharCore PlayerIdentity;
        public Ability AddedAbility;
    }

    public struct RemoveNewAbility
    {
        public CharCore PlayerIdentity;
        public Ability RemovedAbility;
    }

    public struct UseAbility
    {
        public CharCore PlayerIdentity;
        public Ability UsedAbility;
    }
}
