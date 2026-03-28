using UnityEngine;

namespace PlayerEvents
{
    public struct PlayerDead
    {
        public CharCore PlayerIdentity;
    }

    public struct PlayerTakeDamage
    {
        public int Damage;
        public CharCore PlayerIdentity;
    }

    public struct PlayerDealDamage
    {
        public int Damage;
        public CharCore PlayerIdentity;
    }

    public struct PlayerHealHealth
    {
        public int Healing;
        public CharCore PlayerIdentity;
    }


}
