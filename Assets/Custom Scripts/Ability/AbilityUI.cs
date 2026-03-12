using UnityEngine;

public abstract class AbilityUI : MonoBehaviour
{
    public Ability abilityRef;
    public abstract void Initialize();

    public abstract void UpdateUI();
}
