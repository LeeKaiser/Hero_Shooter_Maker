using UnityEngine;
using System.Collections.Generic;

public class ProjectileInfo : MonoBehaviour
{
    public CharCore OwningPlayer;
    public TeamManager AttackAllegience;

    public LayerMask GroundLayer, TeamLayer, EnemyLayer, ProjectileLayer;
    public GameObject DestroyEffect;
    public ProjectileInfo parentInfo;

    public List<RaycastHit> EnemyHit = new List<RaycastHit>();
    public List<RaycastHit> AllyHit = new List<RaycastHit>();
    public List<RaycastHit> ObstacleHit = new List<RaycastHit>();
    public List<RaycastHit> ProjectileHit = new List<RaycastHit>();

    public GameObject ObjectToSpawn;
    
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
        if (DestroyEffect != null)
        {
            GameObject destroyEffect = Instantiate(DestroyEffect, position, transform.rotation);
            Destroy(destroyEffect,1f);
        }
        
        Destroy(gameObject);
    }

    public void SpawnObject(Vector3 position)
    {
        if (ObjectToSpawn != null)
        {
            GameObject newObject = Instantiate(ObjectToSpawn, position, transform.rotation);

            //if the object is a projectile
            ProjectileInfo atkInfo = newObject.GetComponent<ProjectileInfo>();
            if (atkInfo != null)
            {
                atkInfo.OwningPlayer = OwningPlayer;
                atkInfo.AttackAllegience = AttackAllegience;
            }
        }
    }

}
