using UnityEngine;
using System.Collections.Generic;

public class ProjectileInfo : MonoBehaviour
{
    public CharCore OwningPlayer;
    public TeamManager AttackAllegience;

    public LayerMask GroundLayer, TeamLayer, EnemyLayer;
    public GameObject DestroyEffect;
    public ProjectileInfo parentInfo;

    public List<RaycastHit> EnemyHit = new List<RaycastHit>();
    public List<RaycastHit> AllyHit = new List<RaycastHit>();
    public List<RaycastHit> ObstacleHit = new List<RaycastHit>();
    
    void Start()
    {
        TeamLayer = AttackAllegience.TeamLayer;
        EnemyLayer = AttackAllegience.EnemyLayer; 
    }

    public void DestroySelf(Vector3 position)
    {
        if (parentInfo != null)
        {
            parentInfo.gameObject.GetComponent<ProjectileGroup>().ProjectilesInGroup.Remove(gameObject);
        }
        GameObject destroyEffect = Instantiate(DestroyEffect, position, transform.rotation);
        Destroy(destroyEffect,1f);
        Destroy(gameObject);
    }

}
