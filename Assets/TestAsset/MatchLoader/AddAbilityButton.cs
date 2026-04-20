using UnityEngine;

public class AddAbilityButton : MonoBehaviour
{
    public ActiveAbilityGroup ability;
    public TestMatchLoader loader;

    public void AddAbility()
    {
        loader.AddActiveAbility(ability);
    }
}
