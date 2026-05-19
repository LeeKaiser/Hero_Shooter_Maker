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
    public InputConverter InputConvert;
    public AIMovement Movement;

    public ActiveAbility abilityToUse = null;
    protected InputUnit abilityInput = null;
    protected float inputHoldTime;
    public bool HoldingInput = false; //if AI was not holding input, first press then set holding input to true. 
    //  while holding input is true, continuously make hold input, 
    // when changing from holding input from true to false, make release input

    protected GameObject targetPlayer = null;
    protected GameObject playerArmature;
    public LayerMask obstacleMask;          // What counts as cover geometry

    public void Init(Transform movement, Transform aim, ObjectDetection detection, InputConverter input, AIMovement move)
    {
        MoveTarget = movement;
        AimTarget = aim;
        Detection = detection;
        InputConvert = input;
        Movement = move;
        playerArmature = Detection.GetCurrentContext().SelfSummary.SummarizedPlayerCharCore.PlayerArmature;
    }

    public void MakeInput()
    {
        if (abilityToUse != null)
        {
            //check if it should be used
            bool canUse = true; 
            if (targetPlayer != null)
            {
                Vector3 targetPos = targetPlayer.transform.position;
                targetPos.y += 1;
                Vector3 playerPos = playerArmature.transform.position;
                playerPos.y += 1;
                Vector3 direction = targetPos - playerPos;
                if (!abilityToUse.UseWhenObscured )
                {
                    canUse = !Physics.Raycast(playerPos, direction.normalized, direction.magnitude, obstacleMask);
                }
                if (!abilityToUse.UseWhenOutOfRange)
                {
                    canUse = direction.magnitude <= abilityToUse.MaximumRange && direction.magnitude >= abilityToUse.MinimumRange;
                }
            }
            
            if (canUse)
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
            }

            else
            {
                Debug.Log("Can't use abil");
            }
        }
        inputHoldTime -= Time.deltaTime;
    }

    public void PressInput()
    {
        HoldingInput = true;
        InputConvert.AddPressInput(abilityInput.InputCombo);
    }

    public void HoldInput()
    {
        InputConvert.AddHoldInput(abilityInput.InputCombo);
    }

    public void ReleaseInput()
    {
        HoldingInput = false;
        InputConvert.AddReleaseInput(abilityInput.InputCombo);
    }

    public abstract void DetermineMovement();
    public abstract void DetermineAim();
    public abstract void DetermineInput();

}
