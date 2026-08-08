using UnityEngine;
using System.Collections.Generic;
using InputOptions;
 
/*
InputConverter
Converts raw input into values other systems read (move/look/jump), and
separately resolves which queued ability input combo (if any) fired this
frame based on registered InputUnit -> ActiveAbilityID mappings.
*/
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
 
    private InputEnum _pressedThisFrame = 0;
    private InputEnum _heldThisFrame = 0;
    private InputEnum _releasedThisFrame = 0;
 
    public Dictionary<InputUnit, ActiveAbilityID> InputDict = new Dictionary<InputUnit, ActiveAbilityID>();
 
    private void LateUpdate()
    {
        ResolveAbilityInputForFrame();
        ClearFrameInputState();
    }
 
    private void ResolveAbilityInputForFrame()
    {
        InputUnit bestMatch = null;
        ActiveAbilityID bestMatchAbility = null;
 
        foreach (var entry in InputDict)
        {
            InputUnit combo = entry.Key;
 
            if (!combo.CompareInputToCombo(_pressedThisFrame, _heldThisFrame, _releasedThisFrame))
                continue;
 
            // prefer the highest-priority matching combo, so complex combos can override simpler ones
            if (bestMatch == null || combo.Priority > bestMatch.Priority)
            {
                bestMatch = combo;
                bestMatchAbility = entry.Value;
            }
        }
 
        if (bestMatchAbility != null)
        {
            EventBus<ActiveAbilityID>.Invoke(bestMatchAbility);
        }
    }
 
    private void ClearFrameInputState()
    {
        _pressedThisFrame = 0;
        _heldThisFrame = 0;
        _releasedThisFrame = 0;
    }
 
    public void AddHoldInput(InputEnum input) => _heldThisFrame |= input;
    public void AddPressInput(InputEnum input) => _pressedThisFrame |= input;
    public void AddReleaseInput(InputEnum input) => _releasedThisFrame |= input;
 
    public void MoveInput(Vector2 newMoveDirection) => move = newMoveDirection;
    public void LookInput(Vector2 newLookDirection) => look = newLookDirection;
    public void JumpInput(bool newJumpState) => jump = newJumpState;
 
    private void OnApplicationFocus(bool hasFocus)
    {
        ApplyCursorLockState(cursorLocked);
    }
 
    private void ApplyCursorLockState(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
