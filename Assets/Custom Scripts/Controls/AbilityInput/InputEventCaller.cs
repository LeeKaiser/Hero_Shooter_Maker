using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using InputOptions;

/*
InputEventCaller
Converts inputs to event calls of type ActiveAbilityID. 
*/
public class InputEventCaller : MonoBehaviour
{

    private InputEnum inputPressCurrentFrame = 0; 
    private InputEnum inputHoldCurrentFrame = 0;
    private InputEnum InputReleaseCurrentFrame = 0;


    public Dictionary<InputUnit, ActiveAbilityID>  InputDict = new Dictionary<InputUnit, ActiveAbilityID>();

    
    void LateUpdate()
    {
        ActiveAbilityID abilCall = null;
        InputUnit abilCallInput = null;
        //put more complex combos at higher priority
        foreach (KeyValuePair<InputUnit, ActiveAbilityID> inputCombo in InputDict)
        {
            //if the output's combo has keys not in user's input, lists in this var and skip the next if statement
            
            if (inputCombo.Key.CompareInputToCombo(inputPressCurrentFrame,inputHoldCurrentFrame,InputReleaseCurrentFrame))
            {
                if (abilCall == null || abilCallInput.Priority < inputCombo.Key.Priority)
                {
                    abilCall = inputCombo.Value;
                    abilCallInput = inputCombo.Key;
                }
                
            }
        }
        //Type eventType = abilCall.GetType(); 
        if (!(abilCall == null))
        {
            EventBus<ActiveAbilityID>.Invoke(abilCall);
        }
        //clear input for next frame.
        inputPressCurrentFrame = 0;
        inputHoldCurrentFrame = 0;
        InputReleaseCurrentFrame = 0;
    }

    public void AddHoldInput(InputEnum input)
    {
        inputHoldCurrentFrame = inputHoldCurrentFrame | input;
    }

    public void AddPressInput(InputEnum input)
    {
        inputPressCurrentFrame = inputPressCurrentFrame | input;
    }

    public void AddReleaseInput(InputEnum input)
    {
        InputReleaseCurrentFrame = InputReleaseCurrentFrame | input;
    }
}
