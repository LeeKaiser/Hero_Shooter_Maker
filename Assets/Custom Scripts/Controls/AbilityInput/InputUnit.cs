using UnityEngine;
using InputOptions;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InputUnit", menuName = "Scriptable Objects/InputUnit")]
public class InputUnit : ScriptableObject
{
    [Tooltip("the button press the player makes to activate this input")]
    public InputEnum InputCombo;
    [Tooltip("Press - activates only when first pressed. Hold - activates while button is pressed down. Release - activate when button is released")]
    public InputType ComboInputType;
    [Tooltip("in the case of multiple valid input combos being made at same time, the one with higher priority activates")]
    public int Priority; //if multiple inputs are made at the same time, choose one with higher priority.

    //CompareInputToCombo
    //compares input to the InputCombo. return true if the input matches the combo and therefore should activate.
    public bool CompareInputToCombo(InputEnum pressInputs, InputEnum holdInputs, InputEnum releaseInputs)
    {
        switch (ComboInputType)
        {
            case InputType.Press:
                //TODO: would be very sensitive so find a way to make it more lenient if it is an issue
                return (InputCombo & pressInputs) > InputCombo;
            case InputType.Hold:
                return (InputCombo & holdInputs) > InputCombo;
            case InputType.Release:
                //TODO: would be very sensitive so find a way to make it more lenient if it is an issue
                return (InputCombo & releaseInputs) > InputCombo;
            default:
                return false;
        }
    }
}
