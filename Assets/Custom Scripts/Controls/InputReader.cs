using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class InputReader : MonoBehaviour
{

    [SerializeField] private List<InputOptions.Input> inputCurrentFrame = new List<InputOptions.Input>();

    public Dictionary<List<InputOptions.Input>, PlayerEventInfo>  InputDict = 
        new Dictionary<List<InputOptions.Input>, PlayerEventInfo>();

    void Update()
    {
        
    }
    
    void LateUpdate()
    {
        PlayerEventInfo abilCall = null;
        List<InputOptions.Input> abilCallInput = null;
        //put more complex combos at higher priority
        foreach (KeyValuePair<List<InputOptions.Input>, PlayerEventInfo> inputCombo in InputDict)
        {
            //if the output's combo has keys not in user's input, lists in this var and skip the next if statement
            //var inInputButNotOutput = inputCurrentFrame.Except(inputCombo.Key).ToList();
            
            if (!inputCombo.Key.Except(inputCurrentFrame).Any())
            {
                if (abilCall == null)
                {
                    abilCall = inputCombo.Value;
                    abilCallInput = inputCombo.Key;
                }
                else if (abilCallInput.Count < inputCombo.Key.Count)
                {
                    abilCall = inputCombo.Value;
                    abilCallInput = inputCombo.Key;
                }
                
            }
        }
        //Type eventType = abilCall.GetType(); 
        if (!(abilCall == null))
        {
            Debug.Log(abilCall);
            Type eventType = abilCall.GetType();

            Type busType = typeof(EventBus<>).MakeGenericType(eventType);

            var invokeMethod = busType.GetMethod("Invoke");

            invokeMethod.Invoke(null, new object[] { abilCall });
        }
        //clear input for next frame.
        inputCurrentFrame.Clear();
    }

    public void AddInput(InputOptions.Input input)
    {
        inputCurrentFrame.Add(input);
    }
}
