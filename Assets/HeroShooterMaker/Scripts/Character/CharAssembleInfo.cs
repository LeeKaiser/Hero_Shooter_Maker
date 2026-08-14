using UnityEngine;
using System.Collections.Generic;
using HeroShooterMaker.Controls;

namespace HeroShooterMaker.Character
{
    [CreateAssetMenu(fileName = "CharAssembleInfo", menuName = "Scriptable Objects/CharAssembleInfo")]
    public class CharAssembleInfo : ScriptableObject
    {
        public CharStats Stats;

        public List<GameObject> Abilities = new List<GameObject>(); // gameobject must have child of ability component


    }

}
