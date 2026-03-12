using UnityEngine;

public abstract class AbilityUI : MonoBehaviour
{
    //reference to ability
    public Ability abilityRef;

    //called when UI is added
    public abstract void Initialize();

    //called on update
    public abstract void UpdateUI();
}
