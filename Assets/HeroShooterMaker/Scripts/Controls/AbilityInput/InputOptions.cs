using UnityEngine;
using System.Collections.Generic;



namespace HeroShooterMaker.Controls
{
    [System.Flags]
    public enum InputEnum
    {
        Num1 = 1 << 0,
        Num2 = 1 << 1,
        Num3 = 1 << 2,
        Num4 = 1 << 3,
        Num5 = 1 << 4,
        Num6 = 1 << 5,
        Num7 = 1 << 6,
        Num8 = 1 << 7,
        Num9 = 1 << 8,

        AbilE = 1 << 9,
        AbilQ = 1 << 10,
        AbilF = 1 << 11,
        AbilR = 1 << 12,
        AbilZ = 1 << 13,
        AbilX = 1 << 14,
        AbilC = 1 << 15,
        AbilV = 1 << 16,

        MoveLShift = 1 << 17,
        MoveLCtrl = 1 << 18,

        AtkL = 1 << 19,
        AtkR = 1 << 20,

        MiscI = 1 << 21,
        MiscO = 1 << 22,
        MiscM = 1 << 23,
    }

    public enum InputType
    {
        Press = 1 << 0,
        Hold = 1 << 1,
        Release = 1 << 2
    }     

    public class ActiveAbilityID{}
}





