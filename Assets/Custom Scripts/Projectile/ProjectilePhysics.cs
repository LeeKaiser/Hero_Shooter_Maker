using UnityEngine;
using System.Collections;
using System.Linq;

//update projectile's position and check collision
public class ProjectilePhysics : MonoBehaviour
{
    public float Speed = 25f;
    public float Gravity = 9.8f;
    public float EnemyHitRads = 0.25f;
    public float AllyHitRads = 0.25f;
    public float ObstacleHitRads = 0.25f;
    public float Range = 10f;
    
    Vector3 velocity;
    float duration;
    ProjectileInfo info;

    void Start()
    {
        info = GetComponent<ProjectileInfo>();
        duration = Range / Speed;
        velocity = Speed * transform.TransformDirection(Vector3.forward);
    }

    void FixedUpdate()
    {
        //delete self if it expired;
        if (duration <= 0)
        {
            info.DestroySelf(transform.position);
        }
        //update for gravity
        velocity.y = velocity.y - (Gravity * Time.fixedDeltaTime);
        //find position at next fixed update
        Vector3 nextPosition = transform.position + (velocity * Time.fixedDeltaTime);
        
        float distanceTraveled = (velocity * Time.fixedDeltaTime).magnitude;
        //spherecast from where it is to where it will be 
        info.ObstacleHit = Physics.SphereCastAll(transform.position, ObstacleHitRads, velocity.normalized, distanceTraveled, info.GroundLayer).ToList();
        info.EnemyHit = Physics.SphereCastAll(transform.position, EnemyHitRads, velocity.normalized, distanceTraveled, info.EnemyLayer).ToList();
        info.AllyHit = Physics.SphereCastAll(transform.position, AllyHitRads, velocity.normalized, distanceTraveled, info.TeamLayer).ToList();

        //report to parent projectileinfo if it exists
        if (info.parentInfo != null)
        {
            info.parentInfo.ObstacleHit.AddRange(info.ObstacleHit);
            info.parentInfo.EnemyHit.AddRange(info.EnemyHit);
            info.parentInfo.AllyHit.AddRange(info.EnemyHit);
        }
        transform.position = nextPosition;

        duration -= Time.fixedDeltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ObstacleHitRads);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AllyHitRads);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyHitRads);
    }

}
