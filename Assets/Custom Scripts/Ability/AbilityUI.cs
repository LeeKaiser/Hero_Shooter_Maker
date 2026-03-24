using UnityEngine;

/*
Ability UI
abstract parent for User Interface for the ability
*/
public abstract class AbilityUI : MonoBehaviour
{
    public Ability AbilityReference;
    public abstract void Initialize();

    public abstract void UpdateUI();
}
