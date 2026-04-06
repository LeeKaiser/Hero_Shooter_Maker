using UnityEngine;
using PlayerEvents;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private CharCore[] TeamMembers;
    public Transform[] SpawnPositions;

    public LayerMask TeamLayer;
    public LayerMask EnemyLayer;

    public int SpawnTime;

    void Awake()
    {
        EventBus<PlayerDead>.Subscribe(SpawnPlayer);
        TeamLayer = 1 << gameObject.layer;
        EnemyLayer = ~TeamLayer & EnemyLayer;
        AssignTeam();
    }
    void OnDisable()
    {
        EventBus<PlayerDead>.Unsubscribe(SpawnPlayer);
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
