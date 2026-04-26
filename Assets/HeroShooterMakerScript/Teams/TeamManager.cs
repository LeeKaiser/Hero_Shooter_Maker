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
    }
    void OnDisable()
    {
        EventBus<PlayerDead>.Unsubscribe(SpawnPlayer);
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
}
