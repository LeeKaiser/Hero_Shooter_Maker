using UnityEngine;

public class AddAbilityButton : MonoBehaviour
{
    public InputUnit input;
    public GameObject ability;
    public TestMatchLoader loader;

    public void AddAbility()
    {
        loader.AddActiveAbility(input,ability);
    }
}
