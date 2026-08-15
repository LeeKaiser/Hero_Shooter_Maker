using UnityEngine;
using System.Collections.Generic;
using HeroShooterMaker.EventBus;
using HeroShooterMaker.Client;
using HeroShooterMakerDemo; //for end of match text display and match reload
using UnityEngine.SceneManagement; //for reloading the match at end of the game

namespace HeroShooterMaker.MatchSystem
{
    public class GameManager : MonoBehaviour
    {
        // Singleton instance
        public static GameManager Instance { get; private set; }

        public TeamManager[] TeamsInMatch;
        public PlayerClient client;

        void Start()
        {
            Time.timeScale = 1f; 
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        void OnEnable()
        {
            EventBus<TeamCompleteObjective>.Subscribe(DeclareWinner);
        }

        void OnDisable()
        {
            EventBus<TeamCompleteObjective>.Unsubscribe(DeclareWinner);
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

        //Declare Winner
        //event called when a team completes their objective
        //Customize for your game
        public void DeclareWinner(TeamCompleteObjective teamComplete)
        {
            //something to show team winning
            Debug.Log("GameOver: " + teamComplete.TeamIdentity + "Won");
            client.GetComponent<UIMessageText>().ShowMessage(teamComplete.TeamIdentity.name + " Wins!", 0.4f);
            StopMatch();
        }

        public void StopMatch()
        {
            Time.timeScale = 0.2f; 
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Invoke("ReloadMatch", 0.4f);
        }

        public void ReloadMatch()
        {
            SceneManager.LoadScene(0);
        }
    }

    public struct TeamCompleteObjective
    {
        public TeamManager TeamIdentity;
    }
}
