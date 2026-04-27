using UnityEngine;

public abstract class ActiveAbility : Ability
{
    [HideInInspector] public ActiveAbilityID AbilityID;

    public float MinimumRange = 0;
    public float MaximumRange;

    public InputUnit AssociatedInput;

    //SetUpInput
    //sets up the input and ability connection
    public void SetUpInput()
    {
        if (AbilityID == null)
        {
            AbilityID = new ActiveAbilityID();
        }

        manager.SetupInput(this, AbilityID, AssociatedInput);
    }
}
