using UnityEngine;

/* 
AIAction
Abstract Parent class which determines how AI behaves. 
*/
public abstract class AIAction : ScriptableObject
{
    public Transform MoveTarget;
    public Transform AimTarget;
    public ObjectDetection Detection;
    public InputEventCaller InputCall;
    public AIMovement Movement;

    public Ability abilityToUse = null;
    protected InputUnit abilityInput = null;
    protected float inputHoldTime;
    public bool HoldingInput = false; //if AI was not holding input, first press then set holding input to true. 
    //  while holding input is true, continuously make hold input, 
    // when changing from holding input from true to false, make release input

    public void Init(Transform movement, Transform aim, ObjectDetection detection, InputEventCaller input, AIMovement move)
    {
        MoveTarget = movement;
        AimTarget = aim;
        Detection = detection;
        InputCall = input;
        Movement = move;
    }

    public void MakeInput()
    {
        if (abilityToUse != null)
        {
            if (!HoldingInput)
            {
                PressInput();
            }
            else if (inputHoldTime > 0)
            {
                HoldInput();
            }
            else if (inputHoldTime <= 0 || abilityToUse.GetCurrentCharge() <= 0)
            {
                ReleaseInput();
                abilityToUse = null;
                abilityInput = null;
            }
            inputHoldTime -= Time.deltaTime;
        }
    }

    public void PressInput()
    {
        HoldingInput = true;
        InputCall.AddPressInput(abilityInput.InputCombo);
    }

    public void HoldInput()
    {
        InputCall.AddHoldInput(abilityInput.InputCombo);
    }

    public void ReleaseInput()
    {
        HoldingInput = false;
        InputCall.AddReleaseInput(abilityInput.InputCombo);
    }

    public abstract void DetermineMovement();
    public abstract void DetermineAim();
    public abstract void DetermineInput();

}
