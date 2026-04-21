using UnityEngine;

public class ClientAbilityButton : MonoBehaviour
{
    public TestMatchLoader matchLoader;
    public int abilityIndex;
    public AbilitySlotManagement abilitySlot;
    public PassiveAbilitySlotManagement passiveSlot;
    public bool AddPassive;
    
    public void AddAbilityToClient()
    {
        CharAssembleInfo assemble = matchLoader.CharAssemble;
        if (AddPassive)
        {
            passiveSlot.AddToAssember(assemble,abilityIndex);
        }
        else
        {
            abilitySlot.AddToAssember(assemble,abilityIndex);
        }
        
    }
}
