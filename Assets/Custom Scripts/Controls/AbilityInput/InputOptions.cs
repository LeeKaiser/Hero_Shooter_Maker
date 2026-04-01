using UnityEngine;
using System.Collections.Generic;



namespace InputOptions
{
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

    public struct InputUnit
    {
        public Input InputCombo;
        public InputType ComboInputType;

        //CompareInputToCombo
        //compares input to the InputCombo. return true if the input matches the combo and therefore should activate.

        public bool CompareInputToCombo(InputEnum pressInputs, InputEnum holdInputs, InputEnum releaseInputs)
        {
            switch (ComboInputType)
            {
                case InputType.Press:
                    //TODO: would be very sensitive so find a way to make it more lenient if it is an issue
                    return InputCombo & pressInputs > InputCombo;
                case InputType.Hold:
                    return InputCombo & holdInputs > InputCombo;
                case InputType.Release:
                    return InputCombo & releaseInputs > InputCombo;
                default:
                    return false;
            }
        }
    }       
}

public class ActiveAbilityID{}



