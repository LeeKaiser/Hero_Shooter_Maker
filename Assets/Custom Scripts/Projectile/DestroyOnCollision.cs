using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DestroyOnCollision : MonoBehaviour
{
    [Tooltip("Amount of hits on obstacles to be destroyed")]
    public int MaxHitsObstacle = 1;
    [Tooltip("Amount of hits on enemies to be destroyed")]
    public int MaxHitsEnemy = 1;
    [Tooltip("Amount of hits on allies to be destroyed")]
    public int MaxHitsAlly = 1;

    [Tooltip("Gets destroyed from obstacles")]
    public bool DestroyFromObstacle = true;
    [Tooltip("Gets destroyed from enemies")]
    public bool DestroyFromEnemy = true;
    [Tooltip("Gets destroyed from allies")]
    public bool DestroyFromAlly = false;

    int totalHits;
    ProjectileInfo info;
    

    void Start()
    {
        info = GetComponent<ProjectileInfo>();
    }
    
    void FixedUpdate()
    {
        StartCoroutine(CheckForCollision());
    }


    IEnumerator CheckForCollision()
    {
        yield return new WaitForFixedUpdate();
        Vector3? closestPoint = null;
        float closestDistance = -1;
        bool doDestroy = false;
        if (info.ObstacleHit.Length > 0 && DestroyFromObstacle)
        {
            foreach(RaycastHit target in info.ObstacleHit)
            {
                MaxHitsObstacle -= 1;
                if (MaxHitsObstacle <= 0)
                {
                    doDestroy = true;
                    if (closestDistance == -1 || target.distance < closestDistance)
                    {
                        closestDistance = target.distance;
                        closestPoint = target.point;
                    }
                }
            }
        }
        if (info.EnemyHit.Length > 0 && DestroyFromEnemy)
        {
            foreach(RaycastHit target in info.EnemyHit)
            {
                MaxHitsEnemy -= 1;
                if (MaxHitsEnemy <= 0)
                {
                    doDestroy = true;
                    if (closestDistance == -1 || target.distance < closestDistance)
                    {
                        closestDistance = target.distance;
                        closestPoint = target.point;
                    }
                }
            }
        }
        if (info.AllyHit.Length > 0 && DestroyFromAlly)
        {
            foreach(RaycastHit target in info.AllyHit)
            {
                MaxHitsAlly -= 1;
                if (MaxHitsAlly <= 0)
                {
                    doDestroy = true;
                    if (closestDistance == -1 || target.distance < closestDistance)
                    {
                        closestDistance = target.distance;
                        closestPoint = target.point;
                    }
                }
            }
        }
        if (doDestroy && closestPoint != null)
        {
            info.DestroySelf(closestPoint ?? transform.position);
        }

    }


}
