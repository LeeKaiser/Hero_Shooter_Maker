using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMatchLoader : MonoBehaviour
{
    public CharAssembleInfo CharAssemble;

    public void AddActiveAbility(InputUnit input, GameObject ability)
    {
        
        CharAssemble.ActiveAbilityInput.Add(input,ability);
    }

    public void LoadMatch()
    {
        SceneManager.LoadScene(1);
    }
}
