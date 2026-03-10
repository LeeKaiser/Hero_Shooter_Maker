using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using StarterAssets;

public class InputListener : MonoBehaviour
{
    
    private PlayerInput playerInput;
    private InputEventCaller inputRead;
    protected Dictionary<InputAction, InputOptions.Input> inputDict = new Dictionary<InputAction, InputOptions.Input>();
    
    void Awake()
    {
        //initialize playerinput
        playerInput = GetComponent<PlayerInput>();
        inputRead = GetComponent<InputEventCaller>();

        //figure out a better way to initialize inputDict later
        inputDict.Add(playerInput.actions["Num[1]"],InputOptions.Input.Num1 );
        inputDict.Add(playerInput.actions["Num[2]"],InputOptions.Input.Num2 );
        inputDict.Add(playerInput.actions["Num[3]"],InputOptions.Input.Num3 );
        inputDict.Add(playerInput.actions["Num[4]"],InputOptions.Input.Num4 );
        inputDict.Add(playerInput.actions["Num[5]"],InputOptions.Input.Num5 );
        inputDict.Add(playerInput.actions["Num[6]"],InputOptions.Input.Num6 );
        inputDict.Add(playerInput.actions["Num[7]"],InputOptions.Input.Num7 );
        inputDict.Add(playerInput.actions["Num[8]"],InputOptions.Input.Num8 );
        inputDict.Add(playerInput.actions["Num[9]"],InputOptions.Input.Num9 );

        inputDict.Add(playerInput.actions["Ability[E]"],InputOptions.Input.AbilE );
        inputDict.Add(playerInput.actions["Ability[Q]"],InputOptions.Input.AbilQ );
        inputDict.Add(playerInput.actions["Ability[F]"],InputOptions.Input.AbilF );
        inputDict.Add(playerInput.actions["Ability[R]"],InputOptions.Input.AbilR );
        inputDict.Add(playerInput.actions["Ability[Z]"],InputOptions.Input.AbilZ );
        inputDict.Add(playerInput.actions["Ability[X]"],InputOptions.Input.AbilX );
        inputDict.Add(playerInput.actions["Ability[C]"],InputOptions.Input.AbilC );
        inputDict.Add(playerInput.actions["Ability[V]"],InputOptions.Input.AbilV );

        inputDict.Add(playerInput.actions["Movement[Lshift]"],InputOptions.Input.MoveLShift );
        inputDict.Add(playerInput.actions["Movement[Lctrl]"],InputOptions.Input.MoveLCtrl );

        inputDict.Add(playerInput.actions["Attack[L]"],InputOptions.Input.AtkL );
        inputDict.Add(playerInput.actions["Attack[R]"],InputOptions.Input.AtkR );

        inputDict.Add(playerInput.actions["Misc[I]"],InputOptions.Input.MiscI );
        inputDict.Add(playerInput.actions["Misc[O]"],InputOptions.Input.MiscO );
        inputDict.Add(playerInput.actions["Misc[M]"],InputOptions.Input.MiscM );
    }
    
    void Update()
    {
        foreach (KeyValuePair<InputAction, InputOptions.Input> x in inputDict)
        {
            if (x.Key.IsPressed())
            {
                inputRead.AddInput(x.Value);
            }
        }
    }
    

    
    
}
