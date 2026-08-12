using UnityEngine;
using HeroShooterMaker.EventBus;

//general abstract class for objectives
public abstract class ObjectiveSystem : MonoBehaviour
{

    public void GiveScoreToTeam(TeamManager team, int points)
    {
        TeamGetPoint teamPoint = new TeamGetPoint();
        teamPoint.TeamIdentity = team;
        teamPoint.pointQuantity = points;
        EventBus<TeamGetPoint>.Invoke(teamPoint);
    }
}
