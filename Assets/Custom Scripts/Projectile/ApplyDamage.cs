using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerEvents;

public class ApplyDamage : MonoBehaviour
{
    [Tooltip("number of targets it can hit at a time")]
    public int MaxHits = 1;
    [Tooltip("damage it deals")]
    public int BaseDamage;
    [Tooltip("if true, summon an object in projectile info ObjectToSpawn variable")]
    public bool SpawnsObjectInInfo;
    [Tooltip("resets max hit every hit reset time. set to 0 if you want no reset.")]
    public float HitResetTime = 0.5f;

    ProjectileInfo info;
    List<CharCore> alreadyHitPlayers = new List<CharCore>();
    

    void Start()
    {
        info = GetComponent<ProjectileInfo>();
        StartCoroutine(CheckForCollision());
        StartCoroutine(ResetHits());
    }
    
    public void ResetHitPlayers()
    {
        alreadyHitPlayers.Clear();
    }

    IEnumerator ResetHits()
    {
        if (HitResetTime != 0f)
        {
            while (true)
            {
                yield return new WaitForSeconds(HitResetTime);
                ResetHitPlayers();
            }
        }
    }

    IEnumerator CheckForCollision()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (info.EnemyHit.Count > 0)
            {
                foreach(var target in info.EnemyHit)
                {
                    if (MaxHits <= 0)
                    {
                        break;
                    }
                    CharCore enemy = target.transform.parent.GetComponent<CharCore>();
                    if (alreadyHitPlayers.Contains(enemy))
                    {
                        continue;
                    }
                    int damageDealt = (int) (BaseDamage * info.OwningPlayer.GetDamageMult());
                    // deal damage to enemy player
                    damageDealt = enemy.DealDamage(damageDealt, info.OwningPlayer);

                    if (SpawnsObjectInInfo)
                    {
                        info.SpawnObject(target.point);
                    }

                    //add to already hit player so that it does not continuously hit the player.
                    alreadyHitPlayers.Add(enemy);
                    MaxHits -= 1;
                }
                
            }
        }
        
    }
}
