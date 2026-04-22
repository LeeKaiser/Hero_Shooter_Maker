using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using InputOptions;

//converts input to variable values that can be used by other scripts to function in response to input
public class InputConverter : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

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
	public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;
	} 

	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}

	public void JumpInput(bool newJumpState)
	{
		jump = newJumpState;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
	
