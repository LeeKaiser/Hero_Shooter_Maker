using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections.Generic;
using System;
using InputOptions;
/*
InputListener
Sends player’s inputs to InputEventCaller
*/
public class InputListener : MonoBehaviour
{
    
    private PlayerInput playerInput;
    protected Dictionary<InputAction, InputEnum> inputDict = new Dictionary<InputAction, InputEnum>();
    private InputConverter inputConvert;
    
    void Awake()
    {
        //initialize playerinput
        playerInput = GetComponent<PlayerInput>();
        //inputRead = GetComponent<InputEventCaller>();

        //TODO: figure out a better way to initialize inputDict later
        inputDict.Add(playerInput.actions["Num[1]"],InputEnum.Num1 );
        inputDict.Add(playerInput.actions["Num[2]"],InputEnum.Num2 );
        inputDict.Add(playerInput.actions["Num[3]"],InputEnum.Num3 );
        inputDict.Add(playerInput.actions["Num[4]"],InputEnum.Num4 );
        inputDict.Add(playerInput.actions["Num[5]"],InputEnum.Num5 );
        inputDict.Add(playerInput.actions["Num[6]"],InputEnum.Num6 );
        inputDict.Add(playerInput.actions["Num[7]"],InputEnum.Num7 );
        inputDict.Add(playerInput.actions["Num[8]"],InputEnum.Num8 );
        inputDict.Add(playerInput.actions["Num[9]"],InputEnum.Num9 );

        inputDict.Add(playerInput.actions["Ability[E]"],InputEnum.AbilE );
        inputDict.Add(playerInput.actions["Ability[Q]"],InputEnum.AbilQ );
        inputDict.Add(playerInput.actions["Ability[F]"],InputEnum.AbilF );
        inputDict.Add(playerInput.actions["Ability[R]"],InputEnum.AbilR );
        inputDict.Add(playerInput.actions["Ability[Z]"],InputEnum.AbilZ );
        inputDict.Add(playerInput.actions["Ability[X]"],InputEnum.AbilX );
        inputDict.Add(playerInput.actions["Ability[C]"],InputEnum.AbilC );
        inputDict.Add(playerInput.actions["Ability[V]"],InputEnum.AbilV );

        inputDict.Add(playerInput.actions["Movement[Lshift]"],InputEnum.MoveLShift );
        inputDict.Add(playerInput.actions["Movement[Lctrl]"],InputEnum.MoveLCtrl );

        inputDict.Add(playerInput.actions["Attack[L]"],InputEnum.AtkL );
        inputDict.Add(playerInput.actions["Attack[R]"],InputEnum.AtkR );

        inputDict.Add(playerInput.actions["Misc[I]"],InputEnum.MiscI );
        inputDict.Add(playerInput.actions["Misc[O]"],InputEnum.MiscO );
        inputDict.Add(playerInput.actions["Misc[M]"],InputEnum.MiscM );
    }

    public void SetInputConverter(InputConverter inConvert){inputConvert = inConvert;}

    #if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        inputConvert.MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if(inputConvert.cursorInputForLook)
        {
            inputConvert.LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        inputConvert.JumpInput(value.isPressed);
    }
    #endif
    
    void Update()
    {
        foreach (KeyValuePair<InputAction, InputEnum> x in inputDict)
        {
            if (x.Key.WasPressedThisFrame())
            {
                inputConvert.AddPressInput(x.Value);
            }
            if (x.Key.IsPressed())
            {
                inputConvert.AddHoldInput(x.Value);
            }
            if (x.Key.WasReleasedThisFrame())
            {
                inputConvert.AddReleaseInput(x.Value);
            }
        }
    }    
}
