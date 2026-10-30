using UnityEngine;
using System.Collections.Generic;

//helps projectile groups collect information
namespace HeroShooterMaker.Projectile
{
    
    [DefaultExecutionOrder(100)]
    public class ProjectileGroupHitCollector : MonoBehaviour
    {
        private ProjectileGroup pj;
        public void Awake()
        {
            pj = GetComponent<ProjectileGroup>();
        }

        public void FixedUpdate()
        {
            pj.info.ObstacleHit.Clear();
            pj.info.EnemyHit.Clear();
            pj.info.AllyHit.Clear();
            pj.info.ProjectileHit.Clear();

            foreach (GameObject projectile in pj.ProjectilesInGroup)
            {
                ProjectileInfo child = projectile.GetComponent<ProjectileInfo>();
                pj.info.ObstacleHit.AddRange(child.ObstacleHit);
                pj.info.EnemyHit.AddRange(child.EnemyHit);
                pj.info.AllyHit.AddRange(child.AllyHit);
                pj.info.ProjectileHit.AddRange(child.ProjectileHit);
            }
        }
    }
}