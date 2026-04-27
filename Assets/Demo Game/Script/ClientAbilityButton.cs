using UnityEngine;

public class ClientAbilityButton : MonoBehaviour
{
    public TestMatchLoader matchLoader;
    public int abilityIndex;
    public AbilitySlotManagement abilitySlot;
    
    public void AddAbilityToClient()
    {
        CharAssembleInfo assemble = matchLoader.CharAssemble;
        abilitySlot.AddToAssember(assemble,abilityIndex);
        
    }
}
