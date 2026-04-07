using UnityEngine;
using System.Collections.Generic;

//manages a group of projectiles. is controlled by its own projectile info
public class ProjectileGroup : MonoBehaviour
{
    public List<GameObject> ProjectilesInGroup = new List<GameObject>();

    ProjectileInfo info;

    void Start()
    {
        foreach (Transform child in transform)
        {
            ProjectileInfo childInfo = child.gameObject.GetComponent<ProjectileInfo>();
            if (childInfo != null)
            {
                ProjectilesInGroup.Add(childInfo.gameObject);
            }
        }
        info = GetComponent<ProjectileInfo>();
        foreach (GameObject projectile in ProjectilesInGroup)
        {
            ProjectileInfo childInfo = projectile.GetComponent<ProjectileInfo>();
            childInfo.parentInfo = info;
            childInfo.OwningPlayer = info.OwningPlayer;
            childInfo.AttackAllegience = info.AttackAllegience;
            childInfo.TeamLayer = info.TeamLayer;
            childInfo.EnemyLayer = info.EnemyLayer;
            
        }
    }

    //destroy self when all child projectile is destroyed
    void FixedUpdate()
    {
        if (ProjectilesInGroup.Count <= 0)
        {
            info.DestroySelf(transform.position);
        }
    }
}
