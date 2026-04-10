using UnityEngine;

namespace PlayerEvents
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
}
