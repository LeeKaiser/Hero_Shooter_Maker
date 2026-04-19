using UnityEngine;

public class GenerateDefaultForAllPlayers : MonoBehaviour
{
    public CharAssembleInfo defaultInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameManager manager = GetComponent<GameManager>();
        foreach (TeamManager teams in manager.TeamsInMatch)
        {
            foreach (CharCore character in teams.TeamMembers)
            {
                character.GetComponent<CharAssembler>().assembleInfo = Instantiate(defaultInfo);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
