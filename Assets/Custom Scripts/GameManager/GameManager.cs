using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    public TeamManager[] TeamsInMatch;
    public PlayerClient client;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }
    public void GenerateMatch()
    {
        //load in map
        //generate team
        //generate team members with their character information
        Invoke("EnableMatchObjects", 0.1f);
    }

    void EnableMatchObjects()
    {
        client.gameObject.SetActive(true);
        foreach (var team in TeamsInMatch)
        {
            team.gameObject.SetActive(true);
        }
    }

    public void StopMatch()
    {
        client.gameObject.SetActive(false);
        foreach (var team in TeamsInMatch)
        {
            team.gameObject.SetActive(false);
        }
    }
}
