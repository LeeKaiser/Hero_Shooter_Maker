using UnityEngine;
using UnityEditor;

namespace AbilityClassification
{
    // classification of abilities that allows AI to decide ways it uses the ability.
    [System.Flags]
    public enum AbilityClass
    {
        None                = 0,
        Active              = 1 << 0,            // abilities that activate from press of a button. Makes it legal for active use in decision making
        Damage              = 1 << 1,       // abilities that make the AI target the enemy when used.
        SupportSave         = 1 << 2,       // abilities that make the AI target available teammates who have high vuln value (likely to die imminently)
        SupportBoost        = 1 << 3,       // abilities that make the AI target available teammates who have high threat value (good situation to attack enemies)
        SelfSave            = 1 << 4,       // abilities that the AI prefers to use when its vuln value is high
        SelfBoost           = 1 << 5,       // abilities that the AI prefers to use when it is high threat or high vuln
        Zoning              = 1 << 6,       // abilities that makes the AI prefers to target at key locations instead of any players
        MobilEng            = 1 << 7,       // abilities that AI uses to reach its destination faster, preferred to use when its threat value is high
        MobilDis            = 1 << 8,       // abilities that the AI uses to reach its destination faster, preferred to use when its vuln value is high
        Shutdown            = 1 << 9,       // abilities that the AI prefers to use when enemy has high threat value, usually in an attempt to cause the enemy to fail their attack
        Skirmish            = 1 << 10,      // abilities that the AI prefers to use first when attempting to attack.
        LongTermPet         = 1 << 11,      // abilities that the AI prefers to target at nearby obstacles and away from enemies. intended to be used for abilities like turrets that is more effective in safe locations
        Parry               = 1 << 12,      // abilities that AI uses when an enemy projectile gets close to it.
    }
        
    
   
}
