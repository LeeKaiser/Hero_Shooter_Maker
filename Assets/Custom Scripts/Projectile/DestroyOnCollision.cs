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

    public bool SpawnsObjectInInfo;

    int totalHits;
    ProjectileInfo info;
    

    void Start()
    {
        info = GetComponent<ProjectileInfo>();
        StartCoroutine(CheckForCollision());
    }


    IEnumerator CheckForCollision()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            Vector3? closestPoint = null;
            float closestDistance = -1;
            bool doDestroy = false;
            bool[] allBools = {DestroyFromObstacle, DestroyFromEnemy, DestroyFromAlly};
            int[] allInts = {MaxHitsObstacle, MaxHitsEnemy,MaxHitsAlly};
            List<RaycastHit>[] allHits = {info.ObstacleHit, info.EnemyHit, info.AllyHit};
            for (int i = 0; i < allBools.Length; i++)
            {
                if (allHits[i].Count > 0 && allBools[i])
                {
                    foreach(RaycastHit target in allHits[i])
                    {
                        allInts[i] -= 1;
                        if (allInts[i] <= 0)
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
            }

            MaxHitsObstacle = allInts[0];
            MaxHitsEnemy = allInts[1];
            MaxHitsAlly = allInts[2];
            if (doDestroy && closestPoint != null)
            {
                info.DestroySelf(closestPoint ?? transform.position);
                if (SpawnsObjectInInfo)
                {
                    info.SpawnObject(closestPoint ?? transform.position);
                }
            }
        }
        

    }


}
