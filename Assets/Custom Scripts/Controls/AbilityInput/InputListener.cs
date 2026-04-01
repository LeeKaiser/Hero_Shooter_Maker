using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using StarterAssets;
using InputOptions;
/*
InputListener
Sends player’s inputs to InputEventCaller
*/
public class InputListener : MonoBehaviour
{
    
    private PlayerInput playerInput;
    private InputEventCaller inputRead;
    protected Dictionary<InputAction, Input> inputDict = new Dictionary<InputAction, Input>();
    
    void Awake()
    {
        //initialize playerinput
        playerInput = GetComponent<PlayerInput>();
        inputRead = GetComponent<InputEventCaller>();

        //TODO: figure out a better way to initialize inputDict later
        inputDict.Add(playerInput.actions["Num[1]"],Input.Num1 );
        inputDict.Add(playerInput.actions["Num[2]"],Input.Num2 );
        inputDict.Add(playerInput.actions["Num[3]"],Input.Num3 );
        inputDict.Add(playerInput.actions["Num[4]"],Input.Num4 );
        inputDict.Add(playerInput.actions["Num[5]"],Input.Num5 );
        inputDict.Add(playerInput.actions["Num[6]"],Input.Num6 );
        inputDict.Add(playerInput.actions["Num[7]"],Input.Num7 );
        inputDict.Add(playerInput.actions["Num[8]"],Input.Num8 );
        inputDict.Add(playerInput.actions["Num[9]"],Input.Num9 );

        inputDict.Add(playerInput.actions["Ability[E]"],Input.AbilE );
        inputDict.Add(playerInput.actions["Ability[Q]"],Input.AbilQ );
        inputDict.Add(playerInput.actions["Ability[F]"],Input.AbilF );
        inputDict.Add(playerInput.actions["Ability[R]"],Input.AbilR );
        inputDict.Add(playerInput.actions["Ability[Z]"],Input.AbilZ );
        inputDict.Add(playerInput.actions["Ability[X]"],Input.AbilX );
        inputDict.Add(playerInput.actions["Ability[C]"],Input.AbilC );
        inputDict.Add(playerInput.actions["Ability[V]"],Input.AbilV );

        inputDict.Add(playerInput.actions["Movement[Lshift]"],Input.MoveLShift );
        inputDict.Add(playerInput.actions["Movement[Lctrl]"],Input.MoveLCtrl );

        inputDict.Add(playerInput.actions["Attack[L]"],Input.AtkL );
        inputDict.Add(playerInput.actions["Attack[R]"],Input.AtkR );

        inputDict.Add(playerInput.actions["Misc[I]"],Input.MiscI );
        inputDict.Add(playerInput.actions["Misc[O]"],Input.MiscO );
        inputDict.Add(playerInput.actions["Misc[M]"],Input.MiscM );
    }
    
    void Update()
    {
        foreach (KeyValuePair<InputAction, Input> x in inputDict)
        {
            if (x.Key.WasPressedThisFrame())
            {
                
            }
            if (x.Key.IsPressed())
            {
                inputRead.AddInput(x.Value);
            }
            if (x.Key.WasReleasedThisFrame())
            {
                
            }
        }
    }
    

    
    
}
