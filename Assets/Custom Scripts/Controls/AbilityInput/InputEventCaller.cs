using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/*
Input Event Caller
takes input and invokes event based on it
intended to be used to activate abilities
*/
public class InputEventCaller : MonoBehaviour
{
    //variable
    //list of input made at the current frame
    [SerializeField] private List<InputOptions.Input> inputCurrentFrame = new List<InputOptions.Input>();

    //dictionary of input combination to ability call id
    public Dictionary<List<InputOptions.Input>, PlayerActiveAbilID>  InputDict = 
        new Dictionary<List<InputOptions.Input>, PlayerActiveAbilID>();

    //called every frame
    void LateUpdate()
    {
        PlayerActiveAbilID abilCall = null;
        List<InputOptions.Input> abilCallInput = null;
        
        foreach (KeyValuePair<List<InputOptions.Input>, PlayerActiveAbilID> inputCombo in InputDict)
        {
            //if some of the input matches an existing combination, attempt to add to next event invoke
            if (!inputCombo.Key.Except(inputCurrentFrame).Any())
            {
                if (abilCall == null)
                {
                    abilCall = inputCombo.Value;
                    abilCallInput = inputCombo.Key;
                }
                //put more complex combos at higher priority
                else if (abilCallInput.Count < inputCombo.Key.Count)
                {
                    abilCall = inputCombo.Value;
                    abilCallInput = inputCombo.Key;
                }
                
            }
        }
        //invokes event
        if (!(abilCall == null))
        {
            EventBus<PlayerActiveAbilID>.Invoke(abilCall);
        }
        //clear input for next frame.
        inputCurrentFrame.Clear();
    }

    //register input
    public void AddInput(InputOptions.Input input)
    {
        inputCurrentFrame.Add(input);
    }
}
