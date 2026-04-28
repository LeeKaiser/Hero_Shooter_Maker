using UnityEngine;
using PlayerEvents;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class TeamManager : MonoBehaviour
{
    public CharCore[] TeamMembers;
    public Transform[] SpawnPositions;

    public LayerMask TeamLayer;
    public LayerMask EnemyLayer;

    public int SpawnTime;

    private int points;
    public int PointsToWin;

    void Awake()
    {
        
        TeamLayer = 1 << gameObject.layer;
        EnemyLayer = ~TeamLayer & EnemyLayer;
        AssignTeam();
       
    }
    void OnEnable()
    {
        //TODO: spawn in all players at correct spawn location
        EventBus<PlayerDead>.Subscribe(SpawnPlayer);
        EventBus<TeamGetPoint>.Subscribe(GetPoints);
    }
    void OnDisable()
    {
        EventBus<PlayerDead>.Unsubscribe(SpawnPlayer);
        EventBus<TeamGetPoint>.Subscribe(GetPoints);
        //TODO: disable all players to reset them for next round
    }
    public void SpawnPlayer(PlayerDead defeatedPlayer)
    {
        if (TeamMembers.Contains(defeatedPlayer.PlayerIdentity))
        {
            StartCoroutine(SpawnAfterTime(defeatedPlayer.PlayerIdentity));
        }
    }

    public void AssignTeam()
    {
        foreach(CharCore member in TeamMembers)
        {
            member.PlayerAllegience = this;
        }
    }

    IEnumerator SpawnAfterTime(CharCore spawnPlayer)
    {
        yield return new WaitForSeconds(SpawnTime);
        //select random spawn position
        int randomSpawnIndex = Random.Range(0,SpawnPositions.Length);
        spawnPlayer.Spawn(SpawnPositions[randomSpawnIndex].position);
        
    }

    public void GetPoints(TeamGetPoint getPoint)
    {
        if (getPoint.TeamIdentity != this)
        {
            return;
        }
        else
        {
            points += getPoint.pointQuantity;
            Debug.Log("GotPoints: " + this + "current points: " + points);
            if (points >= PointsToWin)
            {
                TeamCompleteObjective completeObjective = new TeamCompleteObjective();
                completeObjective.TeamIdentity = this;
                EventBus<TeamCompleteObjective>.Invoke(completeObjective);
            }
        }
    }
}

public struct TeamGetPoint
{
    public TeamManager TeamIdentity;
    public int pointQuantity;
}
