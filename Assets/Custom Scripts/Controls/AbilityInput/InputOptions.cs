using UnityEngine;

public static class InputOptions
{
    //enum of input options
    public enum Input
    {
        //number keys
        Num1,Num2,Num3, Num4,Num5,Num6,Num7,Num8,Num9,

        //letter keys near movement keys (WASD)
        AbilE,AbilQ,AbilF,AbilR,AbilZ,AbilX,AbilC,AbilV,

        //keys for pinky associated with movement 
        MoveLShift,MoveLCtrl,

        //mouse input
        AtkL,AtkR,

        //letter keys away from left hand
        MiscI,MiscO,MiscM,
    }
}

//class used as parameter when invoking events associated with abilities. functions as a key of sorts
public class PlayerActiveAbilID{}
