using UnityEngine;
using System.Collections;
using System.Linq;

public class AreaOfEffectCircle : MonoBehaviour
{
    public float EnemyHitRads = 5f;
    public float AllyHitRads = 5f;
    public float ObstacleHitRads = 5f;
    public float ProjectileHitRads = 5f;
    public float duration = 1;

    ProjectileInfo info;

    

    void Start()
    {
        info = GetComponent<ProjectileInfo>();
    }

    void FixedUpdate()
    {
        
        //spherecast from where it is to where it will be 
        info.ObstacleHit = Physics.SphereCastAll(transform.position, ObstacleHitRads, Vector3.one, 0, info.GroundLayer).ToList();
        info.EnemyHit = Physics.SphereCastAll(transform.position, EnemyHitRads, Vector3.one, 0, info.EnemyLayer).ToList();
        info.AllyHit = Physics.SphereCastAll(transform.position, AllyHitRads, Vector3.one, 0, info.TeamLayer).ToList();
        info.ProjectileHit = Physics.SphereCastAll(transform.position, ProjectileHitRads, Vector3.one, 0, info.ProjectileLayer).ToList();
        //report to parent projectileinfo if it exists
        if (info.parentInfo != null)
        {
            info.parentInfo.ObstacleHit.AddRange(info.ObstacleHit);
            info.parentInfo.EnemyHit.AddRange(info.EnemyHit);
            info.parentInfo.AllyHit.AddRange(info.EnemyHit);
            info.parentInfo.ProjectileHit.AddRange(info.ProjectileHit);
        }

        duration -= Time.fixedDeltaTime;

        //delete self if it expired;
        if (duration <= 0)
        {
            info.DestroySelf(transform.position);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ObstacleHitRads);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AllyHitRads);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyHitRads);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ProjectileHitRads);
    }
}
