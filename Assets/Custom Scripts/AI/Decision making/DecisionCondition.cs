using UnityEngine;

namespace DecisionCondition
{
    //add more condition as nessecary
    public enum decisionCondition
    {
        EnemyPresent,           //it is aware of an enemy
        EnemyClose,             //enemy is close by some proximity
        TeammatePresent,        //it is aware of an ally
        Random,                 //randomly return true, chance of true set in the param
    }
}
