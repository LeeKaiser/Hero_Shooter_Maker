using UnityEngine;

public class GenerateDefaultForAllPlayers : MonoBehaviour
{
    public CharAssembleInfo defaultInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GenerateRandomDefault();
    }

    public void GenerateRandomDefault()
    {
        GameManager manager = GetComponent<GameManager>();
        foreach (TeamManager teams in manager.TeamsInMatch)
        {
            foreach (CharCore character in teams.TeamMembers)
            {
                CharAssembleInfo newInfo = Instantiate(defaultInfo);
                //for this demo, generate random selection of attack and abilities for all players
                AbilitySlotManagement[] slots = Object.FindObjectsByType<AbilitySlotManagement>();
                foreach (AbilitySlotManagement a in slots)
                {
                    int highestIndex = a.allAbilities.Count;

                    a.AddToAssember(newInfo, Random.Range(0,highestIndex));
                }
                character.GetComponent<CharAssembler>().assembleInfo = newInfo;
            } 
        }
    }
}
