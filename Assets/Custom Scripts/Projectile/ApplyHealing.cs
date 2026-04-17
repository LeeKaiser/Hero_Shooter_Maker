using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerEvents;

public class ApplyHealing : MonoBehaviour
{
    [Tooltip("number of targets it can hit at a time")]
    public int MaxHits = 1;
    [Tooltip("amount of healing applied")]
    public int BaseHealing;
    [Tooltip("Set to true if healing should apply to self")]
    public bool healSelf;
    [Tooltip("if true, summon an object in projectile info ObjectToSpawn variable")]
    public bool SpawnsObjectInInfo;
    [Tooltip("resets max hit every hit reset time. set to 0 if you want no reset.")]
    public float HitResetTime;

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
            if (info.AllyHit.Count > 0)
            {
                foreach(var target in info.AllyHit)
                {
                    if (MaxHits <= 0)
                    {
                        break;
                    }
                    CharCore ally = target.transform.parent.GetComponent<CharCore>();
                    if (alreadyHitPlayers.Contains(ally))
                    {
                        continue;
                    }
                    if (!healSelf && ally == info.OwningPlayer)
                    {
                        continue;
                    }
                    int healthHealed = (int) (BaseHealing /** info.OwningPlayer.GetDamageMult()*/);
                    // heal ally
                    healthHealed = ally.HealHealth(healthHealed, info.OwningPlayer);

                    if (SpawnsObjectInInfo)
                    {
                        info.SpawnObject(target.point);
                    }

                    //add to already hit player so that it does not continuously hit the player.
                    alreadyHitPlayers.Add(ally);
                    MaxHits -= 1;
                }
                
            }
        }
        
    }
}
